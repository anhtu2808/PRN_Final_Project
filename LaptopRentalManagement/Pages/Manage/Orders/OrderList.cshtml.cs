using LaptopRentalManagement.BLL.DTOs.Response;
using LaptopRentalManagement.BLL.Interfaces;
using LaptopRentalManagement.Model.DTOs.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LaptopRentalManagement.Pages.Manage.Orders
{
    [Authorize(Policy = "AdminOnly")]
    public class OrderListModel : PageModel
    {
        private readonly IOrderService _orderService;

        public OrderListModel(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public IList<OrderResponse> Orders { get; set; } = new List<OrderResponse>();

        [BindProperty(SupportsGet = true)]
        public string? Status { get; set; }

        public async Task OnGetAsync()
        {
            var filter = new OrderFilter();

            Orders = await _orderService.GetAllAsync(filter);
        }
    }
}
