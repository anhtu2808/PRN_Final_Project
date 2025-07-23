using LaptopRentalManagement.BLL.DTOs.Request;
using LaptopRentalManagement.BLL.DTOs.Response;
using LaptopRentalManagement.BLL.Interfaces;
using LaptopRentalManagement.Model.DTOs.Request;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace LaptopRentalManagement.Pages.User
{
	public class IndexModel : PageModel
	{
		private readonly ISlotService _slotService;
		private readonly IOrderService _orderService;
		private readonly ILaptopService _laptopService;

		public IndexModel(ISlotService slotService, ILaptopService laptopService, IOrderService orderService)
		{
			_slotService = slotService;
			_laptopService = laptopService;
			_orderService = orderService;
		}
		public IList<LaptopResponse> MyLaptops { get; set; } = new List<LaptopResponse>();
		public IList<OrderResponse> MyRentalOrder { get; set; } = new List<OrderResponse>();

		public async Task OnGet()
		{
			OrderFilter orderFilter = new()
			{
				RenterId = 2
			};
			LaptopFilter laptopFilter = new()
			{
				AccountId = 1 //Nhớ đổi lại
			};

            MyRentalOrder = await _orderService.GetAllAsync(orderFilter);
			MyLaptops = await _laptopService.GetAllAsync(laptopFilter);
        }
	}
}
