using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LaptopRentalManagement.Pages.Laptops
{
    public class IndexModel : PageModel
    {
        public string? Category { get; set; }
        public string? Brand { get; set; }
        public decimal? PriceMin { get; set; }
        public decimal? PriceMax { get; set; }
        public bool AvailableOnly { get; set; }
        public string? SearchQuery { get; set; }
        public string SortBy { get; set; } = "featured";
        
        public void OnGet(string? category, string? brand, decimal? priceMin, decimal? priceMax, bool availableOnly = false, string? q = null, string sortBy = "featured")
        {
            Category = category;
            Brand = brand;
            PriceMin = priceMin;
            PriceMax = priceMax;
            AvailableOnly = availableOnly;
            SearchQuery = q;
            SortBy = sortBy;
            
            // In a real application, you would load laptops from database here
            // based on the filter parameters
            // var laptops = _laptopService.GetLaptops(new LaptopFilterRequest 
            // {
            //     Category = category,
            //     Brand = brand,
            //     PriceMin = priceMin,
            //     PriceMax = priceMax,
            //     AvailableOnly = availableOnly,
            //     SearchQuery = q,
            //     SortBy = sortBy
            // });
        }
    }
} 