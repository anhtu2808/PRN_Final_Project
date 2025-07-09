namespace LaptopRentalManagement.BLL.Interfaces
{
    public interface INotificationService
    {
        Task SendNotificationToUserAsync(string userId, string message, string type = "info");
        Task SendNotificationToAdminsAsync(string message, string type = "info");
        Task SendNotificationToAllAsync(string message, string type = "info");
        Task SendOrderStatusUpdateAsync(string userId, int orderId, string status, string message);
        Task SendNewOrderNotificationAsync(int orderId, string customerName, string laptopModel);
        Task SendOrderCancelledNotificationAsync(string userId, int orderId, string reason);
        Task SendOrderApprovedNotificationAsync(string userId, int orderId, string laptopModel);
        Task SendOrderRejectedNotificationAsync(string userId, int orderId, string reason);
    }
} 