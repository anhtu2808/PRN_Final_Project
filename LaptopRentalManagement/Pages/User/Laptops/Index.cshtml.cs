using System.Security.Claims;
using LaptopRentalManagement.BLL.DTOs.Request;
using LaptopRentalManagement.BLL.DTOs.Response;
using LaptopRentalManagement.BLL.Interfaces;
using LaptopRentalManagement.DAL.Entities;
using LaptopRentalManagement.Model.DTOs.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LaptopRentalManagement.Pages.User.Laptops
{
    [Authorize(Policy = "CustomerOnly")]
    public class Index : PageModel
    {
        private readonly ILaptopService _laptopService;
        private readonly ICategoryService _categoryService;

        public Index(ILaptopService laptopService, ICategoryService categoryService)
        {
            _laptopService = laptopService;
            _categoryService = categoryService;
        }

        public IList<LaptopResponse> Laptops { get; set; } = new List<LaptopResponse>();

        // Mock brands
        public List<Brand> Brands { get; set; } = new()
        {
            new() { BrandId = 1, Name = "Apple" },
            new() { BrandId = 2, Name = "Dell" },
            new() { BrandId = 3, Name = "Asus" },
        };

        public IEnumerable<CategoryResponse> Categories { get; set; } = new List<CategoryResponse>();

        public async Task<IActionResult> OnGetAsync()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("AccountId");
            if (!int.TryParse(userIdClaim, out var userId))
            {
                TempData["Error"] = "Please login to continue";
                return RedirectToPage("/Account/Login");
            }
            var accountId = int.Parse(userIdClaim);
            Laptops = await _laptopService.GetAllAsync(new LaptopFilter { AccountId = accountId });
            Categories = await _categoryService.GetAllCategoriesAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var form = Request.Form;
            var request = new CreateLaptopRequest
            {
                Name = form["Name"],
                Description = form["Description"],
                // ImageURL = form["ImageURL"],
                BrandId = int.Parse(form["BrandId"]),
                AccountId = int.Parse(form["AccountId"]),
                PricePerDay = decimal.Parse(form["PricePerDay"]),
                Cpu = form["Cpu"],
                Ram = int.Parse(form["Ram"]),
                Storage = int.Parse(form["Storage"]),
                CategoryIds = form["CategoryIds"]
                    .ToList()
                    .Select(int.Parse)
                    .ToList()
            };

            await _laptopService.CreateAsync(request);
            return RedirectToPage();
        }
    }
}