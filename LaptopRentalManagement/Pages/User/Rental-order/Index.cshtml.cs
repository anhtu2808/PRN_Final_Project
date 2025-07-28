using LaptopRentalManagement.BLL.DTOs.Request;
using LaptopRentalManagement.BLL.DTOs.Response;
using LaptopRentalManagement.BLL.Interfaces;
using LaptopRentalManagement.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LaptopRentalManagement.Pages.User.Rental_order
{
	public class IndexModel : PageModel
	{
		private readonly IOrderService _orderService;
		private readonly IOrderLogService _orderLogService;
		private readonly ITicketService _ticketService;

		public IndexModel(
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

		[BindProperty]
		public CreateTicketRequest TicketRequest { get; set; }

		public async Task<IActionResult> OnGetAsync(int orderId)
		{
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
			TempData["Success"] = $"Đơn #{orderId} đã được xác nhận là đã trả.";
			return RedirectToPage("/User/Rental-Order/Index", new { orderId });
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
			TempData["Success"] = "Yêu cầu hỗ trợ đã được gửi thành công!";
			return RedirectToPage("/User/Rental-Order/Index", new { TicketRequest.OrderId });
		}

	}
}
