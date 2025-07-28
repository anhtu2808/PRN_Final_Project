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

        public IList<SlotResponse> Slots { get; set; } = new List<SlotResponse>();
        public RentalSlotResponse RentalSlot { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Laptop = await _laptopService.GetByIdAsync(id);
            if (Laptop == null)
            {
                return NotFound();
            }
            Orders = await _orderService.GetAllAsync(new() { LaptopId = id });

            RentalSlot = new RentalSlotResponse
            {
                Slots = await _slotService.GetAllAsync(new() { LaptopId = id, Month = DateTime.UtcNow.Month, Year = DateTime.UtcNow.Year }),
                DaysInMonth = Enumerable.Range(1, DateTime.DaysInMonth(DateTime.UtcNow.Year, DateTime.UtcNow.Month))
                                .Select(d => new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, d)).ToList()
            };

            return Page();
        }

        public async Task<PartialViewResult> OnGetRentalSlotsAsync(int id, int month, int year)
        {
            IList<SlotResponse> slots = await _slotService.GetAllAsync(new() { LaptopId = id, Month = month, Year = year });
            var daysInMonth = Enumerable.Range(1, DateTime.DaysInMonth(year, month))
                .Select(d => new DateTime(year, month, d))
                .ToList();

            var rentalSlot = new RentalSlotResponse
            {
                Slots = slots,
                DaysInMonth = daysInMonth
            };

            return Partial("_RentalSlotsPartial", rentalSlot);
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

        public async Task<IActionResult> OnPostDeliveringAsync(int orderId, int id)
        {
            await _orderService.SetStatusAsync(orderId, "Delivering");
            TempData["Success"] = $"Đơn #{orderId} đang giao hàng.";
            return RedirectToPage("/User/Laptop/Index", new { id });
        }

        public async Task<IActionResult> OnPostDeliveredAsync(int orderId, int id)
        {
            await _orderService.SetStatusAsync(orderId, "Renting");
            TempData["Success"] = $"Đơn #{orderId} đã giao thành công.";
            return RedirectToPage("/User/Laptop/Index", new { id });
        }

        public async Task<IActionResult> OnPostDeliveredFailAsync(int orderId, int id)
        {
            await _orderService.SetStatusAsync(orderId, "DeliveringFail");
            TempData["Warning"] = $"Giao hàng đơn #{orderId} thất bại. Trạng thái quay về 'Delivering'.";
            return RedirectToPage("/User/Laptop/Index", new { id });
        }

    }
}
