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
        
        // Add property for slot validation
        public bool HasAvailableSlots { get; set; }
        public int AvailableSlotsCount { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Laptop = await _laptopService.GetByIdAsync(id);
            if (Laptop == null)
            {
                return NotFound();
            }
            Slots = Laptop.Slots;

            // Validate slot availability
            ValidateSlotAvailability();

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

        private void ValidateSlotAvailability()
        {
            if (Slots == null || !Slots.Any())
            {
                HasAvailableSlots = false;
                AvailableSlotsCount = 0;
                return;
            }

            var currentDate = DateOnly.FromDateTime(DateTime.UtcNow);
            
            // Count available slots that are today or in the future
            var availableSlots = Slots.Where(s => 
                s.Status == "Available" && 
                s.SlotDate >= currentDate
            ).ToList();

            AvailableSlotsCount = availableSlots.Count;
            HasAvailableSlots = AvailableSlotsCount > 0;
        }

        public IActionResult OnPost(int id, List<int> selectedSlots)
        {
            // Validate slot availability before processing
            if (!selectedSlots.Any())
            {
                TempData["Error"] = "Vui lòng chọn ít nhất một ngày thuê.";
                return RedirectToPage(new { id = id });
            }

            // Additional validation: check if selected slots are still available
            // This would require another service call to verify current slot status
            
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