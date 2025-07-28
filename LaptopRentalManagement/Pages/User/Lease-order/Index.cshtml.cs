using LaptopRentalManagement.BLL.DTOs.Request;
using LaptopRentalManagement.BLL.DTOs.Response;
using LaptopRentalManagement.BLL.Interfaces;
using LaptopRentalManagement.BLL.Services;
using LaptopRentalManagement.DAL.Entities;
using LaptopRentalManagement.Model.DTOs.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
        public async Task OnGet()
        {
            OrderFilter orderFilter = new()
            {
                OwnerId = 1
            };

            MyLeaseOrder = await _orderService.GetAllAsync(orderFilter);
        }
    }
}
