using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using LaptopRentalManagement.BLL.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LaptopRentalManagement.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly IAccountService _accountService;

        public LoginModel(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public string? ErrorMessage { get; set; }
        public string? ReturnUrl { get; set; }

        public class InputModel
        {
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;

            [Display(Name = "Remember me")]
            public bool RememberMe { get; set; }
        }

        public void OnGet(string? returnUrl = null)
        {
            ReturnUrl = returnUrl;

            // Clear any existing authentication
            if (User.Identity?.IsAuthenticated == true)
            {
                Response.Redirect("/");
            }
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            ReturnUrl = returnUrl;
            returnUrl ??= Url.Content("~/");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var account = await _accountService.Login(Input.Email, Input.Password);

                if (account != null)
                {
                    // Validate role
                    if (!IsValidRole(account.Role))
                    {
                        ErrorMessage = "Invalid user role. Please contact administrator.";
                        return Page();
                    }

                    // Create claims for the authenticated user
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, account.AccountId.ToString()),
                        new Claim(ClaimTypes.Name, account.Name ?? account.Email),
                        new Claim(ClaimTypes.Email, account.Email),
                        new Claim(ClaimTypes.Role, account.Role),
                        new Claim("AccountId", account.AccountId.ToString())
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = Input.RememberMe,
                        ExpiresUtc = Input.RememberMe ? DateTimeOffset.UtcNow.AddDays(30) : DateTimeOffset.UtcNow.AddHours(1),
                        AllowRefresh = true
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authProperties);

                    // Role-based redirection
                    return account.Role switch
                    {
                        "Admin" => RedirectToPage("/Admin/Dashboard"),
                        "Staff" => RedirectToPage("/Staff/Dashboard"),
                        "Customer" => string.IsNullOrEmpty(returnUrl) || returnUrl == "/"
                            ? RedirectToPage("/Customer/Dashboard")
                            : LocalRedirect(returnUrl),
                        _ => RedirectToPage("/Account/AccessDenied")
                    };
                }
                else
                {
                    ErrorMessage = "Invalid email or password. Please try again.";
                    return Page();
                }
            }
            catch (UnauthorizedAccessException)
            {
                ErrorMessage = "Your account has been disabled. Please contact administrator.";
                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = "An error occurred during login. Please try again.";
                // TODO: Add logging here
                // _logger.LogError(ex, "Login error for user {Email}", Input.Email);
                return Page();
            }
        }

        private static bool IsValidRole(string role)
        {
            return role is "Admin" or "Staff" or "Customer";
        }
    }
}

