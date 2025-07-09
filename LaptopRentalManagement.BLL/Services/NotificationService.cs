using LaptopRentalManagement.BLL.Interfaces;

namespace LaptopRentalManagement.BLL.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IHubService _hubService;

        public NotificationService(IHubService hubService)
        {
            _hubService = hubService;
        }

        public async Task SendNotificationToUserAsync(string userId, string message, string type = "info")
        {
            var notificationData = new
            {
                message,
                type,
                timestamp = DateTime.UtcNow
            };

            await _hubService.SendToUserAsync(userId, "ReceiveNotification", notificationData);
        }

        public async Task SendNotificationToAdminsAsync(string message, string type = "info")
        {
            var notificationData = new
            {
                message,
                type,
                timestamp = DateTime.UtcNow
            };

            await _hubService.SendToGroupAsync("Admins", "ReceiveNotification", notificationData);
        }

        public async Task SendNotificationToAllAsync(string message, string type = "info")
        {
            var notificationData = new
            {
                message,
                type,
                timestamp = DateTime.UtcNow
            };

            await _hubService.SendToAllAsync("ReceiveNotification", notificationData);
        }

        public async Task SendOrderStatusUpdateAsync(string userId, int orderId, string status, string message)
        {
            var orderUpdateData = new
            {
                orderId,
                status,
                message,
                timestamp = DateTime.UtcNow
            };

            await _hubService.SendToUserAsync(userId, "OrderStatusUpdated", orderUpdateData);
        }

        public async Task SendNewOrderNotificationAsync(int orderId, string customerName, string laptopModel)
        {
            var orderData = new
            {
                orderId,
                customerName,
                laptopModel,
                message = $"New order from {customerName} for {laptopModel}",
                timestamp = DateTime.UtcNow
            };

            await _hubService.SendToGroupAsync("Admins", "NewOrderReceived", orderData);
        }

        public async Task SendOrderCancelledNotificationAsync(string userId, int orderId, string reason)
        {
            var cancelData = new
            {
                orderId,
                reason,
                message = $"Order #{orderId} has been cancelled. Reason: {reason}",
                timestamp = DateTime.UtcNow
            };

            await _hubService.SendToUserAsync(userId, "OrderCancelled", cancelData);
        }

        public async Task SendOrderApprovedNotificationAsync(string userId, int orderId, string laptopModel)
        {
            var approvalData = new
            {
                orderId,
                laptopModel,
                message = $"Your order for {laptopModel} has been approved!",
                timestamp = DateTime.UtcNow
            };

            await _hubService.SendToUserAsync(userId, "OrderApproved", approvalData);
        }

        public async Task SendOrderRejectedNotificationAsync(string userId, int orderId, string reason)
        {
            var rejectionData = new
            {
                orderId,
                reason,
                message = $"Your order #{orderId} has been rejected. Reason: {reason}",
                timestamp = DateTime.UtcNow
            };

            await _hubService.SendToUserAsync(userId, "OrderRejected", rejectionData);
        }
    }
} 