using LaptopRentalManagement.BLL.DTOs.Request;
using LaptopRentalManagement.BLL.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopRentalManagement.BLL.Interfaces
{
    public interface IFeedbackService
    {
        Task<ReviewResponse?> GetReviewByIdAsync(int reviewId);
        Task<LaptopReviewSummaryResponse> GetLaptopReviewsAsync(int laptopId);
        Task<List<ReviewResponse>> GetUserReviewsAsync(int userId);
        Task<ReviewResponse> CreateReviewAsync(CreateReviewRequest request, int userId);
        Task<ReviewResponse> UpdateReviewAsync(int reviewId, UpdateReviewRequest request, int userId);
        Task DeleteReviewAsync(int reviewId, int userId);
        Task<bool> CanUserReviewOrderAsync(int orderId, int userId);

        Task<int?> GetEligibleOrderIdForReviewAsync(int laptopId, int userId);

        Task<(IEnumerable<ReviewResponse> Reviews, int TotalPages)> GetFilteredReviewsAsync(
    string? searchTerm, int? selectedRating, DateTime? dateFrom, DateTime? dateTo, int currentPage, int pageSize);

        Task<ReviewStatisticsResponse> GetReviewStatisticsAsync();

        Task<ReviewResponse> AdminUpdateReviewAsync(int reviewId, UpdateReviewRequest request);

        Task AdminDeleteReviewAsync(int reviewId);
    }
}
