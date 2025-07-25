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
    public class CustomerRegisterModel : PageModel
    {
        private readonly IAccountService _accountService;

        public CustomerRegisterModel(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public string? ErrorMessage { get; set; }
        public string? SuccessMessage { get; set; }

        public class InputModel
        {
            public string Name { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;

            [Required(ErrorMessage = "Please confirm your password")]
            [DataType(DataType.Password)]
            [Display(Name = "Confirm Password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match")]
            public string ConfirmPassword { get; set; } = string.Empty;

            [Required(ErrorMessage = "You must agree to the terms and conditions")]
            [Range(typeof(bool), "true", "true", ErrorMessage = "You must agree to the terms and conditions")]
            [Display(Name = "I agree to the Terms of Service and Privacy Policy")]
            public bool AgreeToTerms { get; set; }
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
                    PasswordHash = Input.Password, // This will be hashed in the service layer
                    Role = "Customer"
                };

                var result = await _accountService.CustomerRegister(registerRequest);

                if (result != null)
                {
                    // Registration successful - automatically sign in the user
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, result.AccountId.ToString()),
                        new Claim(ClaimTypes.Name, result.Name ?? result.Email),
                        new Claim(ClaimTypes.Email, result.Email),
                        new Claim(ClaimTypes.Role, result.Role),
                        new Claim("AccountId", result.AccountId.ToString())
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = false,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1),
                        AllowRefresh = true
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authProperties);

                    // Redirect to customer dashboard or welcome page
                    return RedirectToPage("/Customer/Dashboard");
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

