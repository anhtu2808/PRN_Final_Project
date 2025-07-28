using LaptopRentalManagement.DAL.Context;
using LaptopRentalManagement.DAL.Entities;
using LaptopRentalManagement.DAL.Interfaces;
using LaptopRentalManagement.Model.DTOs.Request;
using Microsoft.EntityFrameworkCore;

namespace LaptopRentalManagement.DAL.Repositories;

public class ChatRoomRepository : IChatRoomRepository
{
    private readonly LaptopRentalDbContext _context;

    public ChatRoomRepository(LaptopRentalDbContext context)
    {
        _context = context;
    }

    public async Task<ChatRoom?> GetByIdAsync(int chatRoomId)
    {
        return await _context.ChatRooms
            .Include(cr => cr.Customer)
            .Include(cr => cr.Staff)
            .FirstOrDefaultAsync(cr => cr.ChatRoomId == chatRoomId);
    }

    public async Task<ChatRoom?> GetByIdWithMessagesAsync(int chatRoomId)
    {
        return await _context.ChatRooms
            .Include(cr => cr.Customer)
            .Include(cr => cr.Staff)
            .Include(cr => cr.Messages.OrderBy(m => m.SentAt))
                .ThenInclude(m => m.Sender)
            .FirstOrDefaultAsync(cr => cr.ChatRoomId == chatRoomId);
    }

    public async Task<IEnumerable<ChatRoom>> GetAllAsync()
    {
        return await _context.ChatRooms
            .Include(cr => cr.Customer)
            .Include(cr => cr.Staff)
            .OrderByDescending(cr => cr.LastMessageAt ?? cr.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<ChatRoom>> GetByCustomerIdAsync(int customerId)
    {
        return await _context.ChatRooms
            .Include(cr => cr.Customer)
            .Include(cr => cr.Staff)
            .Where(cr => cr.CustomerId == customerId)
            .OrderByDescending(cr => cr.LastMessageAt ?? cr.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<ChatRoom>> GetByStaffIdAsync(int staffId)
    {
        return await _context.ChatRooms
            .Include(cr => cr.Customer)
            .Include(cr => cr.Staff)
            .Where(cr => cr.StaffId == staffId)
            .OrderByDescending(cr => cr.LastMessageAt ?? cr.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<ChatRoom>> GetUnassignedAsync()
    {
        return await _context.ChatRooms
            .Include(cr => cr.Customer)
            .Where(cr => cr.StaffId == null && cr.Status == "Open")
            .OrderBy(cr => cr.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<ChatRoom>> SearchAsync(ChatRoomFilter filter)
    {
        var query = _context.ChatRooms
            .Include(cr => cr.Customer)
            .Include(cr => cr.Staff)
            .AsQueryable();

        if (!string.IsNullOrEmpty(filter.Status))
            query = query.Where(cr => cr.Status == filter.Status);

        if (filter.CustomerId.HasValue)
            query = query.Where(cr => cr.CustomerId == filter.CustomerId.Value);

        if (filter.StaffId.HasValue)
            query = query.Where(cr => cr.StaffId == filter.StaffId.Value);

        if (!string.IsNullOrEmpty(filter.Subject))
            query = query.Where(cr => cr.Subject.Contains(filter.Subject));

        if (filter.CreatedFrom.HasValue)
            query = query.Where(cr => cr.CreatedAt >= filter.CreatedFrom.Value);

        if (filter.CreatedTo.HasValue)
            query = query.Where(cr => cr.CreatedAt <= filter.CreatedTo.Value);

        return await query
            .OrderByDescending(cr => cr.LastMessageAt ?? cr.CreatedAt)
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();
    }

    public async Task<ChatRoom> CreateAsync(ChatRoom chatRoom)
    {
        _context.ChatRooms.Add(chatRoom);
        await _context.SaveChangesAsync();
        return chatRoom;
    }

    public async Task<ChatRoom> UpdateAsync(ChatRoom chatRoom)
    {
        _context.ChatRooms.Update(chatRoom);
        await _context.SaveChangesAsync();
        return chatRoom;
    }

    public async Task<bool> DeleteAsync(int chatRoomId)
    {
        var chatRoom = await _context.ChatRooms.FindAsync(chatRoomId);
        if (chatRoom == null) return false;

        _context.ChatRooms.Remove(chatRoom);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<int> GetUnreadMessagesCountAsync(int chatRoomId, int userId)
    {
        return await _context.ChatMessages
            .Where(m => m.ChatRoomId == chatRoomId && 
                       m.SenderId != userId && 
                       !m.IsRead)
            .CountAsync();
    }

    public async Task<bool> ExistsAsync(int chatRoomId)
    {
        return await _context.ChatRooms.AnyAsync(cr => cr.ChatRoomId == chatRoomId);
    }
} 