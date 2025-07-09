using Microsoft.AspNetCore.Authorization;

namespace LaptopRentalManagement.Hubs
{
    [Authorize]
    public class OrderHub : BaseHub
    {
        public async Task JoinOrderTracking(string orderId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"Order_{orderId}");
        }

        public async Task LeaveOrderTracking(string orderId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Order_{orderId}");
        }

        public async Task TrackOrder(string orderId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"Order_{orderId}");
            
            // Send current order status to the client
            await Clients.Caller.SendAsync("OrderTrackingStarted", new 
            { 
                orderId, 
                message = "You are now tracking this order",
                timestamp = DateTime.UtcNow 
            });
        }

        public async Task UpdateOrderStatus(string orderId, string status, string? notes = null)
        {
            var updateData = new
            {
                orderId,
                status,
                notes,
                updatedBy = Context.User?.Identity?.Name,
                timestamp = DateTime.UtcNow
            };

            // Send to all tracking this order
            await Clients.Group($"Order_{orderId}").SendAsync("OrderStatusUpdated", updateData);
            
            // Send to admins
            await Clients.Group("Admins").SendAsync("OrderStatusChanged", updateData);
        }

        public async Task SendOrderMessage(string orderId, string message)
        {
            var messageData = new
            {
                orderId,
                message,
                sender = Context.User?.Identity?.Name,
                senderId = Context.UserIdentifier,
                timestamp = DateTime.UtcNow
            };

            await Clients.Group($"Order_{orderId}").SendAsync("OrderMessage", messageData);
        }

        public async Task RequestOrderExtension(string orderId, DateTime newEndDate, string reason)
        {
            var extensionData = new
            {
                orderId,
                newEndDate,
                reason,
                requestedBy = Context.User?.Identity?.Name,
                requesterId = Context.UserIdentifier,
                timestamp = DateTime.UtcNow
            };

            // Notify admins about extension request
            await Clients.Group("Admins").SendAsync("OrderExtensionRequested", extensionData);
            
            // Confirm to requester
            await Clients.Caller.SendAsync("ExtensionRequestSent", extensionData);
        }

        public async Task ApproveOrderExtension(string orderId, DateTime newEndDate, string? adminNotes = null)
        {
            var approvalData = new
            {
                orderId,
                newEndDate,
                adminNotes,
                approvedBy = Context.User?.Identity?.Name,
                timestamp = DateTime.UtcNow
            };

            await Clients.Group($"Order_{orderId}").SendAsync("OrderExtensionApproved", approvalData);
        }

        public async Task RejectOrderExtension(string orderId, string reason)
        {
            var rejectionData = new
            {
                orderId,
                reason,
                rejectedBy = Context.User?.Identity?.Name,
                timestamp = DateTime.UtcNow
            };

            await Clients.Group($"Order_{orderId}").SendAsync("OrderExtensionRejected", rejectionData);
        }
    }
} 