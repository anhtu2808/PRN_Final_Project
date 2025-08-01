﻿using LaptopRentalManagement.BLL.DTOs.Request;
using LaptopRentalManagement.BLL.DTOs.Response;
using LaptopRentalManagement.BLL.Interfaces;
using LaptopRentalManagement.DAL.Entities;
using LaptopRentalManagement.Model.DTOs.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace LaptopRentalManagement.Pages.User.Rental_order
{
	public class IndexModel : PageModel
	{
		private readonly IOrderService _orderService;

		public IndexModel(
		IOrderService orderService)
		{
			_orderService = orderService;
		}
                public IList<OrderResponse> MyRentalOrder { get; set; } = new List<OrderResponse>();

                [BindProperty(SupportsGet = true)]
                public string? Status { get; set; }

                public async Task<IActionResult> OnGet()
                {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("AccountId");
            if (!int.TryParse(userIdClaim, out var userId))
            {
                TempData["Error"] = "Please login to continue";
                return RedirectToPage("/Account/Login");
            }
            OrderFilter orderFilter = new()
                        {
                                RenterId = int.Parse(userIdClaim),
                                Status = Status,
            };

			MyRentalOrder = await _orderService.GetAllAsync(orderFilter);
            return Page();
        }

	}
}
