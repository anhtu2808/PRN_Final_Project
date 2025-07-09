namespace LaptopRentalManagement.BLL.Interfaces
{
    public interface IHubService
    {
        // Send to specific user
        Task SendToUserAsync(string userId, string method, object data);
        
        // Send to specific group
        Task SendToGroupAsync(string groupName, string method, object data);
        
        // Send to all connected clients
        Task SendToAllAsync(string method, object data);
        
        // Group management
        Task AddToGroupAsync(string connectionId, string groupName);
        Task RemoveFromGroupAsync(string connectionId, string groupName);
        
        // Connection management
        Task<bool> IsUserOnlineAsync(string userId);
        Task<List<string>> GetOnlineUsersAsync();
    }
} 