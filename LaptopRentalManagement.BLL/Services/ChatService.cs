using AutoMapper;
using LaptopRentalManagement.BLL.Interfaces;
using LaptopRentalManagement.DAL.Entities;
using LaptopRentalManagement.DAL.Interfaces;
using LaptopRentalManagement.Model.DTOs.Request;
using LaptopRentalManagement.Model.DTOs.Response.Chat;

namespace LaptopRentalManagement.BLL.Services;

public class ChatService : IChatService
{
    private readonly IChatRoomRepository _chatRoomRepository;
    private readonly IChatMessageRepository _chatMessageRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IMapper _mapper;

    public ChatService(
        IChatRoomRepository chatRoomRepository,
        IChatMessageRepository chatMessageRepository,
        IAccountRepository accountRepository,
        IMapper mapper)
    {
        _chatRoomRepository = chatRoomRepository;
        _chatMessageRepository = chatMessageRepository;
        _accountRepository = accountRepository;
        _mapper = mapper;
    }

    public async Task<ChatRoomResponse?> GetChatRoomAsync(int chatRoomId, int userId)
    {
        var chatRoom = await _chatRoomRepository.GetByIdAsync(chatRoomId);
        if (chatRoom == null) return null;

        var response = MapToChatRoomResponse(chatRoom, userId);
        return response;
    }

    public async Task<ChatRoomDetailResponse?> GetChatRoomWithMessagesAsync(int chatRoomId, int userId, int page = 1)
    {
        var chatRoom = await _chatRoomRepository.GetByIdAsync(chatRoomId);
        if (chatRoom == null) return null;

        var messages = await _chatMessageRepository.GetByChatRoomIdAsync(chatRoomId, page);
        var totalMessages = await _chatMessageRepository.GetTotalMessagesCountAsync(chatRoomId);

        var chatRoomResponse = MapToChatRoomResponse(chatRoom, userId);
        var messageResponses = messages.Select(m => MapToChatMessageResponse(m, userId)).ToList();

        return new ChatRoomDetailResponse
        {
            ChatRoom = chatRoomResponse,
            Messages = messageResponses,
            TotalMessages = totalMessages,
            HasMoreMessages = totalMessages > page * 50
        };
    }

    public async Task<IEnumerable<ChatRoomResponse>> GetCustomerChatRoomsAsync(int customerId)
    {
        var chatRooms = await _chatRoomRepository.GetByCustomerIdAsync(customerId);
        return chatRooms.Select(cr => MapToChatRoomResponse(cr, customerId));
    }

    public async Task<IEnumerable<ChatRoomResponse>> GetStaffChatRoomsAsync(int staffId)
    {
        var chatRooms = await _chatRoomRepository.GetByStaffIdAsync(staffId);
        return chatRooms.Select(cr => MapToChatRoomResponse(cr, staffId));
    }

    public async Task<IEnumerable<ChatRoomResponse>> GetUnassignedChatRoomsAsync()
    {
        var chatRooms = await _chatRoomRepository.GetUnassignedAsync();
        return chatRooms.Select(cr => MapToChatRoomResponse(cr, 0));
    }

    public async Task<ChatRoomResponse> CreateChatRoomAsync(CreateChatRoomRequest request)
    {
        var chatRoom = new ChatRoom
        {
            CustomerId = request.CustomerId,
            Subject = request.Subject,
            Status = "Open",
            CreatedAt = DateTime.UtcNow,
            IsCustomerActive = true,
            IsStaffActive = false
        };

        var createdChatRoom = await _chatRoomRepository.CreateAsync(chatRoom);
        
        // Reload with navigation properties
        var chatRoomWithNav = await _chatRoomRepository.GetByIdAsync(createdChatRoom.ChatRoomId);
        return MapToChatRoomResponse(chatRoomWithNav!, request.CustomerId);
    }

    public async Task<bool> AssignStaffAsync(AssignStaffRequest request)
    {
        var chatRoom = await _chatRoomRepository.GetByIdAsync(request.ChatRoomId);
        if (chatRoom == null) return false;

        var staff = await _accountRepository.GetByIdAsync(request.StaffId);
        if (staff == null || (staff.Role != "Admin" && staff.Role != "Staff")) return false;

        chatRoom.StaffId = request.StaffId;
        chatRoom.Status = "InProgress";
        chatRoom.IsStaffActive = true;

        await _chatRoomRepository.UpdateAsync(chatRoom);
        return true;
    }

    public async Task<bool> UpdateChatRoomStatusAsync(int chatRoomId, string status)
    {
        var chatRoom = await _chatRoomRepository.GetByIdAsync(chatRoomId);
        if (chatRoom == null) return false;

        chatRoom.Status = status;
        await _chatRoomRepository.UpdateAsync(chatRoom);
        return true;
    }

