using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LaptopRentalManagement.DAL.LaptopRentalManagement.DAL.Entities;
using LaptopRentalManagement.DAL.LaptopRentalManagement.DAL.Context;

namespace LaptopRentalManagement.Pages.FeedBack
{
    public class IndexModel : PageModel
    {
        private readonly LaptopRentalDbContext _context;

        public IndexModel(LaptopRentalDbContext context)
        {
            _context = context;
        }

        public IList<Review> Reviews { get; set; } = new List<Review>();

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? SelectedRating { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? DateFrom { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? DateTo { get; set; }

        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;

        public int PageSize { get; set; } = 10;
        public int TotalPages { get; set; }
        public int TotalReviews { get; set; }
        public double AverageRating { get; set; }
        public int PositiveReviews { get; set; }
        public int ThisMonthReviews { get; set; }

        public async Task OnGetAsync()
        {
            try
            {
                // Get all reviews with related data
                var allReviews = await _context.Reviews
                    .Include(r => r.Rater)
                    .Include(r => r.Order)
                        .ThenInclude(o => o.Laptop)
                    .ToListAsync();

                // Calculate statistics
                TotalReviews = allReviews.Count;
                AverageRating = allReviews.Any() ? allReviews.Average(r => r.Rating) : 0;
                PositiveReviews = allReviews.Count(r => r.Rating >= 4);
                ThisMonthReviews = allReviews.Count(r => r.CreatedAt >= DateTime.Now.AddDays(-30));

                // Apply filters
                var query = allReviews.AsQueryable();

                if (!string.IsNullOrEmpty(SearchTerm))
                {
                    query = query.Where(r => r.Comment != null && r.Comment.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase));
                }

                if (SelectedRating.HasValue)
                {
                    query = query.Where(r => r.Rating == SelectedRating.Value);
                }

                if (DateFrom.HasValue)
                {
                    query = query.Where(r => r.CreatedAt >= DateFrom.Value);
                }

                if (DateTo.HasValue)
                {
                    query = query.Where(r => r.CreatedAt <= DateTo.Value.AddDays(1));
                }

                // Calculate pagination
                var totalFiltered = query.Count();
                TotalPages = (int)Math.Ceiling(totalFiltered / (double)PageSize);

                // Ensure CurrentPage is valid
                if (CurrentPage < 1) CurrentPage = 1;
                if (CurrentPage > TotalPages && TotalPages > 0) CurrentPage = TotalPages;

                // Get paginated results
                Reviews = query
                    .OrderByDescending(r => r.CreatedAt)
                    .Skip((CurrentPage - 1) * PageSize)
                    .Take(PageSize)
                    .ToList();
            }
            catch (Exception ex)
            {
                // Log error and provide fallback
                Console.WriteLine($"Error loading reviews: {ex.Message}");

                // Set default values to prevent errors
                TotalReviews = 0;
                AverageRating = 0;
                PositiveReviews = 0;
                ThisMonthReviews = 0;
                TotalPages = 0;
                Reviews = new List<Review>();
            }
        }

        public async Task<IActionResult> OnDeleteAsync(int id)
        {
            try
            {
                var review = await _context.Reviews.FindAsync(id);
                if (review != null)
                {
                    _context.Reviews.Remove(review);
                    await _context.SaveChangesAsync();
                    return new JsonResult(new { success = true });
                }
                return new JsonResult(new { success = false, message = "Review not found" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = ex.Message });
            }
        }
    }
}