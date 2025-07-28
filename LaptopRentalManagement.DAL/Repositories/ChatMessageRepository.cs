using LaptopRentalManagement.DAL.Context;
using LaptopRentalManagement.DAL.Entities;
using LaptopRentalManagement.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LaptopRentalManagement.DAL.Repositories;

public class ChatMessageRepository : IChatMessageRepository
{
    private readonly LaptopRentalDbContext _context;

    public ChatMessageRepository(LaptopRentalDbContext context)
    {
        _context = context;
    }

    public async Task<ChatMessage?> GetByIdAsync(int messageId)
    {
        return await _context.ChatMessages
            .Include(m => m.Sender)
            .Include(m => m.ChatRoom)
            .FirstOrDefaultAsync(m => m.ChatMessageId == messageId);
    }

    public async Task<IEnumerable<ChatMessage>> GetByChatRoomIdAsync(int chatRoomId, int page = 1, int pageSize = 50)
    {
        return await _context.ChatMessages
            .Include(m => m.Sender)
            .Where(m => m.ChatRoomId == chatRoomId && !m.IsDeleted)
            .OrderByDescending(m => m.SentAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<ChatMessage>> GetUnreadMessagesAsync(int chatRoomId, int userId)
    {
        return await _context.ChatMessages
            .Include(m => m.Sender)
            .Where(m => m.ChatRoomId == chatRoomId && 
                       m.SenderId != userId && 
                       !m.IsRead && 
                       !m.IsDeleted)
            .OrderBy(m => m.SentAt)
            .ToListAsync();
    }

    public async Task<ChatMessage> CreateAsync(ChatMessage message)
    {
        _context.ChatMessages.Add(message);
        await _context.SaveChangesAsync();
        
        // Update ChatRoom's LastMessageAt
        var chatRoom = await _context.ChatRooms.FindAsync(message.ChatRoomId);
        if (chatRoom != null)
        {
            chatRoom.LastMessageAt = message.SentAt;
            await _context.SaveChangesAsync();
        }
        
        return message;
    }

    public async Task<ChatMessage> UpdateAsync(ChatMessage message)
    {
        _context.ChatMessages.Update(message);
        await _context.SaveChangesAsync();
        return message;
    }

    public async Task<bool> DeleteAsync(int messageId)
    {
        var message = await _context.ChatMessages.FindAsync(messageId);
        if (message == null) return false;

        message.IsDeleted = true;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<int> GetTotalMessagesCountAsync(int chatRoomId)
    {
        return await _context.ChatMessages
            .Where(m => m.ChatRoomId == chatRoomId && !m.IsDeleted)
            .CountAsync();
    }

    public async Task<bool> MarkAsReadAsync(int messageId)
    {
        var message = await _context.ChatMessages.FindAsync(messageId);
        if (message == null) return false;

        message.IsRead = true;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<int> MarkRoomMessagesAsReadAsync(int chatRoomId, int userId)
    {
        var messages = await _context.ChatMessages
            .Where(m => m.ChatRoomId == chatRoomId && 
                       m.SenderId != userId && 
                       !m.IsRead)
            .ToListAsync();

        foreach (var message in messages)
        {
            message.IsRead = true;
        }

        await _context.SaveChangesAsync();
        return messages.Count;
    }

    public async Task<ChatMessage?> GetLastMessageAsync(int chatRoomId)
    {
        return await _context.ChatMessages
            .Include(m => m.Sender)
            .Where(m => m.ChatRoomId == chatRoomId && !m.IsDeleted)
            .OrderByDescending(m => m.SentAt)
            .FirstOrDefaultAsync();
    }
} 