    public async Task<ChatSummaryResponse> GetChatSummaryAsync(int? staffId = null)
    {
        var allChatRooms = await _chatRoomRepository.GetAllAsync();
        
        if (staffId.HasValue)
        {
            allChatRooms = allChatRooms.Where(cr => cr.StaffId == staffId.Value);
        }

        var summary = new ChatSummaryResponse
        {
            TotalChatRooms = allChatRooms.Count(),
            OpenChatRooms = allChatRooms.Count(cr => cr.Status == "Open"),
            InProgressChatRooms = allChatRooms.Count(cr => cr.Status == "InProgress"),
            ResolvedChatRooms = allChatRooms.Count(cr => cr.Status == "Resolved"),
            ClosedChatRooms = allChatRooms.Count(cr => cr.Status == "Closed"),
            UnassignedChatRooms = allChatRooms.Count(cr => cr.StaffId == null),
            RecentChatRooms = allChatRooms
                .OrderByDescending(cr => cr.LastMessageAt ?? cr.CreatedAt)
                .Take(5)
                .Select(cr => MapToChatRoomResponse(cr, staffId ?? 0))
                .ToList()
        };

        return summary;
    }

    public async Task<ChatMessageResponse> SendMessageAsync(SendMessageRequest request, int senderId)
    {
        var message = new ChatMessage
        {
            ChatRoomId = request.ChatRoomId,
            SenderId = senderId,
            Content = request.Content,
            MessageType = request.MessageType,
            SentAt = DateTime.UtcNow,
            IsRead = false,
            IsDeleted = false
        };

        var createdMessage = await _chatMessageRepository.CreateAsync(message);
        
        // Reload with navigation properties
        var messageWithNav = await _chatMessageRepository.GetByIdAsync(createdMessage.ChatMessageId);
        return MapToChatMessageResponse(messageWithNav!, senderId);
    }

    public async Task<int> MarkMessagesAsReadAsync(int chatRoomId, int userId)
    {
        return await _chatMessageRepository.MarkRoomMessagesAsReadAsync(chatRoomId, userId);
    }

    public async Task<bool> CanAccessChatRoomAsync(int chatRoomId, int userId, string userRole)
    {
        var chatRoom = await _chatRoomRepository.GetByIdAsync(chatRoomId);
        if (chatRoom == null) return false;

        // Admin can access all chat rooms
        if (userRole == "Admin") return true;

        // Staff can access assigned chat rooms
        if (userRole == "Staff" && chatRoom.StaffId == userId) return true;

        // Customer can access their own chat rooms
        if (userRole == "Customer" && chatRoom.CustomerId == userId) return true;

        return false;
    }

    private ChatRoomResponse MapToChatRoomResponse(ChatRoom chatRoom, int currentUserId)
    {
        var lastMessage = _chatMessageRepository.GetLastMessageAsync(chatRoom.ChatRoomId).Result;
        var unreadCount = _chatRoomRepository.GetUnreadMessagesCountAsync(chatRoom.ChatRoomId, currentUserId).Result;

        return new ChatRoomResponse
        {
            ChatRoomId = chatRoom.ChatRoomId,
            CustomerId = chatRoom.CustomerId,
            CustomerName = chatRoom.Customer?.Name ?? "Unknown",
            CustomerEmail = chatRoom.Customer?.Email ?? "Unknown",
            StaffId = chatRoom.StaffId,
            StaffName = chatRoom.Staff?.Name,
            StaffEmail = chatRoom.Staff?.Email,
            Subject = chatRoom.Subject,
            Status = chatRoom.Status,
            CreatedAt = chatRoom.CreatedAt,
            LastMessageAt = chatRoom.LastMessageAt,
            IsCustomerActive = chatRoom.IsCustomerActive,
            IsStaffActive = chatRoom.IsStaffActive,
            UnreadMessagesCount = unreadCount,
            LastMessage = lastMessage?.Content
        };
    }

    private ChatMessageResponse MapToChatMessageResponse(ChatMessage message, int currentUserId)
    {
        return new ChatMessageResponse
        {
            ChatMessageId = message.ChatMessageId,
            ChatRoomId = message.ChatRoomId,
            SenderId = message.SenderId,
            SenderName = message.Sender?.Name ?? "Unknown",
            SenderRole = message.Sender?.Role ?? "Unknown",
            Content = message.Content,
            MessageType = message.MessageType,
            SentAt = message.SentAt,
            IsRead = message.IsRead,
            IsDeleted = message.IsDeleted,
            IsOwnMessage = message.SenderId == currentUserId
        };
    }
} 