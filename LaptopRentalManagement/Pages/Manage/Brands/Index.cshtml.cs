using LaptopRentalManagement.BLL.DTOs.Response;
using LaptopRentalManagement.BLL.Interfaces;
using LaptopRentalManagement.Model.DTOs.Request;
using LaptopRentalManagement.Model.DTOs.Response.Brand;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LaptopRentalManagement.Pages.Manage.Brands
{
    public class IndexModel : PageModel
    {
        private readonly IBrandService _brandService;

        public IndexModel(IBrandService brandService)
        {
            _brandService = brandService;
        }

        // DTOs để nhận dữ liệu từ các form trong modal
        [BindProperty]
        public CreateBrandRequest BrandCreate { get; set; }

        [BindProperty]
        public UpdateBrandRequest BrandUpdate { get; set; }

        public IList<BrandResponse> Brands { get; set; } = new List<BrandResponse>();

        // Lấy danh sách ban đầu khi tải trang
        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var brands = await _brandService.GetAllBrandsAsync();
                Brands = brands.ToList();
            }
            catch (Exception ex)
            {
                // Ghi log lỗi (quan trọng)
                TempData["Error"] = "Failed to load brands. Please try again.";
            }
            return Page();
        }

        // === CÁC PHƯƠNG THỨC XỬ LÝ CRUD ĐƯỢC THÊM VÀO ===

        // Handler để lấy dữ liệu cho modal "View"
        public async Task<IActionResult> OnGetGetBrandAsync(int id)
        {
            try
            {
                var brand = await _brandService.GetBrandByIdAsync(id);
                if (brand == null)
                {
                    return NotFound();
                }
                return new JsonResult(brand);
            }
            catch (Exception ex)
            {
                // Ghi log lỗi
                return StatusCode(500, "An error occurred while fetching brand details.");
            }
        }

        // Handler để xử lý form "Create"
        public async Task<IActionResult> OnPostCreateAsync()
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );
                return new JsonResult(new { success = false, errors });
            }

            try
            {
                await _brandService.CreateBrandAsync(BrandCreate);
                TempData["Success"] = "Brand created successfully!";
                return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
                // Ghi log lỗi
                return new JsonResult(new { success = false, message = "An error occurred while creating the brand." });
            }
        }

        // Handler để xử lý form "Update"
        public async Task<IActionResult> OnPostUpdateAsync()
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );
                return new JsonResult(new { success = false, errors });
            }

            try
            {
                await _brandService.UpdateBrandAsync(BrandUpdate);
                TempData["Success"] = "Brand updated successfully!";
                return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
                // Ghi log lỗi
                return new JsonResult(new { success = false, message = "An error occurred while updating the brand." });
            }
        }

        // Handler để xử lý form "Delete"
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            try
            {
                await _brandService.DeleteBrandAsync(id);
                TempData["Success"] = "Brand deleted successfully.";
            }
            catch (InvalidOperationException ex) // Bắt lỗi nghiệp vụ cụ thể
            {
                TempData["Error"] = ex.Message; // Ví dụ: "Không thể xóa thương hiệu đã có laptop."
            }
            catch (Exception ex)
            {
                // Ghi log lỗi
                TempData["Error"] = "An error occurred while deleting the brand.";
            }

            // Form delete dùng postback truyền thống nên cần Redirect
            return RedirectToPage();
        }
    }
}