using LaptopRentalManagement.BLL.DTOs.Request;
using LaptopRentalManagement.BLL.DTOs.Response;
using LaptopRentalManagement.DAL.Context;
using LaptopRentalManagement.DAL.Entities;
using LaptopRentalManagement.BLL.Interfaces;
using LaptopRentalManagement.Model.DTOs.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LaptopRentalManagement.Pages.Manage.Laptops
{
    public class IndexModel : PageModel
    {
        private readonly ILaptopService _laptopService;
        private readonly IBrandService _brandService;
        private readonly ICategoryService _categoryService;
        private readonly IAccountService _accountService;

        public IList<LaptopResponse> Laptops { get; set; } = new List<LaptopResponse>();

        [BindProperty(SupportsGet = true)] public LaptopFilter Filter { get; set; } = new();

        public SelectList CategorySelect { get; set; }
        public SelectList BrandSelect { get; set; }
        public SelectList AccountSelect { get; set; }
        public SelectList StatusSelect { get; set; }

        public IndexModel(
            ILaptopService laptopService,
            ICategoryService categoryService,
            IBrandService brandService,
            IAccountService accountService
        )
        {
            _laptopService = laptopService;
            _categoryService = categoryService;
            _brandService = brandService;
            _accountService = accountService;
        }

        public async Task OnGetAsync()
        {
            // 1. Load categories via CategoryService
            var cats = await _categoryService.GetAllCategoriesAsync();

            // 2. Load brands + accounts trực tiếp từ DbContext
            var brands = await _brandService.GetAllBrandsAsync();
            var users = await _accountService.GetAll();

            // 3. Build các SelectList
            CategorySelect = new SelectList(cats, "CategoryId", "Name", Filter.CategoryId);
            BrandSelect = new SelectList(brands, "BrandId", "Name", Filter.BrandId);
            AccountSelect = new SelectList(users, "AccountId", "Name", Filter.AccountId);
            StatusSelect = new SelectList(new[]
            {
                new { Value = "", Text = "All" },
                new { Value = "Available", Text = "Available" },
                new { Value = "PendingApproval", Text = "Pending Approval" },
                new { Value = "Rented", Text = "Rented" }
            }, "Value", "Text", Filter.Status);

            // 4. Lấy danh sách laptop đã filter
            Laptops = await _laptopService.GetAllAsync(Filter);
            // Console.WriteLine($"Laptops owner: {Laptops[0].Owner.Name}");
        }

        public async Task<JsonResult> OnGetGetLaptopAsync(int id)
        {
            var laptop = await _laptopService.GetByIdAsync(id);
            if (laptop == null)
                return new JsonResult(null) { StatusCode = 404 };

            return new JsonResult(new
            {
                laptopId = laptop.LaptopId,
                name = laptop.Name,
                brandId = laptop.Brand.BrandId,
                accountId = laptop.Owner.AccountId,
                categoryIds = laptop.Categories.Select(c => c.CategoryId).ToList(),
                pricePerDay = laptop.PricePerDay,
                cpu = laptop.Cpu,
                ram = laptop.Ram,
                storage = laptop.Storage,
                status = laptop.Status,
                brandName = laptop.Brand.Name,
                ownerName = laptop.Owner.Name,
                categories = laptop.Categories.Select(c => c.Name).ToList()
            });
        }

        public async Task<IActionResult> OnPostCreateAsync(CreateLaptopRequest req)
        {
            if (!ModelState.IsValid) return Page();
            await _laptopService.CreateAsync(req);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEditAsync(EditLaptopRequest req)
        {
            if (!ModelState.IsValid) return Page();
            await _laptopService.UpdateAsync(req);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnGetDeleteAsync(int id)
        {
            await _laptopService.DeleteAsync(id);
            return RedirectToPage();
        }
    }
}