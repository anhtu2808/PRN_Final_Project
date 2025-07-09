using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LaptopRentalManagement.Pages.Categories
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
            // In a real application, you would load categories from database here
            // var categories = _categoryService.GetAllCategories();
        }
    }
} 