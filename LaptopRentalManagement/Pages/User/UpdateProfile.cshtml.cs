using LaptopRentalManagement.BLL.DTOs.Request;
using LaptopRentalManagement.BLL.DTOs.Response;
using LaptopRentalManagement.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        [BindProperty]
        public AccountDetailResponse Account { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _accountService.GetById(id.Value);
            if (account == null)
            {
                return NotFound();
            }
            Account = account;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var updateRequest = new AccountUpdateRequest
            {
                AccountId = Account.AccountId,
                Email = Account.Email,
                Role = Account.Role,
                Name = Account.Name,
                PasswordHash = Account.PasswordHash
            };

            await _accountService.Update(updateRequest);

            return RedirectToPage("./Index");
        }

    }
}
