using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaptopRentalManagement.BLL.DTOs.Request;
using LaptopRentalManagement.BLL.DTOs.Response;
using LaptopRentalManagement.BLL.Interfaces;
using LaptopRentalManagement.DAL.Context;
using LaptopRentalManagement.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LaptopRentalManagement.Pages.Account
{
    public class EditModel : PageModel
    {
        private readonly IAccountService _accountService;

        public EditModel(IAccountService accountService)
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

             //await _accountService.Update(Account);

            return RedirectToPage("./Index");
        }

    }
}
