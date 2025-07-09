using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using LaptopRentalManagement.BLL.Hubs;

namespace LaptopRentalManagement.Hubs
{
    [Authorize]
    public class ChatHub : BaseHub
    {
        public async Task SendMessage(string receiverId, string message)
        {
            var senderId = Context.UserIdentifier;
            var senderName = Context.User?.Identity?.Name ?? "Unknown";
            
            var messageData = new
            {
                senderId,
                senderName,
                message,
                timestamp = DateTime.UtcNow
            };

            // Send to receiver
            await Clients.User(receiverId).SendAsync("ReceiveMessage", messageData);
            
            // Send back to sender for confirmation
            await Clients.Caller.SendAsync("MessageSent", messageData);
        }

        public async Task JoinChatRoom(string roomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"ChatRoom_{roomId}");
            await Clients.Group($"ChatRoom_{roomId}").SendAsync("UserJoined", new 
            { 
                userId = Context.UserIdentifier, 
                userName = Context.User?.Identity?.Name 
            });
        }

        public async Task LeaveChatRoom(string roomId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"ChatRoom_{roomId}");
            await Clients.Group($"ChatRoom_{roomId}").SendAsync("UserLeft", new 
            { 
                userId = Context.UserIdentifier, 
                userName = Context.User?.Identity?.Name 
            });
        }

        public async Task SendRoomMessage(string roomId, string message)
        {
            var messageData = new
            {
                senderId = Context.UserIdentifier,
                senderName = Context.User?.Identity?.Name ?? "Unknown",
                message,
                roomId,
                timestamp = DateTime.UtcNow
            };

            await Clients.Group($"ChatRoom_{roomId}").SendAsync("ReceiveRoomMessage", messageData);
        }

        public async Task StartTyping(string receiverId)
        {
            await Clients.User(receiverId).SendAsync("UserTyping", new 
            { 
                userId = Context.UserIdentifier, 
                userName = Context.User?.Identity?.Name 
            });
        }

        public async Task StopTyping(string receiverId)
        {
            await Clients.User(receiverId).SendAsync("UserStoppedTyping", new 
            { 
                userId = Context.UserIdentifier 
            });
        }
    }
} 