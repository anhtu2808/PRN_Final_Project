using LaptopRentalManagement.BLL.DTOs.Request;
using LaptopRentalManagement.BLL.DTOs.Response;
using LaptopRentalManagement.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LaptopRentalManagement.Pages.Manage.Customers
{
    public class UpdateAccountModel : PageModel
    {
        private readonly IAccountService _accountService;

        public UpdateAccountModel(IAccountService accountService)
        {
            _accountService = accountService;
        }
        public List<SelectListItem> Roles { get; set; }
        [BindProperty]
        public AccountDetailResponse Account { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            Roles = new List<SelectListItem>
{
    new SelectListItem { Value = "Admin", Text = "Admin" },
    new SelectListItem { Value = "Customer", Text = "Customer" },
    new SelectListItem { Value = "Staff", Text = "Staff" }
};
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
                Roles = new List<SelectListItem>
                {
                    new SelectListItem { Value = "Admin", Text = "Admin" },
                    new SelectListItem { Value = "Customer", Text = "Customer" },
                    new SelectListItem { Value = "Staff", Text = "Staff" }
                };
                return Page();
            }

            var updateRequest = new AccountUpdateRequest
            {
                Email = Account.Email,
                Role = Account.Role,
                Name = Account.Name
            };

            await _accountService.Update(updateRequest);

            return RedirectToPage("./Index");
        }

    }
}
