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
		private readonly ICategoryService _categoryService;

		public IndexModel(IOrderService orderService, ILaptopService laptopService, ISlotService slotService, ICategoryService categoryService)
        {
            _orderService = orderService;
            _laptopService = laptopService;
            _slotService = slotService;
            _categoryService = categoryService;
        }

		public List<BrandResponse> Brands { get; set; } = new();
		public IEnumerable<CategoryResponse> Categories { get; set; } = new List<CategoryResponse>();

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

		public async Task<IActionResult> OnPostEditAsync()
		{
			var form = Request.Form;

			// Lấy CategoryIds an toàn (sẽ là mảng nhiều giá trị nếu select multiple)
			var rawCategories = form["CategoryIds"];
			List<int> categoryIds = new();
			foreach (var cat in rawCategories)
			{
				if (int.TryParse(cat, out var id))
					categoryIds.Add(id);
			}

			// Parse các field còn lại
			int laptopId = int.Parse(form["LaptopId"]);
			string name = form["Name"];
			string imageUrl = form["ImageURL"];

			string cpu = form["Cpu"];
			int.TryParse(form["Ram"], out int ram);
			int.TryParse(form["Storage"], out int storage);
			decimal.TryParse(form["PricePerDay"], out decimal pricePerDay);
			string description = form["Description"];

			// Gọi service update
			var updateRequest = new EditLaptopRequest
			{
				LaptopId = laptopId,
				Name = name,
				Cpu = cpu,
				Ram = ram,
				Storage = storage,
				PricePerDay = pricePerDay,
				Description = description,
				CategoryIds = categoryIds
			};

			await _laptopService.UpdateAsync(updateRequest);
			return RedirectToPage(); // hoặc RedirectToPage("Index");
		}

	}
}
