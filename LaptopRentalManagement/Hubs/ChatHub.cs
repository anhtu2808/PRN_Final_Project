using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using LaptopRentalManagement.BLL.Interfaces;
using System.Security.Claims;

namespace LaptopRentalManagement.Hubs;

    [Authorize]
public class ChatHub : Hub
{
    private readonly IChatService _chatService;
    private static readonly Dictionary<string, string> _userConnections = new();

    public ChatHub(IChatService chatService)
    {
        _chatService = chatService;
    }

    public async Task JoinChatRoom(int chatRoomId)
    {
        var userId = GetCurrentUserId();
        var userRole = GetCurrentUserRole();

        if (userId == null || userRole == null) return;

        var canAccess = await _chatService.CanAccessChatRoomAsync(chatRoomId, userId.Value, userRole);
        if (!canAccess) return;

        await Groups.AddToGroupAsync(Context.ConnectionId, $"ChatRoom_{chatRoomId}");
        
        // Mark messages as read when joining room
        await _chatService.MarkMessagesAsReadAsync(chatRoomId, userId.Value);
        
        // Notify others in the room that user joined
        await Clients.Group($"ChatRoom_{chatRoomId}").SendAsync("UserJoinedRoom", new
            { 
            UserId = userId.Value,
            UserName = Context.User?.Identity?.Name,
            UserRole = userRole,
            ChatRoomId = chatRoomId
            });
        }

    public async Task LeaveChatRoom(int chatRoomId)
    {
        var userId = GetCurrentUserId();
        var userRole = GetCurrentUserRole();

        if (userId == null || userRole == null) return;

        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"ChatRoom_{chatRoomId}");
        
        // Notify others in the room that user left
        await Clients.Group($"ChatRoom_{chatRoomId}").SendAsync("UserLeftRoom", new
            { 
            UserId = userId.Value,
            UserName = Context.User?.Identity?.Name,
            UserRole = userRole,
            ChatRoomId = chatRoomId
            });
        }

    public async Task SendMessage(int chatRoomId, string content, string messageType = "Text")
    {
        var userId = GetCurrentUserId();
        var userRole = GetCurrentUserRole();

        if (userId == null || userRole == null) return;

        var canAccess = await _chatService.CanAccessChatRoomAsync(chatRoomId, userId.Value, userRole);
        if (!canAccess) return;

        try
        {
            var sendMessageRequest = new Model.DTOs.Request.SendMessageRequest
            {
                ChatRoomId = chatRoomId,
                Content = content.Trim(),
                MessageType = messageType
            };

            var messageResponse = await _chatService.SendMessageAsync(sendMessageRequest, userId.Value);

            // Send message to all users in the chat room
            await Clients.Group($"ChatRoom_{chatRoomId}").SendAsync("ReceiveMessage", messageResponse);

            // Send notification to staff dashboard if customer sent message
            if (userRole == "Customer")
            {
                await Clients.Group("StaffDashboard").SendAsync("NewCustomerMessage", new
                {
                    ChatRoomId = chatRoomId,
                    CustomerName = messageResponse.SenderName,
                    Message = content.Length > 50 ? content.Substring(0, 50) + "..." : content
                });
            }
        }
        catch (Exception ex)
        {
            await Clients.Caller.SendAsync("Error", new
            {
                Message = "Failed to send message",
                Details = ex.Message
            });
        }
        }

    public async Task StartTyping(int chatRoomId)
    {
        var userId = GetCurrentUserId();
        var userName = Context.User?.Identity?.Name;

        if (userId == null) return;

        await Clients.GroupExcept($"ChatRoom_{chatRoomId}", Context.ConnectionId)
            .SendAsync("UserStartedTyping", new
            { 
                UserId = userId.Value,
                UserName = userName,
                ChatRoomId = chatRoomId
            });
    }

    public async Task StopTyping(int chatRoomId)
    {
        var userId = GetCurrentUserId();

        if (userId == null) return;

        await Clients.GroupExcept($"ChatRoom_{chatRoomId}", Context.ConnectionId)
            .SendAsync("UserStoppedTyping", new
            {
                UserId = userId.Value,
                ChatRoomId = chatRoomId
            });
        }

    public async Task JoinStaffDashboard()
    {
        var userRole = GetCurrentUserRole();
        
        if (userRole == "Admin" || userRole == "Staff")
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "StaffDashboard");
        }
    }

    public async Task LeaveStaffDashboard()
        {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, "StaffDashboard");
    }

    public override async Task OnConnectedAsync()
    {
        var userId = GetCurrentUserId();
        var userRole = GetCurrentUserRole();

        if (userId != null)
        {
            _userConnections[userId.Value.ToString()] = Context.ConnectionId;
            
            // Auto-join staff dashboard for staff/admin users
            if (userRole == "Admin" || userRole == "Staff")
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, "StaffDashboard");
            }
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = GetCurrentUserId();
        
        if (userId != null)
        {
            _userConnections.Remove(userId.Value.ToString());
        }

        await base.OnDisconnectedAsync(exception);
    }

    private int? GetCurrentUserId()
    {
        var userIdClaim = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                         Context.User?.FindFirst("AccountId")?.Value;
        
        return int.TryParse(userIdClaim, out var userId) ? userId : null;
    }

    private string? GetCurrentUserRole()
    {
        return Context.User?.FindFirst(ClaimTypes.Role)?.Value;
        }

    public static string? GetConnectionId(int userId)
    {
        return _userConnections.TryGetValue(userId.ToString(), out var connectionId) ? connectionId : null;
    }
} 