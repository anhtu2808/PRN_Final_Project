using LaptopRentalManagement.BLL.DTOs.Response;
using LaptopRentalManagement.BLL.Interfaces;
using LaptopRentalManagement.Model.DTOs.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LaptopRentalManagement.Pages.Laptops
{
    public class DetailsModel : PageModel
    {
        public LaptopResponse? Laptop { get; set; } = new LaptopResponse();
        public IList<LaptopResponse> SimilarLaptops { get; set; } = new List<LaptopResponse>();
        private readonly ILaptopService _laptopService;

        public DetailsModel(ILaptopService laptopService)
        {
            _laptopService = laptopService;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Laptop = await _laptopService.GetByIdAsync(id);
            if (Laptop == null)
            {
                return NotFound();
            }

            // Load similar laptops based on the same category
            var filter = new LaptopFilter()
            {
                CategoryIds = new List<int> { Laptop.Categories.FirstOrDefault()?.CategoryId ?? 0 },
            };
            SimilarLaptops = await _laptopService.GetAllAsync(filter);
            SimilarLaptops = SimilarLaptops
                .Where(x => x.LaptopId != Laptop.LaptopId) // Exclude the current laptop
                .Take(4) // Limit to 4 similar laptops
                .ToList();

            return Page();
        }
    }
}