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
        private readonly IBrandService _brandService;

        public IndexModel(ILaptopService laptopService, ICategoryService categoryService, IBrandService brandService)
        {
            _categoryService = categoryService;
            _brandService = brandService;
            _laptopService = laptopService;
        }


        public IList<LaptopResponse> Laptops { get; private set; } = new List<LaptopResponse>();
        public IEnumerable<CategoryResponse> Categories { get; set; } = new List<CategoryResponse>();
        public IEnumerable<BrandResponse> Brands { get; set; } = new List<BrandResponse>();
        [BindProperty(SupportsGet = true)] public LaptopFilter Filter { get; set; } = new();

        public int CurrentPage { get; set; }
        public int PageSize { get; set; } = 9; // 9 items per page
        public int TotalPages { get; set; }

        public async Task OnGet(
            List<int>? categoryIds, List<int>? brandIds,
            decimal? minPrice, decimal? maxPrice,
            bool availableOnly = false, string? name = null,[FromQuery] int page = 1)
        {
            CurrentPage = page;
            Categories = await _categoryService.GetAllCategoriesAsync();
            Brands = await _brandService.GetAllBrandsAsync();


            var filter = new LaptopFilter
            {
                CategoryIds = categoryIds,
                BrandIds = brandIds,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                Name = name,
                Status = availableOnly ? "Available" : null
            };

            var allLaptops = await _laptopService.GetAllAsync(filter);
            TotalPages = (int)Math.Ceiling(allLaptops.Count / (double)PageSize);
            Laptops = allLaptops.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
        }
    }
}