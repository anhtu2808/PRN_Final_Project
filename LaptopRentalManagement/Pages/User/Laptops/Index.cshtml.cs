using LaptopRentalManagement.BLL.DTOs.Response;
using LaptopRentalManagement.BLL.Interfaces;
using LaptopRentalManagement.Model.DTOs.Request;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LaptopRentalManagement.Pages.User.Laptops;

public class Index : PageModel
{
    private readonly ILaptopService _laptopService;

    public Index(ILaptopService laptopService)
    {
        _laptopService = laptopService;
    }

    public IList<LaptopResponse> Laptops { get; set; } = new List<LaptopResponse>();

    public async Task OnGetAsync()
    {
        // var userId = User.Identity?.Name;
        var accountId = 1;
        var laptopFilter = new LaptopFilter()
        {
            AccountId = accountId
        };

        Laptops = await _laptopService.GetAllAsync(laptopFilter);
    }
}