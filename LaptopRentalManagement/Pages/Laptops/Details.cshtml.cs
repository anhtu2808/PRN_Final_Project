using LaptopRentalManagement.BLL.DTOs.Request;
using LaptopRentalManagement.BLL.DTOs.Response;
using LaptopRentalManagement.BLL.Interfaces;
using LaptopRentalManagement.BLL.Services;
using LaptopRentalManagement.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LaptopRentalManagement.Pages.Laptops
{
    public class DetailsModel : PageModel
    {
      
        private readonly ILaptopService _laptopService;
        private readonly IOrderService _orderService;

        public DetailsModel(ILaptopService laptopService, IOrderService orderService)
        {
            _laptopService = laptopService;
            _orderService = orderService;
        }

        public IList<SlotResponse> Slots { get; set; } = new List<SlotResponse>();

        [BindProperty]
        public List<int> SelectedSlots { get; set; }
        public LaptopResponse? Laptop { get; set; } = new LaptopResponse();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Laptop = await _laptopService.GetByIdAsync(id);
            Slots = Laptop.Slots;
            if (Laptop == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            Laptop = await _laptopService.GetByIdAsync(id);
            if (Laptop == null)
            {
                return NotFound();
            }

            if (SelectedSlots == null || !SelectedSlots.Any())
            {
                ModelState.AddModelError(string.Empty, "Bạn phải chọn ít nhất 1 ngày thuê.");
                return Page();
            }

            var totalDays = SelectedSlots.Count;
            var pricePerDay = Laptop.PricePerDay;
            decimal totalCharge = totalDays * pricePerDay;

            await _orderService.CreateAsync(new()
            {
                LaptopId = Laptop.LaptopId,
                RenterId = 2, // Nhớ đổi lại
                TotalCharge = totalCharge,
                SlotIds = SelectedSlots
            });


            return RedirectToPage("/Laptops/Index");
        }
    }
}