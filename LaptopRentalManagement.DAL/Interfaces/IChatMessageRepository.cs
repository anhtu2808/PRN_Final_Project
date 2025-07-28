using LaptopRentalManagement.DAL.Entities;

namespace LaptopRentalManagement.DAL.Interfaces;

public interface IChatMessageRepository
{
    Task<ChatMessage?> GetByIdAsync(int messageId);
    Task<IEnumerable<ChatMessage>> GetByChatRoomIdAsync(int chatRoomId, int page = 1, int pageSize = 50);
    Task<IEnumerable<ChatMessage>> GetUnreadMessagesAsync(int chatRoomId, int userId);
    Task<ChatMessage> CreateAsync(ChatMessage message);
    Task<ChatMessage> UpdateAsync(ChatMessage message);
    Task<bool> DeleteAsync(int messageId);
    Task<int> GetTotalMessagesCountAsync(int chatRoomId);
    Task<bool> MarkAsReadAsync(int messageId);
    Task<int> MarkRoomMessagesAsReadAsync(int chatRoomId, int userId);
    Task<ChatMessage?> GetLastMessageAsync(int chatRoomId);
} 