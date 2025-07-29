using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LaptopRentalManagement.Pages.User.Laptops
{
    [Authorize(Policy = "CustomerOnly")]
    public class _RentOutModalModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
