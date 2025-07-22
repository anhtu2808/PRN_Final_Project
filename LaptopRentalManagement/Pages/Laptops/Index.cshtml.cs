using LaptopRentalManagement.BLL.DTOs.Response;
using LaptopRentalManagement.BLL.Interfaces;
using LaptopRentalManagement.Model.DTOs.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LaptopRentalManagement.Pages.Laptops
{
    public class IndexModel : PageModel
    {
        private readonly ILaptopService _laptopService;
        private readonly ICategoryService _categoryService;

        public IndexModel(ILaptopService laptopService, ICategoryService categoryService)
        {
            _categoryService = categoryService;
            _laptopService = laptopService;
        }


        public IList<LaptopResponse> Laptops { get; private set; } = new List<LaptopResponse>();
        public IEnumerable<CategoryResponse> Categories { get; set; } = new List<CategoryResponse>();
        public IList<BrandResponse> Brands { get; set; } = new List<BrandResponse>();
        [BindProperty(SupportsGet = true)] public LaptopFilter Filter { get; set; } = new();

        public async Task OnGet(
            List<int>? categoryIds, List<int>? brandIds,
            decimal? minPrice, decimal? maxPrice,
            bool availableOnly = false, string? name = null)
        {
            Categories = await _categoryService.GetAllCategoriesAsync();
            Brands = new List<BrandResponse> // Mock data for brands, replace with actual service call
            {
                new BrandResponse
                {
                    BrandId = 1,
                    Name = "Apple",
                    Description = "Premium laptops and devices",
                    LogoUrl = "https://upload.wikimedia.org/wikipedia/commons/f/fa/Apple_logo_black.svg",
                    Country = "USA"
                },
                new BrandResponse
                {
                    BrandId = 2,
                    Name = "Dell",
                    Description = "Business-class laptops",
                    LogoUrl = "https://upload.wikimedia.org/wikipedia/commons/4/48/Dell_Logo.svg",
                    Country = "USA"
                },
                new BrandResponse
                {
                    BrandId = 3,
                    Name = "HP",
                    Description = "Reliable performance laptops",
                    LogoUrl = "https://upload.wikimedia.org/wikipedia/commons/8/82/Hewlett_Packard_Enterprise_logo.svg",
                    Country = "USA"
                },
                new BrandResponse
                {
                    BrandId = 4,
                    Name = "Lenovo",
                    Description = "Value and performance",
                    LogoUrl = "https://upload.wikimedia.org/wikipedia/commons/0/0f/Lenovo_logo_2015.svg",
                    Country = "China"
                },
                new BrandResponse
                {
                    BrandId = 5,
                    Name = "ASUS",
                    Description = "Gaming and creative laptops",
                    LogoUrl = "https://upload.wikimedia.org/wikipedia/commons/0/0c/AsusTek_logo.svg",
                    Country = "Taiwan"
                }
            };


            var filter = new LaptopFilter
            {
                CategoryIds = categoryIds,
                BrandIds = brandIds,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                Name = name,
                Status = availableOnly ? "Available" : null
            };

            Laptops = await _laptopService.GetAllAsync(filter);
        }
    }
}