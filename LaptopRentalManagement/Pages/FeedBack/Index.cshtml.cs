using LaptopRentalManagement.BLL.DTOs.Request;
using LaptopRentalManagement.BLL.DTOs.Response;
using LaptopRentalManagement.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LaptopRentalManagement.Pages.FeedBack
{
    public class IndexModel : PageModel
    {
        private readonly IFeedbackService _feedbackService;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(IFeedbackService feedbackService, ILogger<IndexModel> logger)
        {
            _feedbackService = feedbackService;
            _logger = logger;
        }

        public IEnumerable<ReviewResponse> Reviews { get; set; } = new List<ReviewResponse>();
        public ReviewStatisticsResponse Stats { get; set; } = new ReviewStatisticsResponse();

        [BindProperty(SupportsGet = true)] public string? SearchTerm { get; set; }
        [BindProperty(SupportsGet = true)] public int? SelectedRating { get; set; }
        [BindProperty(SupportsGet = true)] public DateTime? DateFrom { get; set; }
        [BindProperty(SupportsGet = true)] public DateTime? DateTo { get; set; }
        [BindProperty(SupportsGet = true)] public int CurrentPage { get; set; } = 1;

        public int PageSize { get; set; } = 10;
        public int TotalPages { get; set; }

        public async Task OnGetAsync()
        {
            try
            {
                Stats = await _feedbackService.GetReviewStatisticsAsync();
                var result = await _feedbackService.GetFilteredReviewsAsync(SearchTerm, SelectedRating, DateFrom, DateTo, CurrentPage, PageSize);
                Reviews = result.Reviews;
                TotalPages = result.TotalPages;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading feedback management page.");
                TempData["Error"] = "An error occurred while loading reviews.";
                Reviews = new List<ReviewResponse>();
            }
        }

        public async Task<IActionResult> OnGetReviewDetailsAsync(int id)
        {
            var review = await _feedbackService.GetReviewByIdAsync(id);
            return review != null
                ? new JsonResult(new { success = true, review })
                : new JsonResult(new { success = false, message = "Review not found." });
        }

        public async Task<IActionResult> OnPostUpdateAsync(int id, [FromForm] UpdateReviewRequest request)
        {
            if (!ModelState.IsValid)
            {
                return new JsonResult(new { success = false, message = "Invalid data submitted." });
            }
            try
            {
                await _feedbackService.AdminUpdateReviewAsync(id, request);
                TempData["Success"] = "Review updated successfully!";
                return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating review {ReviewId}", id);
                return new JsonResult(new { success = false, message = ex.Message });
            }
        }

        // === PHIÊN BẢN CHÍNH XÁC CỦA HANDLER DELETE ===
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            try
            {
                await _feedbackService.AdminDeleteReviewAsync(id);
                // Đặt TempData để hiển thị thông báo SAU KHI JavaScript tải lại trang
                TempData["Success"] = "Review deleted successfully!";
                // Trả về JSON để JavaScript biết đã thành công
                return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting review {ReviewId}", id);
                // Trả về lỗi dưới dạng JSON để JavaScript có thể hiển thị
                return new JsonResult(new { success = false, message = "An error occurred while deleting the review." });
            }
        }
    }
}