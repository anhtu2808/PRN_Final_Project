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
        private readonly IOrderService _orderService;
        private readonly ILaptopService _laptopService;

        public IndexModel(IOrderService orderService, ILaptopService laptopService)
        {
			_orderService = orderService;
            _laptopService = laptopService;

		}

		public List<OrderDto> Orders { get; set; } = new();
		public IList<OrderResponse> RentalOrder { get; set; } = new List<OrderResponse>();
		public IList<LaptopResponse> MyLaptops { get; set; } = new List<LaptopResponse>();

		[BindProperty]
		public CreateOrderRequest NewOrder { get; set; }

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

        public async Task<IActionResult> OnPostRentOutAsync()
        {
            // var currentUserId = int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId) ? userId : 0;
            var currentUserId = 1;
			NewOrder.OwnerId = currentUserId;
			try
			{
				await _orderService.CreateAsync(NewOrder);
				TempData["Success"] = "Đăng cho thuê thành công!";
			}
			catch (Exception ex)
			{
				// Log lỗi nếu cần
				TempData["Error"] = "Có lỗi xảy ra khi tạo đơn hàng: " + ex.Message;
			}

			return RedirectToPage();
		}


		public class OrderDto
        {
            public int OrderId { get; set; }
            public DateTime CreatedAt { get; set; }
            public string Status { get; set; }
        }
    }
}
