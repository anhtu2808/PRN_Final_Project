using AutoMapper;
using LaptopRentalManagement.BLL.DTOs.Request;
using LaptopRentalManagement.BLL.DTOs.Response;
using LaptopRentalManagement.BLL.Interfaces;
using LaptopRentalManagement.DAL.Entities;
using LaptopRentalManagement.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LaptopRentalManagement.BLL.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;

        public FeedbackService(IReviewRepository reviewRepository, IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
        }

        // --- CÁC PHƯƠNG THỨC GỐC CỦA BẠN (KHÔNG THAY ĐỔI) ---

        public async Task<ReviewResponse?> GetReviewByIdAsync(int reviewId)
        {
            var review = await _reviewRepository.GetByIdAsync(reviewId);
            return _mapper.Map<ReviewResponse>(review);
        }

        public async Task<LaptopReviewSummaryResponse> GetLaptopReviewsAsync(int laptopId)
        {
            var reviews = await _reviewRepository.GetByLaptopIdAsync(laptopId);
            var averageRating = await _reviewRepository.GetAverageRatingByLaptopIdAsync(laptopId);
            var totalReviews = await _reviewRepository.GetTotalReviewsByLaptopIdAsync(laptopId);
            var ratingDistribution = await _reviewRepository.GetRatingDistributionByLaptopIdAsync(laptopId);

            return new LaptopReviewSummaryResponse
            {
                LaptopId = laptopId,
                AverageRating = Math.Round(averageRating, 1),
                TotalReviews = totalReviews,
                Reviews = _mapper.Map<List<ReviewResponse>>(reviews),
                RatingDistribution = ratingDistribution
            };
        }

        public async Task<List<ReviewResponse>> GetUserReviewsAsync(int userId)
        {
            var reviews = await _reviewRepository.GetByRaterIdAsync(userId);
            return _mapper.Map<List<ReviewResponse>>(reviews);
        }

        public async Task<ReviewResponse> CreateReviewAsync(CreateReviewRequest request, int userId)
        {
            var canReview = await _reviewRepository.CanUserReviewOrderAsync(request.OrderId, userId);
            if (!canReview)
            {
                throw new UnauthorizedAccessException("You cannot review this order.");
            }

            var review = _mapper.Map<Review>(request);
            review.RaterId = userId;
            review.CreatedAt = DateTime.UtcNow;

            var createdReview = await _reviewRepository.CreateAsync(review);
            return _mapper.Map<ReviewResponse>(createdReview);
        }

        public async Task<ReviewResponse> UpdateReviewAsync(int reviewId, UpdateReviewRequest request, int userId)
        {
            var review = await _reviewRepository.GetByIdAsync(reviewId);
            if (review == null) throw new KeyNotFoundException("Review not found.");
            if (review.RaterId != userId) throw new UnauthorizedAccessException("You can only update your own reviews.");

            _mapper.Map(request, review);
            var updatedReview = await _reviewRepository.UpdateAsync(review);
            return _mapper.Map<ReviewResponse>(updatedReview);
        }

        public async Task DeleteReviewAsync(int reviewId, int userId)
        {
            var review = await _reviewRepository.GetByIdAsync(reviewId);
            if (review == null) return;
            if (review.RaterId != userId) throw new UnauthorizedAccessException("You can only delete your own reviews.");

            await _reviewRepository.DeleteAsync(reviewId);
        }

        public async Task<bool> CanUserReviewOrderAsync(int orderId, int userId)
        {
            return await _reviewRepository.CanUserReviewOrderAsync(orderId, userId);
        }

        // --- TRIỂN KHAI ĐẦY ĐỦ CÁC PHƯƠNG THỨC MỚI CHO TRANG QUẢN TRỊ ---

        public async Task<(IEnumerable<ReviewResponse> Reviews, int TotalPages)> GetFilteredReviewsAsync(string? searchTerm, int? selectedRating, DateTime? dateFrom, DateTime? dateTo, int currentPage, int pageSize)
        {
            var (reviews, totalCount) = await _reviewRepository.GetFilteredAsync(searchTerm, selectedRating, dateFrom, dateTo, currentPage, pageSize);

            var reviewResponses = _mapper.Map<IEnumerable<ReviewResponse>>(reviews);
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            return (reviewResponses, totalPages);
        }

        public async Task<ReviewStatisticsResponse> GetReviewStatisticsAsync()
        {
            // Thực thi tuần tự, mỗi lần chỉ một truy vấn
            var totalReviews = await _reviewRepository.CountAllAsync();
            var averageRating = await _reviewRepository.GetAverageRatingAllAsync();
            var positiveReviews = await _reviewRepository.CountPositiveReviewsAsync();
            var thisMonthReviews = await _reviewRepository.CountThisMonthReviewsAsync();

            return new ReviewStatisticsResponse
            {
                TotalReviews = totalReviews,
                AverageRating = Math.Round(averageRating, 1),
                PositiveReviews = positiveReviews,
                ThisMonthReviews = thisMonthReviews
            };
        }
        public async Task<ReviewResponse> AdminUpdateReviewAsync(int reviewId, UpdateReviewRequest request)
        {
            var review = await _reviewRepository.GetByIdAsync(reviewId);
            if (review == null)
            {
                throw new KeyNotFoundException("Review not found.");
            }

            _mapper.Map(request, review);
            var updatedReview = await _reviewRepository.UpdateAsync(review);
            return _mapper.Map<ReviewResponse>(updatedReview);
        }

        public async Task AdminDeleteReviewAsync(int reviewId)
        {
            await _reviewRepository.DeleteAsync(reviewId);
        }
    }
}