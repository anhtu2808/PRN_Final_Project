using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LaptopRentalManagement.Pages.User.Rental_order
{
    [Authorize(Policy = "CustomerOnly")]
    public class _ComplainModalModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
