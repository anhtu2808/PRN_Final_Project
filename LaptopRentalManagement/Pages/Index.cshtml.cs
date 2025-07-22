using LaptopRentalManagement.BLL.DTOs.Response;
using LaptopRentalManagement.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LaptopRentalManagement.Pages;

public class IndexModel : PageModel
{
    private readonly ILaptopService _laptopService;
    private readonly ICategoryService _categoryService;

    public IndexModel(ILaptopService laptopService, ICategoryService categoryService)
    {
        _categoryService = categoryService;
        _laptopService = laptopService;
    }

    public IList<LaptopResponse> FeaturedLaptops { get; private set; } = new List<LaptopResponse>();
    public IEnumerable<CategoryResponse> Categories { get; set; } = new List<CategoryResponse>();

    public async Task OnGetAsync()
    {
        // Load top 3 most rented laptops
        FeaturedLaptops = await _laptopService.GetTopRentedLaptopsAsync(3);
        // Load all categories for the sidebar
        var allCategories = await _categoryService.GetAllCategoriesAsync();
        Categories = allCategories.OrderBy(x => Guid.NewGuid())
            .Take(4)
            .ToList();
    }
}