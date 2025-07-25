using LaptopRentalManagement.BLL.DTOs.Response;
using LaptopRentalManagement.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LaptopRentalManagement.Pages.User.Rental_order
{
    public class IndexModel : PageModel
    {
        private readonly IOrderService _orderService;

        public IndexModel(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public OrderResponse? Order { get; set; }

        public async Task<IActionResult> OnGetAsync(int orderId)
        {
            Order = await _orderService.GetByIdAsync(orderId);
            if (Order == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
