using LaptopRentalManagement.BLL.DTOs.Response;
using LaptopRentalManagement.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LaptopRentalManagement.Pages.User.Rental_order
{
    public class IndexModel : PageModel
    {
        private readonly IOrderService _orderService;
        private readonly IOrderLogService _orderLogService;

        public IndexModel(IOrderService orderService, IOrderLogService orderLogService)
        {
            _orderService = orderService;
            _orderLogService = orderLogService;
        }

        public OrderResponse? Order { get; set; }
        public IList<OrderLogResponse> Logs = new List<OrderLogResponse>();

        public async Task<IActionResult> OnGetAsync(int orderId)
        {
            Order = await _orderService.GetByIdAsync(orderId);
            Logs = await _orderLogService.GetByOrderIdAsync(orderId);
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

    }
}
