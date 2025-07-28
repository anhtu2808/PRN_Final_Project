using LaptopRentalManagement.BLL.DTOs.Request;
using LaptopRentalManagement.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LaptopRentalManagement.Pages.Manage.Customers
{
    //[Authorize(Policy = "AdminOnly")]
    public class CreateAccountModel : PageModel
    {

        private readonly IAccountService _accountService;

        public CreateAccountModel(IAccountService accountService)
        {
            _accountService = accountService;
        }
        public List<SelectListItem> Roles { get; set; }

        public IActionResult OnGet()
        {
            Roles = new List<SelectListItem>
        {
            new SelectListItem { Value = "Admin", Text = "Admin" },
            new SelectListItem { Value = "Customer", Text = "Customer" },
            new SelectListItem { Value = "Staff", Text = "Staff" }
        };
            return Page();
        }

        [BindProperty]
        public AccountRegisterRequest Account { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Roles = new List<SelectListItem>
        {
            new SelectListItem { Value = "Admin", Text = "Admin" },
            new SelectListItem { Value = "Customer", Text = "Customer" },
            new SelectListItem { Value = "Staff", Text = "Staff" }
        };
                return Page();
            }
            await _accountService.AdminCreateAccount(Account);

            return RedirectToPage("./AccountList");
        }
    }
}
