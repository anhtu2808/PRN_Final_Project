using LaptopRentalManagement.BLL.DTOs.Request;
using LaptopRentalManagement.BLL.DTOs.Response;
using LaptopRentalManagement.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LaptopRentalManagement.Pages.Manage.Categories;

public class IndexModel : PageModel
{
    [Authorize(Policy = "AdminOnly")]
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
            TempData["Error"] = "Lỗi khi tải danh sách danh mục: " + ex.Message;
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
            TempData["Success"] = $"Tạo danh mục '{result.Name}' thành công!";
            return new JsonResult(new { success = true });
        }
        catch (InvalidOperationException ex)
        {
            return new JsonResult(new { success = false, errors = new { Name = new[] { ex.Message } } });
        }
        catch (Exception ex)
        {
            return new JsonResult(new
                { success = false, errors = new { General = new[] { "Lỗi khi tạo danh mục: " + ex.Message } } });
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
            TempData["Success"] = $"Cập nhật danh mục '{result.Name}' thành công!";
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
                { success = false, errors = new { General = new[] { "Lỗi khi cập nhật danh mục: " + ex.Message } } });
        }
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        try
        {
            var success = await _categoryService.DeleteCategoryAsync(id);
            if (success)
                TempData["Success"] = "Xóa danh mục thành công!";
            else
                TempData["Error"] = "Không thể xóa danh mục!";
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Lỗi khi xóa danh mục: " + ex.Message;
        }

        return RedirectToPage();
    }
}