using LaptopRentalManagement.BLL.DTOs.Response;
using LaptopRentalManagement.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LaptopRentalManagement.Pages.Laptops
{
    public class DetailsModel : PageModel
    {
        public LaptopResponse? Laptop { get; set; } = new LaptopResponse();
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

            return Page();
        }
    }
}