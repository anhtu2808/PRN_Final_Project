using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using LaptopRentalManagement.BLL.DTOs.Request;
using LaptopRentalManagement.BLL.DTOs.Response;
using LaptopRentalManagement.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Scripting;

namespace LaptopRentalManagement.Pages.User
{
    [Authorize(Policy = "CustomerOnly")]
    public class UpdateProfileModel : PageModel
    {
        private readonly IAccountService _accountService;

        public UpdateProfileModel(IAccountService accountService)
        {
            _accountService = accountService;
        }
        public class AccountUpdateModel
        {
            public int AccountId { get; set; }

            [Required]
            public string Name { get; set; } = string.Empty;

            [Required]
            [EmailAddress]
            public string Email { get; set; } = string.Empty;

            public string Role { get; set; } = "Customer";

            public string? NewPassword { get; set; }
        }

        [BindProperty]
        public AccountUpdateModel Account { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("AccountId");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int id))
            {
                return NotFound();
            }

            var account = await _accountService.GetById(id);
            if (account == null)
            {
                return NotFound();
            }

            Account = new AccountUpdateModel
            {
                AccountId = account.AccountId,
                Name = account.Name,
                Email = account.Email,
                Role = account.Role
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var existingAccount = await _accountService.GetById(Account.AccountId);
            if (existingAccount == null)
            {
                return NotFound();
            }

            string? passwordHash = null;

            var updateRequest = new AccountUpdateRequest
            {
                AccountId = Account.AccountId,
                Email = Account.Email,
                Name = Account.Name,
                Role = "Customer",
                PasswordHash = passwordHash
            };

            await _accountService.Update(updateRequest);

            return RedirectToPage("./Index");
        }

    }
}
