using System.Security.Claims;
using LaptopRentalManagement.BLL.DTOs.Request;
using LaptopRentalManagement.BLL.DTOs.Response;
using LaptopRentalManagement.BLL.Interfaces;
using LaptopRentalManagement.BLL.Services;
using LaptopRentalManagement.DAL.Entities;
using LaptopRentalManagement.Model.DTOs.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LaptopRentalManagement.Pages.Laptops
{
    public class DetailsModel : PageModel
    {
        private readonly ILaptopService _laptopService;
        private readonly IOrderService _orderService;
        private readonly IFeedbackService _feedbackService;

        public DetailsModel(ILaptopService laptopService, IOrderService orderService, IFeedbackService feedbackService)
        {
            _laptopService = laptopService;
            _orderService = orderService;
            _feedbackService = feedbackService;
        }

        [BindProperty] public CreateReviewRequest NewReview { get; set; } = new();
        public int? EligibleOrderId { get; set; }
        public LaptopReviewSummaryResponse? ReviewSummary { get; set; }

        public IList<SlotResponse> Slots { get; set; } = new List<SlotResponse>();

        [BindProperty] public List<int> SelectedSlots { get; set; }
        public LaptopResponse? Laptop { get; set; } = new LaptopResponse();
        public IList<LaptopResponse> SimilarLaptops { get; set; } = new List<LaptopResponse>();

        [Authorize]
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
                // var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var userId = 2; // For testing, replace with actual user ID retrieval logic
                await _feedbackService.CreateReviewAsync(NewReview, userId);
                NewReview.OrderId = 29;

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


        public async Task<IActionResult> OnGetAsync(int id)
        {
            Laptop = await _laptopService.GetByIdAsync(id);
            Slots = Laptop.Slots;
            if (Laptop == null)
            {
                return NotFound();
            }

            // Load similar laptops based on the same category
            var filter = new LaptopFilter()
            {
                CategoryIds = new List<int> { Laptop.Categories.FirstOrDefault()?.CategoryId ?? 0 },
            };
            SimilarLaptops = await _laptopService.GetAllAsync(filter);
            SimilarLaptops = SimilarLaptops
                .Where(x => x.LaptopId != Laptop.LaptopId) // Exclude the current laptop
                .Take(4) // Limit to 4 similar laptops
                .ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            Laptop = await _laptopService.GetByIdAsync(id);
            if (Laptop == null)
            {
                return NotFound();
            }

            if (SelectedSlots == null || !SelectedSlots.Any())
            {
                ModelState.AddModelError(string.Empty, "Bạn phải chọn ít nhất 1 ngày thuê.");
                return Page();
            }

            var totalDays = SelectedSlots.Count;
            var pricePerDay = Laptop.PricePerDay;
            decimal totalCharge = totalDays * pricePerDay;

            await _orderService.CreateAsync(new()
            {
                LaptopId = Laptop.LaptopId,
                RenterId = 2, // Nhớ đổi lại
                TotalCharge = totalCharge,
                SlotIds = SelectedSlots
            });


            return RedirectToPage("/Laptops/Index");
        }
    }
}