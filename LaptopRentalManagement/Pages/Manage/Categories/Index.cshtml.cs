using LaptopRentalManagement.BLL.DTOs.Request;
using LaptopRentalManagement.BLL.DTOs.Response;
using LaptopRentalManagement.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LaptopRentalManagement.Pages.Manage.Categories;

public class IndexModel : PageModel
{
    private readonly ICategoryService _categoryService;

    public IndexModel(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public IEnumerable<CategoryResponse> Categories { get; set; } = new List<CategoryResponse>();

    public async Task OnGetAsync()
    {
        try
        {
            Categories = await _categoryService.GetAllCategoriesAsync();
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Error loading categories: " + ex.Message;
            Categories = new List<CategoryResponse>();
        }
    }

    public async Task<IActionResult> OnGetGetCategoryAsync(int id)
    {
        try
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null) return NotFound();
            return new JsonResult(category);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    public async Task<IActionResult> OnPostCreateAsync(string name, string description)
    {
        if (!ModelState.IsValid)
        {
            // Return validation errors
            var errors = ModelState.Where(x => x.Value.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );
            return new JsonResult(new { success = false, errors });
        }

        try
        {
            var request = new CreateCategoryRequest { Name = name, Description = description };
            var result = await _categoryService.CreateCategoryAsync(request);
            TempData["Success"] = $"Category '{result.Name}' created successfully!";
            return new JsonResult(new { success = true });
        }
        catch (InvalidOperationException ex)
        {
            return new JsonResult(new { success = false, errors = new { Name = new[] { ex.Message } } });
        }
        catch (Exception ex)
        {
            return new JsonResult(new
                { success = false, errors = new { General = new[] { "Error creating category: " + ex.Message } } });
        }
    }

    public async Task<IActionResult> OnPostUpdateAsync(int categoryId, string name, string description)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Where(x => x.Value.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );
            return new JsonResult(new { success = false, errors });
        }

        try
        {
            var request = new UpdateCategoryRequest { CategoryId = categoryId, Name = name, Description = description };
            var result = await _categoryService.UpdateCategoryAsync(request);
            TempData["Success"] = $"Category '{result.Name}' updated successfully!";
            return new JsonResult(new { success = true });
        }
        catch (ArgumentException ex)
        {
            return new JsonResult(new { success = false, errors = new { General = new[] { ex.Message } } });
        }
        catch (InvalidOperationException ex)
        {
            return new JsonResult(new { success = false, errors = new { Name = new[] { ex.Message } } });
        }
        catch (Exception ex)
        {
            return new JsonResult(new
                { success = false, errors = new { General = new[] { "Error updating category: " + ex.Message } } });
        }
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        try
        {
            var success = await _categoryService.DeleteCategoryAsync(id);
            if (success)
                TempData["Success"] = "Category deleted successfully!";
            else
                TempData["Error"] = "Cannot delete category!";
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Error deleting category: " + ex.Message;
        }

        return RedirectToPage();
    }
}