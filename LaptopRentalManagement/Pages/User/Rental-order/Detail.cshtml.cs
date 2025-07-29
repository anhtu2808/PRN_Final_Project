using LaptopRentalManagement.BLL.DTOs.Request;
using LaptopRentalManagement.BLL.DTOs.Response;
using LaptopRentalManagement.BLL.Interfaces;
using LaptopRentalManagement.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace LaptopRentalManagement.Pages.User.Rental_order
{
    public class DetailModel : PageModel
    {
		private readonly IOrderService _orderService;
		private readonly IOrderLogService _orderLogService;
		private readonly ITicketService _ticketService;
		private readonly IHubContext<RentalHub> _hubContext;
		public DetailModel(
		IOrderService orderService,
		IOrderLogService orderLogService,
		ITicketService ticketService,
		IHubContext<RentalHub> hubContext)
		{
			_orderService = orderService;
			_orderLogService = orderLogService;
			_ticketService = ticketService;
			_hubContext = hubContext;
		}

		public OrderResponse? Order { get; set; }
		public IList<OrderLogResponse> Logs = new List<OrderLogResponse>();
		public IList<TicketResponse> Tickets { get; set; } = new List<TicketResponse>();

		[BindProperty]
		public CreateTicketRequest TicketRequest { get; set; }

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

		public async Task<IActionResult> OnPostConfirmReturnAsync(int orderId)
		{
			await _orderService.ConfirmReturn(orderId);

			await _hubContext.Clients.All.SendAsync("ReceiveOrderStatusUpdate");
			TempData["Success"] = $"Order #{orderId} marked as returned.";
			return RedirectToPage("/User/Rental-Order/Detail", new { orderId });
		}

		public async Task<IActionResult> OnPostCreateTicketAsync()
		{
			if (!ModelState.IsValid)
			{
				Order = await _orderService.GetByIdAsync(TicketRequest.OrderId);
				Logs = await _orderLogService.GetByOrderIdAsync(TicketRequest.OrderId);
				return Page();
			}

			await _ticketService.CreateTicketAsync(TicketRequest);
			await _hubContext.Clients.All.SendAsync("ReceiveOrderStatusUpdate");
			TempData["Success"] = "Support request submitted successfully!";
			return RedirectToPage("/User/Rental-Order/Detail", new { TicketRequest.OrderId });
		}

	}
}
