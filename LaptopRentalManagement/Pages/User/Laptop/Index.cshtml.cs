using LaptopRentalManagement.BLL.DTOs.Request;
using LaptopRentalManagement.BLL.DTOs.Response;
using LaptopRentalManagement.BLL.Interfaces;
using LaptopRentalManagement.BLL.Services;
using LaptopRentalManagement.DAL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LaptopRentalManagement.Pages.User.Rental_orders
{
    public class IndexModel : PageModel
    {
        private readonly IOrderService _orderService;
        private readonly ILaptopService _laptopService;
        private readonly ISlotService _slotService;

        public IndexModel(IOrderService orderService, ILaptopService laptopService, ISlotService slotService)
        {
            _orderService = orderService;
            _laptopService = laptopService;
            _slotService = slotService;
        }

        public LaptopResponse? Laptop { get; set; }
        public IList<OrderResponse> Orders { get; set; } = new List<OrderResponse>();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Laptop = await _laptopService.GetByIdAsync(id);
            if (Laptop == null)
            {
                return NotFound();
            }
            Orders = await _orderService.GetAllAsync(new() { LaptopId = id });
            return Page();
        }
        [BindProperty]
        public CreateSlotRequest NewSlot { get; set; }

        public async Task<IActionResult> OnPostRentOutAsync()
        {
            try
            {
                await _slotService.CreateAsync(NewSlot);
                TempData["Success"] = "Tạo slot thành công!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi xảy ra khi tạo slot: " + ex.Message;
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostApproveAsync(int orderId, int id)
        {
            await _orderService.ApproveAsync(orderId);
            TempData["Success"] = $"Đã duyệt đơn #{orderId}.";
            return RedirectToPage("/User/Laptop/Index", new { id });
        }

        public async Task<IActionResult> OnPostRejectAsync(int orderId, int id)
        {
            await _orderService.RejectAsync(orderId);
            TempData["Success"] = $"Đã từ chối đơn #{orderId}.";
            return RedirectToPage("/User/Laptop/Index", new { id });
        }

		public async Task<IActionResult> OnPostConfirmReturnAsync(int orderId, int id)
		{
			await _orderService.ConfirmReturn(orderId);
			TempData["Success"] = $"Đơn #{orderId} đã được xác nhận là đã trả.";
			return RedirectToPage("/User/Laptop/Index", new { id });
		}

	}
}
