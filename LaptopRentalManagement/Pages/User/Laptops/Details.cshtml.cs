using LaptopRentalManagement.BLL.DTOs.Request;
using LaptopRentalManagement.BLL.DTOs.Response;
using LaptopRentalManagement.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace LaptopRentalManagement.Pages.User.Laptops;

public class DetailsModel : PageModel
{
    private readonly IOrderService _orderService;
    private readonly ILaptopService _laptopService;
    private readonly ISlotService _slotService;
    private readonly ICategoryService _categoryService;
    private readonly IBrandService _brandService; // Thêm

    public DetailsModel(
        IOrderService orderService,
        ILaptopService laptopService,
        ISlotService slotService,
        ICategoryService categoryService,
        IBrandService brandService) // Thêm
    {
        _orderService = orderService;
        _laptopService = laptopService;
        _slotService = slotService;
        _categoryService = categoryService;
        _brandService = brandService; // Thêm
    }
    
    // Properties để hiển thị dữ liệu
    public LaptopResponse Laptop { get; set; }
    public IList<OrderResponse> Orders { get; set; } = new List<OrderResponse>();
    public RentalSlotResponse RentalSlot { get; set; } = new();

    // Properties để điền vào form
    public IEnumerable<BrandResponse> Brands { get; set; } = new List<BrandResponse>();
    public IEnumerable<CategoryResponse> Categories { get; set; } = new List<CategoryResponse>();

    [BindProperty]
    public EditLaptopRequest EditLaptop { get; set; }

    [BindProperty]
    public CreateSlotRequest NewSlot { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("AccountId");
        if (!int.TryParse(userIdClaim, out var userId))
        {
            TempData["Error"] = "Please login to continue";
            return RedirectToPage("/Account/Login");
        }
        
        Laptop = await _laptopService.GetByIdAsync(id);
        if (Laptop == null || Laptop.Owner.AccountId != userId) // Kiểm tra quyền sở hữu
        {
            return NotFound();
        }

        // Khởi tạo form Edit với dữ liệu hiện tại
        EditLaptop = new EditLaptopRequest
        {
            LaptopId = Laptop.LaptopId,
            Name = Laptop.Name,
            Description = Laptop.Description,
            BrandId = Laptop.Brand.BrandId,
            Cpu = Laptop.Cpu,
            Ram = Laptop.Ram,
            Storage = Laptop.Storage,
            PricePerDay = Laptop.PricePerDay,
            Deposit = Laptop.Deposit,
            CategoryIds = Laptop.Categories.Select(c => c.CategoryId).ToList()
        };

        // Lấy dữ liệu cho các dropdown/checkbox
        Orders = await _orderService.GetAllAsync(new() { LaptopId = id });
        Categories = await _categoryService.GetAllCategoriesAsync();
        Brands = await _brandService.GetAllBrandsAsync();
        
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
        // ... (logic không đổi)
        IList<SlotResponse> slots = await _slotService.GetAllAsync(new() { LaptopId = id, Month = month, Year = year });
        var daysInMonth = Enumerable.Range(1, DateTime.DaysInMonth(year, month)).Select(d => new DateTime(year, month, d)).ToList();
        var rentalSlot = new RentalSlotResponse { Slots = slots, DaysInMonth = daysInMonth };
        return Partial("_RentalSlotsPartial", rentalSlot);
    }
    
    public async Task<IActionResult> OnPostRentOutAsync()
    {
        // ... (logic không đổi)
        try
        {
            await _slotService.CreateAsync(NewSlot);
            TempData["Success"] = "Slot created successfully!";
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Error while creating slot: " + ex.Message;
        }
        return RedirectToPage(new { id = NewSlot.LaptopId });
    }

    public async Task<IActionResult> OnPostEditAsync()
    {
        if (!ModelState.IsValid)
        {
            // Nếu model không hợp lệ, tải lại dữ liệu cần thiết và hiển thị lại trang với lỗi
            await OnGetAsync(EditLaptop.LaptopId);
            return Page();
        }

        await _laptopService.UpdateAsync(EditLaptop);
        
        TempData["Success"] = "Laptop updated successfully!";
        return RedirectToPage(new { id = EditLaptop.LaptopId });
    }
}