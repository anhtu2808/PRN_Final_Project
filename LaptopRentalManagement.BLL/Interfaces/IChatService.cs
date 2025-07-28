using LaptopRentalManagement.Model.DTOs.Request;
using LaptopRentalManagement.Model.DTOs.Response.Chat;

namespace LaptopRentalManagement.BLL.Interfaces;

public interface IChatService
{
    // Chat Room operations
    Task<ChatRoomResponse?> GetChatRoomAsync(int chatRoomId, int userId);
    Task<ChatRoomDetailResponse?> GetChatRoomWithMessagesAsync(int chatRoomId, int userId, int page = 1);
    Task<IEnumerable<ChatRoomResponse>> GetCustomerChatRoomsAsync(int customerId);
    Task<IEnumerable<ChatRoomResponse>> GetStaffChatRoomsAsync(int staffId);
    Task<IEnumerable<ChatRoomResponse>> GetUnassignedChatRoomsAsync();
    Task<ChatRoomResponse> CreateChatRoomAsync(CreateChatRoomRequest request);
    Task<bool> AssignStaffAsync(AssignStaffRequest request);
    Task<bool> UpdateChatRoomStatusAsync(int chatRoomId, string status);
    Task<ChatSummaryResponse> GetChatSummaryAsync(int? staffId = null);
    
    // Message operations
    Task<ChatMessageResponse> SendMessageAsync(SendMessageRequest request, int senderId);
    Task<int> MarkMessagesAsReadAsync(int chatRoomId, int userId);
    Task<bool> CanAccessChatRoomAsync(int chatRoomId, int userId, string userRole);
} 