using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LaptopRentalManagement.Pages.Chat;

[Authorize]
public class ChatIndexModel : PageModel
{
    public void OnGet()
    {
        // Page initialization
    }
} 