using LaptopRentalManagement.DAL.LaptopRentalManagement.DAL.Context;
using LaptopRentalManagement.DAL.LaptopRentalManagement.DAL.Entities;
using LaptopRentalManagement.DAL.LaptopRentalManagement.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopRentalManagement.DAL.LaptopRentalManagement.DAL.Repositories
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
                           order.Status == "Completed" &&
                           order.EndDate < DateOnly.FromDateTime(DateTime.Now);

            // Kiểm tra chưa có review nào cho order này
            var existingReview = await _context.Reviews
                .AnyAsync(r => r.OrderId == orderId);

            return canReview && !existingReview;
        }
    }
}
