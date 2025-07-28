using System.Security.Claims;
using LaptopRentalManagement.BLL.DTOs.Request;
using LaptopRentalManagement.BLL.DTOs.Response;
using LaptopRentalManagement.BLL.Interfaces;
using LaptopRentalManagement.Model.DTOs.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LaptopRentalManagement.Pages.Laptops
{
    public class DetailsModel : PageModel
    {
        private readonly ILaptopService _laptopService;
        private readonly IFeedbackService _feedbackService;

        // IOrderService has been removed as order creation logic moved to the Checkout page
        public DetailsModel(ILaptopService laptopService, IFeedbackService feedbackService)
        {
            _laptopService = laptopService;
            _feedbackService = feedbackService;
        }

        [BindProperty] public CreateReviewRequest NewReview { get; set; } = new();
        public int? EligibleOrderId { get; set; }
        public LaptopReviewSummaryResponse? ReviewSummary { get; set; }
        public IList<SlotResponse> Slots { get; set; } = new List<SlotResponse>();
        [BindProperty] public List<int> SelectedSlots { get; set; }
        public LaptopResponse? Laptop { get; set; } = new LaptopResponse();
        public IList<LaptopResponse> SimilarLaptops { get; set; } = new List<LaptopResponse>();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Laptop = await _laptopService.GetByIdAsync(id);
            if (Laptop == null)
            {
                return NotFound();
            }
            Slots = Laptop.Slots;
            ReviewSummary = await _feedbackService.GetLaptopReviewsAsync(id);


            var filter = new LaptopFilter()
            {
                CategoryIds = new List<int> { Laptop.Categories.FirstOrDefault()?.CategoryId ?? 0 },
            };
            SimilarLaptops = await _laptopService.GetAllAsync(filter);
            SimilarLaptops = SimilarLaptops
                .Where(x => x.LaptopId != Laptop.LaptopId)
                .Take(4)
                .ToList();

            return Page();
        }

        // This method is called when the form on the Details page is submitted
        public IActionResult OnPost(int id, List<int> selectedSlots)
        {
            // Check if the user has selected any dates
            if (selectedSlots == null || !selectedSlots.Any())
            {
                TempData["Error"] = "Please select at least one rental day."; // Translated
                return RedirectToPage(new { id = id }); // Reload the Details page if no selection
            }

            // Redirect to the Checkout page, passing the laptop ID and the array of selected slots
            // ASP.NET Core will automatically convert this to a URL like: /Checkout?id=123&selectedSlots=1&selectedSlots=2
            return RedirectToPage("/Checkout", new { id = id, selectedSlots = selectedSlots });
        }

        [Authorize]
        public async Task<IActionResult> OnPostSubmitReviewAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync(id);
                return Page();
            }

            try
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

                await _feedbackService.CreateReviewAsync(NewReview, userId);
                TempData["Success"] = "Your review has been submitted successfully!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An unexpected error occurred while submitting your review: " + ex.Message;
            }

            return RedirectToPage(new { id = id });
        }
    }
}