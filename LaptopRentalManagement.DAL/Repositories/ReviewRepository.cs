using LaptopRentalManagement.DAL.Context;
using LaptopRentalManagement.DAL.Entities;
using LaptopRentalManagement.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LaptopRentalManagement.DAL.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly LaptopRentalDbContext _context;

        public ReviewRepository(LaptopRentalDbContext context)
        {
            _context = context;
        }

        public async Task<Review?> GetByIdAsync(int reviewId)
        {
            return await _context.Reviews
                .Include(r => r.Order)
                    .ThenInclude(o => o.Laptop)
                .Include(r => r.Rater)
                .FirstOrDefaultAsync(r => r.ReviewId == reviewId);
        }

        public async Task<List<Review>> GetByLaptopIdAsync(int laptopId)
        {
            return await _context.Reviews
                .Include(r => r.Rater)
                .Include(r => r.Order)
                    .ThenInclude(o => o.Laptop)
                .Where(r => r.Order.LaptopId == laptopId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Review>> GetByRaterIdAsync(int raterId)
        {
            return await _context.Reviews
                .Include(r => r.Order)
                    .ThenInclude(o => o.Laptop)
                .Where(r => r.RaterId == raterId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<Review?> GetByOrderIdAsync(int orderId)
        {
            return await _context.Reviews
                .Include(r => r.Rater)
                .Include(r => r.Order)
                    .ThenInclude(o => o.Laptop)
                .FirstOrDefaultAsync(r => r.OrderId == orderId);
        }

        public async Task<Review> CreateAsync(Review review)
        {
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            return review;
        }

        public async Task<Review> UpdateAsync(Review review)
        {
            _context.Reviews.Update(review);
            await _context.SaveChangesAsync();
            return review;
        }

        public async Task DeleteAsync(int reviewId)
        {
            var review = await _context.Reviews.FindAsync(reviewId);

            if (review != null)
            {
                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<double> GetAverageRatingByLaptopIdAsync(int laptopId)
        {
            var reviews = await _context.Reviews
                .Where(r => r.Order.LaptopId == laptopId)
                .ToListAsync();

            return reviews.Any() ? reviews.Average(r => (double)r.Rating) : 0;
        }

        public async Task<int> GetTotalReviewsByLaptopIdAsync(int laptopId)
        {
            return await _context.Reviews
                .CountAsync(r => r.Order.LaptopId == laptopId);
        }

        public async Task<Dictionary<int, int>> GetRatingDistributionByLaptopIdAsync(int laptopId)
        {
            var distribution = await _context.Reviews
                .Where(r => r.Order.LaptopId == laptopId)
                .GroupBy(r => r.Rating)
                .Select(g => new { Rating = (int)g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Rating, x => x.Count);

            // Đảm bảo có đủ các rating từ 1-5
            for (int i = 1; i <= 5; i++)
            {
                if (!distribution.ContainsKey(i))
                    distribution[i] = 0;
            }

            return distribution;
        }

        public async Task<bool> CanUserReviewOrderAsync(int orderId, int userId)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null) return false;

            // Chỉ cho phép renter review và order phải đã hoàn thành
            var canReview = order.RenterId == userId &&
                           order.Status == "Completed";

            // Kiểm tra chưa có review nào cho order này
            var existingReview = await _context.Reviews
                .AnyAsync(r => r.OrderId == orderId);

            return canReview && !existingReview;
        }

        public async Task<int?> GetEligibleOrderIdForReviewAsync(int laptopId, int userId)
        {
            var order = await _context.Orders
                .Include(o => o.Reviews)
                .Where(o => o.LaptopId == laptopId &&
                            o.RenterId == userId &&
                            o.Status == "Completed")
                .FirstOrDefaultAsync(o => !o.Reviews.Any());

            return order?.OrderId;
        }

        public async Task<(IEnumerable<Review> Reviews, int TotalCount)> GetFilteredAsync(
    string? searchTerm, int? selectedRating, DateTime? dateFrom, DateTime? dateTo, int currentPage, int pageSize)
        {
            var query = _context.Reviews
                .AsNoTracking()
                .Include(r => r.Rater)
                .Include(r => r.Order).ThenInclude(o => o.Laptop)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(r => r.Comment != null && EF.Functions.Like(r.Comment, $"%{searchTerm}%"));
            }
            if (selectedRating.HasValue)
            {
                query = query.Where(r => r.Rating == selectedRating.Value);
            }
            if (dateFrom.HasValue)
            {
                query = query.Where(r => r.CreatedAt >= dateFrom.Value);
            }
            if (dateTo.HasValue)
            {
                var endDate = dateTo.Value.Date.AddDays(1);
                query = query.Where(r => r.CreatedAt < endDate);
            }

            var totalCount = await query.CountAsync();

            var reviews = await query
                .OrderByDescending(r => r.CreatedAt)
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (reviews, totalCount);
        }

        public async Task<int> CountAllAsync()
        {
            return await _context.Reviews.CountAsync();
        }

        public async Task<double> GetAverageRatingAllAsync()
        {
            return await _context.Reviews.AnyAsync()
                ? await _context.Reviews.AverageAsync(r => r.Rating)
                : 0;
        }

        public async Task<int> CountPositiveReviewsAsync()
        {
            return await _context.Reviews.CountAsync(r => r.Rating >= 4);
        }

        public async Task<int> CountThisMonthReviewsAsync()
        {
            return await _context.Reviews.CountAsync(r => r.CreatedAt >= DateTime.UtcNow.AddDays(-30));
        }
    }
}
