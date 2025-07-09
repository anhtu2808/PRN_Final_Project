using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using LaptopRentalManagement.BLL.Services;

namespace LaptopRentalManagement.Hubs
{
    [Authorize]
    public class BaseHub : Hub
    {
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
                HubService.AddUserConnection(userId, Context.ConnectionId);
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
                HubService.RemoveUserConnection(userId);
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
    }
} 