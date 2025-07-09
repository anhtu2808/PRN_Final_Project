using Microsoft.AspNetCore.SignalR;
using LaptopRentalManagement.BLL.Interfaces;
using LaptopRentalManagement.Hubs;
using System.Collections.Concurrent;

namespace LaptopRentalManagement.BLL.Services
{
    public class HubService : IHubService
    {
        private readonly IHubContext<BaseHub> _hubContext;
        private static readonly ConcurrentDictionary<string, string> _userConnections = new();
        private static readonly ConcurrentDictionary<string, HashSet<string>> _groupConnections = new();

        public HubService(IHubContext<BaseHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendToUserAsync(string userId, string method, object data)
        {
            if (_userConnections.TryGetValue(userId, out string? connectionId))
            {
                await _hubContext.Clients.Client(connectionId).SendAsync(method, data);
            }
        }

        public async Task SendToGroupAsync(string groupName, string method, object data)
        {
            await _hubContext.Clients.Group(groupName).SendAsync(method, data);
        }

        public async Task SendToAllAsync(string method, object data)
        {
            await _hubContext.Clients.All.SendAsync(method, data);
        }

        public async Task AddToGroupAsync(string connectionId, string groupName)
        {
            await _hubContext.Groups.AddToGroupAsync(connectionId, groupName);
            
            if (!_groupConnections.ContainsKey(groupName))
            {
                _groupConnections[groupName] = new HashSet<string>();
            }
            _groupConnections[groupName].Add(connectionId);
        }

        public async Task RemoveFromGroupAsync(string connectionId, string groupName)
        {
            await _hubContext.Groups.RemoveFromGroupAsync(connectionId, groupName);
            
            if (_groupConnections.ContainsKey(groupName))
            {
                _groupConnections[groupName].Remove(connectionId);
                if (_groupConnections[groupName].Count == 0)
                {
                    _groupConnections.TryRemove(groupName, out _);
                }
            }
        }

        public Task<bool> IsUserOnlineAsync(string userId)
        {
            return Task.FromResult(_userConnections.ContainsKey(userId));
        }

        public Task<List<string>> GetOnlineUsersAsync()
        {
            return Task.FromResult(_userConnections.Keys.ToList());
        }

        // Internal methods for connection management
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
    }
} 