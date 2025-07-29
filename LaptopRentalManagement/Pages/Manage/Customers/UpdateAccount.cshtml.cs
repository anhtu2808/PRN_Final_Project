using System.ComponentModel.DataAnnotations;
using LaptopRentalManagement.BLL.DTOs.Request;
using LaptopRentalManagement.BLL.DTOs.Response;
using LaptopRentalManagement.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Scripting;

namespace LaptopRentalManagement.Pages.Manage.Customers
{
    [Authorize(Policy = "AdminOnly")]
    public class UpdateAccountModel : PageModel
    {
        private readonly IAccountService _accountService;

        public UpdateAccountModel(IAccountService accountService)
        {
            _accountService = accountService;
        }
        public List<SelectListItem> Roles { get; set; } = new();

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public class InputModel
        {
            public int AccountId { get; set; }

            [Required]
            [EmailAddress]
            public string Email { get; set; } = null!;

            [Required]
            public string Role { get; set; } = null!;

            [Required]
            public string Name { get; set; } = null!;

            [DataType(DataType.Password)]
            public string? NewPassword { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var account = await _accountService.GetById(id.Value);
            if (account == null)
                return NotFound();

            Input = new InputModel
            {
                AccountId = account.AccountId,
                Email = account.Email,
                Role = account.Role,
                Name = account.Name
            };

            Roles = GetRoleList();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Roles = GetRoleList();

            if (!ModelState.IsValid)
                return Page();

            var existingAccount = await _accountService.GetById(Input.AccountId);
            if (existingAccount == null)
                return NotFound();

            var updateRequest = new AccountUpdateRequest
            {
                AccountId = Input.AccountId,
                Email = Input.Email,
                Role = Input.Role,
                Name = Input.Name,
                PasswordHash = string.IsNullOrWhiteSpace(Input.NewPassword)
                    ? existingAccount.PasswordHash
                    : Input.NewPassword
            };

            await _accountService.Update(updateRequest);
            return RedirectToPage("./AccountList");
        }

        private List<SelectListItem> GetRoleList() => new()
        {
            new SelectListItem { Value = "Admin", Text = "Admin" },
            new SelectListItem { Value = "Customer", Text = "Customer" },
            new SelectListItem { Value = "Staff", Text = "Staff" }
        };

    }
}
