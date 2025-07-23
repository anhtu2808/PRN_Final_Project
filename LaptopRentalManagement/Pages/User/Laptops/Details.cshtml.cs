using LaptopRentalManagement.BLL.DTOs.Request;
using LaptopRentalManagement.BLL.DTOs.Response;
using LaptopRentalManagement.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LaptopRentalManagement.Pages.User.Laptops;

public class DetailsModel : PageModel
{
    private readonly ILaptopService _laptopService;

    public DetailsModel(ILaptopService laptopService)
    {
        _laptopService = laptopService;
    }

    public LaptopResponse? Laptop { get; private set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        EditLaptopRequest request = new EditLaptopRequest();

        if (Laptop == null)
        {
            return NotFound();
        }

        return Page();
    }
}