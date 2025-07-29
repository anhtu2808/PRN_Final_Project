using LaptopRentalManagement.BLL.DTOs.Request;
using LaptopRentalManagement.BLL.DTOs.Response;
using LaptopRentalManagement.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace LaptopRentalManagement.Pages.User.Lease_order
{
    public class DetailModel : PageModel
    {
        private readonly IOrderService _orderService;
        private readonly IOrderLogService _orderLogService;
        private readonly ITicketService _ticketService;

        public DetailModel(
        IOrderService orderService,
        IOrderLogService orderLogService,
        ITicketService ticketService)
        {
            _orderService = orderService;
            _orderLogService = orderLogService;
            _ticketService = ticketService;
        }

        public OrderResponse? Order { get; set; }
        public IList<OrderLogResponse> Logs = new List<OrderLogResponse>();
        public IList<TicketResponse> Tickets { get; set; } = new List<TicketResponse>();

        // Add properties for date validation
        public bool CanShowStatusButtons { get; set; }
        public bool IsOrderDateValid { get; set; }

        public async Task<IActionResult> OnGetAsync(int orderId)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("AccountId");
            if (!int.TryParse(userIdClaim, out var userId))
            {
                TempData["Error"] = "Please login to continue";
                return RedirectToPage("/Account/Login");
            }
            Order = await _orderService.GetByIdAsync(orderId);
            Logs = await _orderLogService.GetByOrderIdAsync(orderId);
            Tickets = await _ticketService.GetAllByOrderIdAsync(orderId);
            if (Order == null)
            {
                return NotFound();
            }

            // Validate date for showing status buttons
            ValidateOrderDate();

            return Page();
        }

        private void ValidateOrderDate()
        {
            if (Order?.Slots == null || !Order.Slots.Any())
            {
                CanShowStatusButtons = false;
                IsOrderDateValid = false;
                return;
            }

            var currentDate = DateOnly.FromDateTime(DateTime.UtcNow);
            var orderStartDate = Order.Slots.Min(s => s.SlotDate);
            var orderEndDate = Order.Slots.Max(s => s.SlotDate);

            // Show status buttons only if current date is within or before the order period
            // and the order is in appropriate status
            IsOrderDateValid = currentDate <= orderEndDate;
            
            // Additional logic: only allow status changes on or before the start date for certain statuses
            CanShowStatusButtons = Order.Status switch
            {
                "Pending" => currentDate <= orderStartDate, // Can approve/reject before start date
                "Approved" => currentDate <= orderStartDate, // Can start delivery before start date
                "Delivering" => currentDate <= orderEndDate, // Can mark delivered during order period
                _ => false
            };
        }

        public async Task<IActionResult> OnPostApproveAsync(int orderId, int id)
        {
            // Validate date before action
            Order = await _orderService.GetByIdAsync(orderId);
            ValidateOrderDate();
            
            if (!CanShowStatusButtons)
            {
                TempData["Error"] = "Cannot approve this order at this time. Order date has passed.";
                return RedirectToPage("/User/Lease-Order/Detail", new { orderId });
            }

            await _orderService.ApproveAsync(orderId);
            TempData["Success"] = $"Đã duyệt đơn #{orderId}.";
            return RedirectToPage("/User/Lease-Order/Detail", new { orderId });
        }

        public async Task<IActionResult> OnPostRejectAsync(int orderId, int id)
        {
            // Validate date before action
            Order = await _orderService.GetByIdAsync(orderId);
            ValidateOrderDate();
            
            if (!CanShowStatusButtons)
            {
                TempData["Error"] = "Cannot reject this order at this time. Order date has passed.";
                return RedirectToPage("/User/Lease-Order/Detail", new { orderId });
            }

            await _orderService.RejectAsync(orderId);
            TempData["Success"] = $"Đã từ chối đơn #{orderId}.";
            return RedirectToPage("/User/Lease-Order/Detail", new { orderId });
        }

        public async Task<IActionResult> OnPostConfirmReturnAsync(int orderId, int id)
        {
            await _orderService.ConfirmReturn(orderId);
            TempData["Success"] = $"Đơn #{orderId} đã được xác nhận là đã trả.";
            return RedirectToPage("/User/Lease-Order/Detail", new { orderId });
        }

        public async Task<IActionResult> OnPostDeliveringAsync(int orderId, int id)
        {
            // Validate date before action
            Order = await _orderService.GetByIdAsync(orderId);
            ValidateOrderDate();
            
            if (!CanShowStatusButtons)
            {
                TempData["Error"] = "Cannot start delivery at this time. Order date has passed.";
                return RedirectToPage("/User/Lease-Order/Detail", new { orderId });
            }

            await _orderService.SetStatusAsync(new()
            {
                OrderId = orderId,
                NewStatus = "Delivering"
            });
            TempData["Success"] = $"Đơn #{orderId} đang giao hàng.";
            return RedirectToPage("/User/Lease-Order/Detail", new { orderId });
        }

        public async Task<IActionResult> OnPostDeliveredAsync(int orderId, int id)
        {
            // Validate date before action
            Order = await _orderService.GetByIdAsync(orderId);
            ValidateOrderDate();
            
            if (!CanShowStatusButtons)
            {
                TempData["Error"] = "Cannot mark as delivered at this time. Order date has passed.";
                return RedirectToPage("/User/Lease-Order/Detail", new { orderId });
            }

            await _orderService.SetStatusAsync(new()
            {
                OrderId = orderId,
                NewStatus = "Renting"
            });
            TempData["Success"] = $"Đơn #{orderId} đã giao thành công.";
            return RedirectToPage("/User/Lease-Order/Detail", new { orderId });
        }

        [BindProperty]
        public List<IFormFile> Images { get; set; } = new();

        public async Task<IActionResult> OnPostDeliveredFailAsync(int orderId, int id, string reason)
        {
            var request = new OrderLogRequest
            {
                OrderId = orderId,
                NewStatus = "DeliveringFail",
                Reason = reason,
                Forms = Images
            };

            await _orderService.SetStatusAsync(request);

            TempData["Warning"] = $"Giao hàng đơn #{orderId} thất bại. Trạng thái quay về 'Delivering'.";

            return RedirectToPage("/User/Lease-Order/Detail", new { orderId });
        }
    }
}
