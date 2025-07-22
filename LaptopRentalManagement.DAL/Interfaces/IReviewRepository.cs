using LaptopRentalManagement.DAL.Entities;

namespace LaptopRentalManagement.DAL.Interfaces;

public interface IReviewRepository
{
    Task<Review?> GetByIdAsync(int reviewId);
    Task<List<Review>> GetByLaptopIdAsync(int laptopId);
    Task<List<Review>> GetByRaterIdAsync(int raterId);
    Task<Review?> GetByOrderIdAsync(int orderId);
    Task<Review> CreateAsync(Review review);
    Task<Review> UpdateAsync(Review review);
    Task DeleteAsync(int reviewId);
    Task<double> GetAverageRatingByLaptopIdAsync(int laptopId);
    Task<int> GetTotalReviewsByLaptopIdAsync(int laptopId);
    Task<Dictionary<int, int>> GetRatingDistributionByLaptopIdAsync(int laptopId);
    Task<bool> CanUserReviewOrderAsync(int orderId, int userId);

    Task<(IEnumerable<Review> Reviews, int TotalCount)> GetFilteredAsync(
    string? searchTerm, int? selectedRating, DateTime? dateFrom, DateTime? dateTo, int currentPage, int pageSize);

    Task<int> CountAllAsync();
    Task<double> GetAverageRatingAllAsync();
    Task<int> CountPositiveReviewsAsync();
    Task<int> CountThisMonthReviewsAsync();
}