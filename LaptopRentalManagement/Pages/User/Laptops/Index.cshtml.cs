using LaptopRentalManagement.BLL.DTOs.Request;
using LaptopRentalManagement.BLL.DTOs.Response;
using LaptopRentalManagement.BLL.Interfaces;
using LaptopRentalManagement.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using LaptopRentalManagement.Model.DTOs.Request;

namespace LaptopRentalManagement.Pages.User.Laptops
{
    public class Index : PageModel
    {
        private readonly ILaptopService _laptopService;
        private readonly ICategoryService _categoryService;
        private readonly IBrandService _brandService;

        public Index(ILaptopService laptopService, ICategoryService categoryService, IBrandService brandService)
        {
            _laptopService = laptopService;
            _categoryService = categoryService;
            _brandService = brandService;
        }

        public IList<LaptopResponse> Laptops { get; set; } = new List<LaptopResponse>();
        public IEnumerable<BrandResponse> Brands { get; set; } = new List<BrandResponse>();
        public IEnumerable<CategoryResponse> Categories { get; set; } = new List<CategoryResponse>();

        [BindProperty]
        public CreateLaptopRequest NewLaptop { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("AccountId");
            if (!int.TryParse(userIdClaim, out var userId))
            {
                TempData["Error"] = "Please login to continue";
                return RedirectToPage("/Account/Login");
            }

            Laptops = await _laptopService.GetAllAsync(new LaptopFilter() { AccountId = userId });
            Categories = await _categoryService.GetAllCategoriesAsync();
            Brands = await _brandService.GetAllBrandsAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // Nếu dữ liệu không hợp lệ, tải lại các danh sách cần thiết và hiển thị lại trang
                await OnGetAsync(); 
                return Page();
            }

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("AccountId");
            if (!int.TryParse(userIdClaim, out var userId))
            {
                TempData["Error"] = "Please login to continue";
                return RedirectToPage("/Account/Login");
            }

            // Gán AccountId từ claim của user đang đăng nhập
            NewLaptop.AccountId = userId;
            
            // NewLaptop đã được binding tự động từ form, bao gồm cả file ảnh và deposit
            await _laptopService.CreateAsync(NewLaptop);

            TempData["Success"] = "Your laptop has been listed successfully and is pending approval.";
            return RedirectToPage();
        }
    }
}