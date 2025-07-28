using LaptopRentalManagement.DAL.Entities;
using LaptopRentalManagement.Model.DTOs.Request;

namespace LaptopRentalManagement.DAL.Interfaces;

public interface IChatRoomRepository
{
    Task<ChatRoom?> GetByIdAsync(int chatRoomId);
    Task<ChatRoom?> GetByIdWithMessagesAsync(int chatRoomId);
    Task<IEnumerable<ChatRoom>> GetAllAsync();
    Task<IEnumerable<ChatRoom>> GetByCustomerIdAsync(int customerId);
    Task<IEnumerable<ChatRoom>> GetByStaffIdAsync(int staffId);
    Task<IEnumerable<ChatRoom>> GetUnassignedAsync();
    Task<IEnumerable<ChatRoom>> SearchAsync(ChatRoomFilter filter);
    Task<ChatRoom> CreateAsync(ChatRoom chatRoom);
    Task<ChatRoom> UpdateAsync(ChatRoom chatRoom);
    Task<bool> DeleteAsync(int chatRoomId);
    Task<int> GetUnreadMessagesCountAsync(int chatRoomId, int userId);
    Task<bool> ExistsAsync(int chatRoomId);
} 