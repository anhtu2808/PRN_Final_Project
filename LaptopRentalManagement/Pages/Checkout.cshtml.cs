using System.Security.Claims;
using LaptopRentalManagement.BLL.DTOs.Request;
using LaptopRentalManagement.BLL.DTOs.Response;
using LaptopRentalManagement.BLL.Interfaces;
using LaptopRentalManagement.DAL.Interfaces;
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
        private readonly IZaloPayService _zaloPayService;
        private readonly IOrderRepository _orderRepository;

        public Checkout(ILaptopService laptopService, IOrderService orderService, ISlotService slotService,
            IZaloPayService zaloPayService, IOrderRepository orderRepository)
        {
            _laptopService = laptopService;
            _orderService = orderService;
            _slotService = slotService;
            _zaloPayService = zaloPayService;
            _orderRepository = orderRepository;
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
                TempData["Error"] = "Please select at least one rental day.";
                return RedirectToPage("/Laptops/Details", new { id = id });
            }

            Slots = await _slotService.GetByIdsAsync(selectedSlots);

            Laptop = await _laptopService.GetByIdAsync(id);
            if (Laptop == null)
            {
                return NotFound("Laptop not found.");
            }

            LaptopId = Laptop.LaptopId;
            SelectedSlots = selectedSlots;
            // Cập nhật công thức tính tổng tiền, cộng thêm tiền cọc
            TotalCharge = (SelectedSlots.Count * Laptop.PricePerDay) + Laptop.Deposit;

            return Page();
        }

        // Handler for ZaloPay checkout
        public async Task<IActionResult> OnPostConfirmOrderAsync()
        {
            if (!ModelState.IsValid || !SelectedSlots.Any())
            {
                if (LaptopId > 0)
                {
                    Laptop = await _laptopService.GetByIdAsync(LaptopId);
                    // Cập nhật công thức tính tổng tiền nếu có lỗi
                    if (Laptop != null)
                    {
                        TotalCharge = (SelectedSlots.Count * Laptop.PricePerDay) + Laptop.Deposit;
                    }
                }
                return Page();
            }

            try
            {
                // Get current user ID
                var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("AccountId");
                if (!int.TryParse(userIdClaim, out var userId))
                {
                    TempData["Error"] = "Please login to continue";
                    return RedirectToPage("/Account/Login");
                }

                var laptop = await _laptopService.GetByIdAsync(LaptopId);
                if (laptop == null)
                {
                    return NotFound("Laptop not found.");
                }

                // Cập nhật công thức tính tổng tiền, cộng thêm tiền cọc
                var totalCharge = (SelectedSlots.Count * laptop.PricePerDay) + laptop.Deposit;

                // Step 1: Create order with Unpaid status
                var orderResponse = await _orderService.CreateOrderForPaymentAsync(new CreateOrderRequest
                {
                    LaptopId = LaptopId,
                    RenterId = userId,
                    TotalCharge = totalCharge, // Sử dụng tổng tiền mới
                    SlotIds = this.SelectedSlots
                });

                // Step 2: Generate ZaloPay transaction ID
                var appTransId = GenerateAppTransId(orderResponse.OrderId);
                await _orderRepository.UpdateZaloPayTransactionIdAsync(orderResponse.OrderId, appTransId);

                // Step 3: Get the Order entity to pass to ZaloPay
                var orderEntity = await _orderService.GetOrderEntityByIdAsync(orderResponse.OrderId);
                if (orderEntity == null)
                {
                    TempData["Error"] = "Failed to create order";
                    return Page();
                }

                // Step 4: Create ZaloPay payment URL
                var redirectUrl = $"{Request.Scheme}://{Request.Host}/PaymentCallback";
                var zaloPayResponse = await _zaloPayService.GetPymentUrl((long)totalCharge, orderEntity, redirectUrl);

                if (zaloPayResponse.ReturnCode != 1)
                {
                    TempData["Error"] = $"Payment initialization failed: {zaloPayResponse.ReturnMessage}";
                    return Page();
                }

                // Step 5: Redirect to ZaloPay
                return Redirect(zaloPayResponse.OrderUrl);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"An error occurred: {ex.Message}";
                return Page();
            }
        }

        private string GenerateAppTransId(int orderId)
        {
            var timestamp = DateTime.Now.ToString("yyMMdd");
            return $"{timestamp}_{orderId}_{DateTime.Now.Ticks}";
        }
    }
}