using System.Runtime.CompilerServices;
using LaptopRentalManagement.BLL.DTOs.Response;
using LaptopRentalManagement.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LaptopRentalManagement.Pages.Manage.Customers
{
    [Authorize(Policy = "AdminOnly")]
    public class AccountListModel : PageModel
    {
        private readonly IAccountService _accountService;

        public AccountListModel(IAccountService accountService)
        {
            _accountService = accountService;
        }
        public IEnumerable<AccountResponse> Account { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Account = await _accountService.GetAll();
        }
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var success = await _accountService.Delete(id);
            if (!success)
            {
                TempData["Error"] = "Failed to delete account.";
            }
            return RedirectToPage();
        }
    }
}
