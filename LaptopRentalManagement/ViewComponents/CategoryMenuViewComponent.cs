using LaptopRentalManagement.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace LaptopRentalManagement.ViewComponents
{
    public class CategoryMenuViewComponent : ViewComponent
    {
        private readonly ICategoryService _categoryService;

        public CategoryMenuViewComponent(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return View(categories); // sẽ tìm tới Default.cshtml
        }
    }
}