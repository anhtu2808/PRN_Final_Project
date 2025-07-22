using LaptopRentalManagement.BLL.DTOs.Response;
using LaptopRentalManagement.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LaptopRentalManagement.Pages;

public class IndexModel : PageModel
{
    private readonly ILaptopService _laptopService;

    public IndexModel(ILaptopService laptopService)
    {
        _laptopService = laptopService;
    }
    public IList<LaptopResponse> FeaturedLaptops { get; private set; } = new List<LaptopResponse>();

    public async Task OnGetAsync()
    {
        // Load top 3 most rented laptops
        FeaturedLaptops = await _laptopService.GetTopRentedLaptopsAsync(3);
    }
}