using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using LaptopRentalManagement.BLL.DTOs.Request;
using LaptopRentalManagement.BLL.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LaptopRentalManagement.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly IAccountService _accountService;

        public RegisterModel(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public string? ErrorMessage { get; set; }
        public string? SuccessMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Please enter your full name")]
            [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
            public string Name { get; set; } = string.Empty;

            [Required(ErrorMessage = "Please enter your email address")]
            [EmailAddress(ErrorMessage = "Please enter a valid email address")]
            public string Email { get; set; } = string.Empty;

            [Required(ErrorMessage = "Please enter a password")]
            [DataType(DataType.Password)]
            [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long")]
            public string Password { get; set; } = string.Empty;

            [Required(ErrorMessage = "Please confirm your password")]
            [DataType(DataType.Password)]
            [Display(Name = "Confirm Password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match")]
            public string ConfirmPassword { get; set; } = string.Empty;
        }

        public void OnGet()
        {
            // Clear any existing authentication
            if (User.Identity?.IsAuthenticated == true)
            {
                Response.Redirect("/");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var registerRequest = new AccountRegisterRequest
                {
                    Name = Input.Name,
                    Email = Input.Email,
                    PasswordHash = Input.Password,
                    Role = "Customer"
                };

                var result = await _accountService.CustomerRegister(registerRequest);

                if (result != null)
                {
                    // Registration successful - redirect to login with success message
                    TempData["SuccessMessage"] = "Account created successfully! Please log in with your credentials.";
                    return RedirectToPage("/Account/Login");
                }
                else
                {
                    ErrorMessage = "Registration failed. Please try again.";
                    return Page();
                }
            }
            catch (InvalidOperationException ex)
            {
                // Handle duplicate email or other business logic errors
                ErrorMessage = ex.Message;
                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = "An error occurred during registration. Please try again.";
                // TODO: Add logging here
                // _logger.LogError(ex, "Registration error for email {Email}", Input.Email);
                return Page();
            }
        }
    }
}

