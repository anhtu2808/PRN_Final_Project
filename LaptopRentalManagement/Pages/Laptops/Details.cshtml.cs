using LaptopRentalManagement.BLL.DTOs.Request;
using LaptopRentalManagement.BLL.DTOs.Response;
using LaptopRentalManagement.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace LaptopRentalManagement.Pages.Laptops
{
    [AllowAnonymous]
    public class DetailsModel : PageModel
    {
        private readonly ILaptopService _laptopService;
        private readonly IFeedbackService _feedbackService;

        public LaptopResponse? Laptop { get; set; }
        public LaptopReviewSummaryResponse? ReviewSummary { get; set; }
        public int? EligibleOrderId { get; set; }

        [BindProperty]
        public CreateReviewRequest NewReview { get; set; } = new();

        public DetailsModel(ILaptopService laptopService, IFeedbackService feedbackService)
        {
            _laptopService = laptopService;
            _feedbackService = feedbackService;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            // Lấy thông tin chi tiết của laptop
            Laptop = await _laptopService.GetByIdAsync(id);
            if (Laptop == null)
            {
                return NotFound();
            }
            ReviewSummary = await _feedbackService.GetLaptopReviewsAsync(id);
            if (User.Identity?.IsAuthenticated ?? false)
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            }

            return Page();
        }


        //[Authorize]
        public async Task<IActionResult> OnPostSubmitReviewAsync(int id)
        {
			if (!ModelState.IsValid)
			{
				foreach (var modelState in ModelState)
				{
					foreach (var error in modelState.Value.Errors)
					{
						Console.WriteLine($"Model error in {modelState.Key}: {error.ErrorMessage}");
					}
				}

				await OnGetAsync(id);
				return Page();
			}

			try
			{
                var userId = 2; 
                NewReview.OrderId = 26; 
                await _feedbackService.CreateReviewAsync(NewReview, userId);
                TempData["Success"] = "Your review has been submitted successfully!";
            }
            catch (UnauthorizedAccessException ex)
            {
                TempData["Error"] = ex.Message;
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An unexpected error occurred while submitting your review.";
            }

            return RedirectToPage(new { id = id });
        }
    }
}