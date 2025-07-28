using LaptopRentalManagement.BLL.DTOs.Request;
using LaptopRentalManagement.BLL.DTOs.Response;
using LaptopRentalManagement.BLL.Interfaces;
using LaptopRentalManagement.DAL.Entities;
using LaptopRentalManagement.Model.DTOs.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LaptopRentalManagement.Pages.User.Rental_order
{
	public class IndexModel : PageModel
	{
		private readonly IOrderService _orderService;

		public IndexModel(
		IOrderService orderService)
		{
			_orderService = orderService;
		}
		public IList<OrderResponse> MyRentalOrder { get; set; } = new List<OrderResponse>();
		public async Task OnGet()
		{
			OrderFilter orderFilter = new()
			{
				RenterId = 2
			};

			MyRentalOrder = await _orderService.GetAllAsync(orderFilter);
		}

	}
}
