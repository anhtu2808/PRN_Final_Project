using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Concurrent;

namespace LaptopRentalManagement.BLL.Hubs
{
    [Authorize]
    public class BaseHub : Hub, IBaseHub
    {
        private static readonly ConcurrentDictionary<string, string> _userConnections = new();

        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task JoinUserRoom(string userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"User_{userId}");
        }

        public async Task LeaveUserRoom(string userId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"User_{userId}");
        }

        public override async Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier;
            if (!string.IsNullOrEmpty(userId))
            {
                _userConnections[userId] = Context.ConnectionId;
                await Groups.AddToGroupAsync(Context.ConnectionId, $"User_{userId}");
                
                // Join role-based groups
                if (Context.User?.IsInRole("Admin") == true)
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, "Admins");
                }
                else if (Context.User?.IsInRole("Customer") == true)
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, "Customers");
                }
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.UserIdentifier;
            if (!string.IsNullOrEmpty(userId))
            {
                _userConnections.TryRemove(userId, out _);
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"User_{userId}");
                
                if (Context.User?.IsInRole("Admin") == true)
                {
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, "Admins");
                }
                else if (Context.User?.IsInRole("Customer") == true)
                {
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, "Customers");
                }
            }

            await base.OnDisconnectedAsync(exception);
        }

        // Static methods for connection management
        internal static void AddUserConnection(string userId, string connectionId)
        {
            _userConnections[userId] = connectionId;
        }

        internal static void RemoveUserConnection(string userId)
        {
            _userConnections.TryRemove(userId, out _);
        }

        internal static string? GetConnectionId(string userId)
        {
            _userConnections.TryGetValue(userId, out string? connectionId);
            return connectionId;
        }

        internal static bool IsUserOnline(string userId)
        {
            return _userConnections.ContainsKey(userId);
        }

        internal static List<string> GetOnlineUsers()
        {
            return _userConnections.Keys.ToList();
        }
    }
} 