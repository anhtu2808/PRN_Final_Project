using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaptopRentalManagement.BLL.DTOs.Response;
using LaptopRentalManagement.BLL.Interfaces;
using LaptopRentalManagement.DAL.Context;
using LaptopRentalManagement.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LaptopRentalManagement.Pages.Account
{
    public class IndexModel : PageModel
    {
        private readonly IAccountService _accountService;

        public IndexModel(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public IEnumerable<AccountDetailResponse> Account { get;set; } = default!;

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
