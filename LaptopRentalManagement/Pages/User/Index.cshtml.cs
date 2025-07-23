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

		public List<OrderDto> Orders { get; set; } = new();
		public IList<OrderResponse> RentalOrder { get; set; } = new List<OrderResponse>();
		public IList<LaptopResponse> MyLaptops { get; set; } = new List<LaptopResponse>();

		public async Task OnGet()
        {
            // Mock đơn mua
            Orders = new List<OrderDto>
        {
            new OrderDto { OrderId = 101, CreatedAt = DateTime.Now.AddDays(-3), Status = "Đang xử lý" },
            new OrderDto { OrderId = 102, CreatedAt = DateTime.Now.AddDays(-1), Status = "Đã giao" }
        };

            OrderFilter orderFilter = new();
            LaptopFilter laptopFilter = new()
            {
                AccountId = 1 //Nhớ đổi lại
            };

            RentalOrder = await _orderService.GetAllAsync(orderFilter);
            MyLaptops = await _laptopService.GetAllAsync(laptopFilter);
        }

		public class OrderDto
        {
            public int OrderId { get; set; }
            public DateTime CreatedAt { get; set; }
            public string Status { get; set; }
        }
    }
}
