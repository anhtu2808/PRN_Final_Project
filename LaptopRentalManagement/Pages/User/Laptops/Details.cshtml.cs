using LaptopRentalManagement.BLL.DTOs.Request;
using LaptopRentalManagement.BLL.DTOs.Response;
using LaptopRentalManagement.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LaptopRentalManagement.Pages.User.Laptops;

public class DetailsModel : PageModel
{
    private readonly ILaptopService _laptopService;

    // private readonly IBrandService _brandService;
    private readonly ICategoryService _categoryService;

    public DetailsModel(
        ILaptopService laptopService,
        ICategoryService categoryService)
    {
        _laptopService = laptopService;
        _categoryService = categoryService;
    }

    public LaptopResponse? Laptop { get; private set; }
    public List<BrandResponse> Brands { get; set; } = new();
    public IEnumerable<CategoryResponse> Categories { get; set; } = new List<CategoryResponse>();


    public async Task<IActionResult> OnGetAsync(int id)
    {
        Laptop = await _laptopService.GetByIdAsync(id);
        Categories = await _categoryService.GetAllCategoriesAsync();

        if (Laptop == null)
        {
            return NotFound();
        }

        return Page();
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
            ImageURL = imageUrl,
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