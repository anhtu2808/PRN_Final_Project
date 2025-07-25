using LaptopRentalManagement.BLL.Interfaces;
using LaptopRentalManagement.BLL.Services;
using LaptopRentalManagement.BLL.DTOs.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LaptopRentalManagement.Pages.Brands
{
    public class IndexModel : PageModel
    {
        private readonly IBrandService _brandService;

        public IndexModel(IBrandService brandService)
        {
            _brandService = brandService;
        }

        public IEnumerable<BrandResponse> Brands { get; set; } = new List<BrandResponse>();

        public async Task OnGetAsync()
        {
            Brands = await _brandService.GetAllBrandsAsync();
        }
    }
}
