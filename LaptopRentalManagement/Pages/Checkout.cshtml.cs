using System.Security.Claims;
using LaptopRentalManagement.BLL.DTOs.Request;
using LaptopRentalManagement.BLL.DTOs.Response;
using LaptopRentalManagement.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LaptopRentalManagement.Pages
{
    // [Authorize]
    public class Checkout : PageModel
    {
        private readonly ILaptopService _laptopService;
        private readonly IOrderService _orderService;
        private readonly ISlotService _slotService;

        public Checkout(ILaptopService laptopService, IOrderService orderService, ISlotService slotService)
        {
            _laptopService = laptopService;
            _orderService = orderService;
            _slotService = slotService;
        }

        public LaptopResponse Laptop { get; set; }
        public List<SlotResponse> Slots { get; set; }

        public decimal TotalCharge { get; set; }

        [BindProperty] public List<int> SelectedSlots { get; set; }

        [BindProperty] public int LaptopId { get; set; }


        public async Task<IActionResult> OnGetAsync(int id, List<int> selectedSlots)
        {
            if (selectedSlots == null || !selectedSlots.Any())
            {
                TempData["Error"] = "Vui lòng chọn ít nhất một ngày thuê.";
                return RedirectToPage("/Laptops/Details", new { id = id });
            }

            Slots = await _slotService.GetByIdsAsync(selectedSlots);

            Laptop = await _laptopService.GetByIdAsync(id);
            if (Laptop == null)
            {
                return NotFound("Không tìm thấy laptop.");
            }

            LaptopId = Laptop.LaptopId;
            SelectedSlots = selectedSlots;
            TotalCharge = SelectedSlots.Count * Laptop.PricePerDay;

            return Page();
        }

        // Handler này nhận thông tin từ chính trang Checkout để tạo đơn hàng (không đổi)
        public async Task<IActionResult> OnPostConfirmOrderAsync()
        {
            if (!ModelState.IsValid || !SelectedSlots.Any())
            {
                // Tải lại dữ liệu nếu có lỗi để hiển thị lại trang
                if (LaptopId > 0)
                {
                    Laptop = await _laptopService.GetByIdAsync(LaptopId);
                    TotalCharge = SelectedSlots.Count * Laptop.PricePerDay;
                }

                return Page();
            }

            // var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var userId = 2;
            var laptop = await _laptopService.GetByIdAsync(LaptopId);
            if (laptop == null)
            {
                return NotFound("Không tìm thấy laptop.");
            }

            var totalCharge = SelectedSlots.Count * laptop.PricePerDay;

            await _orderService.CreateAsync(new CreateOrderRequest
            {
                LaptopId = this.LaptopId,
                RenterId = userId,
                TotalCharge = totalCharge,
                SlotIds = this.SelectedSlots
            });

            TempData["Success"] = "Bạn đã đặt hàng thành công!";
            return RedirectToPage("/User/Rental-order/Index");
        }
    }
}