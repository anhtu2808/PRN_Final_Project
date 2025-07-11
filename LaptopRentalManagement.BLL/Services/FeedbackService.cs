using AutoMapper;
using LaptopRentalManagement.BLL.DTOs.Request;
using LaptopRentalManagement.BLL.DTOs.Response;
using LaptopRentalManagement.BLL.Interfaces;
using LaptopRentalManagement.DAL.LaptopRentalManagement.DAL.Entities;
using LaptopRentalManagement.DAL.LaptopRentalManagement.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public async Task<ReviewResponse?> GetReviewByIdAsync(int reviewId)
        {
            var review = await _reviewRepository.GetByIdAsync(reviewId);
            return review != null ? _mapper.Map<ReviewResponse>(review) : null;
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
            // Kiểm tra quyền review
            var canReview = await _reviewRepository.CanUserReviewOrderAsync(request.OrderId, userId);
            if (!canReview)
            {
                throw new UnauthorizedAccessException("You cannot review this order.");
            }

            var review = new Review
            {
                OrderId = request.OrderId,
                RaterId = userId,
                Rating = (byte)request.Rating,
                Comment = request.Comment,
                CreatedAt = DateTime.UtcNow
            };

            var createdReview = await _reviewRepository.CreateAsync(review);
            var result = await _reviewRepository.GetByIdAsync(createdReview.ReviewId);

            return _mapper.Map<ReviewResponse>(result);
        }

        public async Task<ReviewResponse> UpdateReviewAsync(int reviewId, UpdateReviewRequest request, int userId)
        {
            var review = await _reviewRepository.GetByIdAsync(reviewId);
            if (review == null)
            {
                throw new KeyNotFoundException("Review not found.");
            }

            if (review.RaterId != userId)
            {
                throw new UnauthorizedAccessException("You can only update your own reviews.");
            }

            review.Rating = (byte)request.Rating;
            review.Comment = request.Comment;

            var updatedReview = await _reviewRepository.UpdateAsync(review);
            return _mapper.Map<ReviewResponse>(updatedReview);
        }

        public async Task DeleteReviewAsync(int reviewId, int userId)
        {
            var review = await _reviewRepository.GetByIdAsync(reviewId);
            if (review == null)
            {
                throw new KeyNotFoundException("Review not found.");
            }

            if (review.RaterId != userId)
            {
                throw new UnauthorizedAccessException("You can only delete your own reviews.");
            }

            await _reviewRepository.DeleteAsync(reviewId);
        }

        public async Task<bool> CanUserReviewOrderAsync(int orderId, int userId)
        {
            return await _reviewRepository.CanUserReviewOrderAsync(orderId, userId);
        }
    }
}
