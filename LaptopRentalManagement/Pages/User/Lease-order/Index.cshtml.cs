using LaptopRentalManagement.BLL.DTOs.Request;
using LaptopRentalManagement.BLL.DTOs.Response;
using LaptopRentalManagement.BLL.Interfaces;
using LaptopRentalManagement.Model.DTOs.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace LaptopRentalManagement.Pages.User.Lease_order
{
    public class IndexModel : PageModel
    {

		private readonly IOrderService _orderService;

		public IndexModel(
		IOrderService orderService)
		{
			_orderService = orderService;
		}
        public IList<OrderResponse> MyLeaseOrder { get; set; } = new List<OrderResponse>();

        public decimal TotalRevenue { get; set; }
        public async Task<IActionResult> OnGet()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("AccountId");
            if (!int.TryParse(userIdClaim, out var userId))
            {
                TempData["Error"] = "Please login to continue";
                return RedirectToPage("/Account/Login");
            }
            OrderFilter orderFilter = new()
            {
                OwnerId = int.Parse(userIdClaim)
            };

            MyLeaseOrder = await _orderService.GetAllAsync(orderFilter);

            TotalRevenue = await _orderService.GetCompletedRevenueAsync(userId);

            return Page();
        }
    }
}
