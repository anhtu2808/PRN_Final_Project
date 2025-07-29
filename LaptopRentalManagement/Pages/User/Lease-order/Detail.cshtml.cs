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
            return Page();
        }

        public async Task<IActionResult> OnPostApproveAsync(int orderId, int id)
        {
            await _orderService.ApproveAsync(orderId);
            TempData["Success"] = $"Order #{orderId} approved.";
            return RedirectToPage("/User/Lease-Order/Detail", new { id });
        }

        public async Task<IActionResult> OnPostRejectAsync(int orderId, int id)
        {
            await _orderService.RejectAsync(orderId);
            TempData["Success"] = $"Order #{orderId} rejected.";
            return RedirectToPage("/User/Lease-Order/Detail", new { id });
        }

        public async Task<IActionResult> OnPostConfirmReturnAsync(int orderId, int id)
        {
            await _orderService.ConfirmReturn(orderId);
            TempData["Success"] = $"Order #{orderId} marked as returned.";
            return RedirectToPage("/User/Lease-Order/Detail", new { id });
        }

        public async Task<IActionResult> OnPostDeliveringAsync(int orderId, int id)
        {
            await _orderService.SetStatusAsync(new()
            {
                OrderId = orderId,
                NewStatus = "Delivering"
            });
            TempData["Success"] = $"Order #{orderId} set to 'Delivering'.";
            return RedirectToPage("/User/Lease-Order/Detail", new { id });
        }

        public async Task<IActionResult> OnPostDeliveredAsync(int orderId, int id)
        {
            await _orderService.SetStatusAsync(new()
            {
                OrderId = orderId,
                NewStatus = "Renting"
            });
            TempData["Success"] = $"Order #{orderId} delivered successfully.";
            return RedirectToPage("/User/Lease-Order/Detail", new { id });
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

            TempData["Warning"] = $"Delivery for order #{orderId} failed. Status reverted to 'Delivering'.";

            return RedirectToPage("/User/Lease-Order/Detail", new { id });
        }
    }
}
