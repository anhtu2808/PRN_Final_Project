using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using LaptopRentalManagement.DAL.LaptopRentalManagement.DAL.Entities;
using LaptopRentalManagement.DAL.LaptopRentalManagement.DAL.Context;
using System.Diagnostics;

namespace LaptopRentalManagement.Pages.FeedBack
{
    public class IndexModel : PageModel
    {
        private readonly LaptopRentalDbContext _context;
        private readonly IMemoryCache _cache;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(LaptopRentalDbContext context, IMemoryCache cache, ILogger<IndexModel> logger)
        {
            _context = context;
            _cache = cache;
            _logger = logger;
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
            var stopwatch = Stopwatch.StartNew();

            try
            {
                // Lấy thống kê từ cache hoặc tính toán nếu không có
                var stats = await GetReviewStatisticsAsync();
                TotalReviews = stats.TotalReviews;
                AverageRating = stats.AverageRating;
                PositiveReviews = stats.PositiveReviews;
                ThisMonthReviews = stats.ThisMonthReviews;

                // Xây dựng query từ đầu (không tải toàn bộ dữ liệu)
                var query = _context.Reviews
                    .Include(r => r.Rater)
                    .Include(r => r.Order)
                        .ThenInclude(o => o.Laptop)
                    .AsQueryable();

                // Áp dụng bộ lọc ở cấp độ database
                if (!string.IsNullOrEmpty(SearchTerm))
                {
                    query = query.Where(r => r.Comment != null && EF.Functions.Like(r.Comment, $"%{SearchTerm}%"));
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
                    var dateTo = DateTo.Value.AddDays(1);
                    query = query.Where(r => r.CreatedAt <= dateTo);
                }

                // Đếm tổng số bản ghi sau khi lọc (sử dụng COUNT ở DB)
                var totalFiltered = await query.CountAsync();
                TotalPages = (int)Math.Ceiling(totalFiltered / (double)PageSize);

                // Kiểm tra trang hiện tại hợp lệ
                if (CurrentPage < 1) CurrentPage = 1;
                if (CurrentPage > TotalPages && TotalPages > 0) CurrentPage = TotalPages;

                // Lấy kết quả phân trang trực tiếp từ database
                Reviews = await query
                    .OrderByDescending(r => r.CreatedAt)
                    .Skip((CurrentPage - 1) * PageSize)
                    .Take(PageSize)
                    .ToListAsync();

                _logger.LogInformation($"Reviews page loaded in {stopwatch.ElapsedMilliseconds}ms. Filtered count: {totalFiltered}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading reviews");

                // Thiết lập giá trị mặc định để tránh lỗi
                TotalReviews = 0;
                AverageRating = 0;
                PositiveReviews = 0;
                ThisMonthReviews = 0;
                TotalPages = 0;
                Reviews = new List<Review>();
            }
        }

        private async Task<ReviewStatistics> GetReviewStatisticsAsync()
        {
            // Thử lấy từ cache trước
            if (_cache.TryGetValue("ReviewStatistics", out ReviewStatistics stats))
            {
                return stats;
            }

            // Nếu không có trong cache, tính toán với các truy vấn tối ưu
            stats = new ReviewStatistics
            {
                TotalReviews = await _context.Reviews.CountAsync(),
                AverageRating = await _context.Reviews.AnyAsync() ? await _context.Reviews.AverageAsync(r => r.Rating) : 0,
                PositiveReviews = await _context.Reviews.CountAsync(r => r.Rating >= 4),
                ThisMonthReviews = await _context.Reviews.CountAsync(r => r.CreatedAt >= DateTime.Now.AddDays(-30))
            };

            // Lưu vào cache trong 15 phút
            _cache.Set("ReviewStatistics", stats, TimeSpan.FromMinutes(15));

            return stats;
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

                    // Xóa cache thống kê khi có thay đổi dữ liệu
                    _cache.Remove("ReviewStatistics");

                    return new JsonResult(new { success = true });
                }
                return new JsonResult(new { success = false, message = "Review not found" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting review {ReviewId}", id);
                return new JsonResult(new { success = false, message = ex.Message });
            }
        }
    }

    public class ReviewStatistics
    {
        public int TotalReviews { get; set; }
        public double AverageRating { get; set; }
        public int PositiveReviews { get; set; }
        public int ThisMonthReviews { get; set; }
    }
}