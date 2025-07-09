using Microsoft.AspNetCore.Mvc.RazorPages;
using LaptopRentalManagement.BLL.Interfaces;
using LaptopRentalManagement.BLL.DTOs.Response;

namespace LaptopRentalManagement.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly ICategoryService _categoryService;

        public IndexModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public IEnumerable<CategoryResponseDto> Categories { get; set; } = new List<CategoryResponseDto>();

        public async Task OnGetAsync()
        {
            try
            {
                Categories = await _categoryService.GetAllCategoriesAsync();
            }
            catch (Exception ex)
            {
                // Log error and show empty list
                Categories = new List<CategoryResponseDto>();
                // In production, you would log this error
                Console.WriteLine($"Error loading categories: {ex.Message}");
            }
        }
    }
} 