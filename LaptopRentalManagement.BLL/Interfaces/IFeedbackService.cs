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
    }
}
