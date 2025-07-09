using Microsoft.AspNetCore.Authorization;

namespace LaptopRentalManagement.Hubs
{
    [Authorize]
    public class NotificationHub : BaseHub
    {
        // Specific notification methods
        public async Task SendNotification(string userId, string message, string type = "info")
        {
            await Clients.User(userId).SendAsync("ReceiveNotification", new { message, type, timestamp = DateTime.UtcNow });
        }

        public async Task SendOrderUpdate(string userId, object orderData)
        {
            await Clients.User(userId).SendAsync("OrderUpdated", orderData);
        }

        public async Task SendSystemAlert(string message, string type = "warning")
        {
            await Clients.Group("Admins").SendAsync("SystemAlert", new { message, type, timestamp = DateTime.UtcNow });
        }
    }
} 