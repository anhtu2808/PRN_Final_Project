using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopRentalManagement.BLL.DTOs.Response
{
    public class LaptopReviewSummaryResponse
    {
        public int LaptopId { get; set; }
        public double AverageRating { get; set; }
        public int TotalReviews { get; set; }
        public List<ReviewResponse> Reviews { get; set; } = new();

        // Breakdown rating
        public Dictionary<int, int> RatingDistribution { get; set; } = new(); // Rating (1-5) -> Count
    }
}
