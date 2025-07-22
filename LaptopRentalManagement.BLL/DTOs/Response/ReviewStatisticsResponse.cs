using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopRentalManagement.BLL.DTOs.Response
{
    public class ReviewStatisticsResponse
    {
        public int TotalReviews { get; set; }
        public double AverageRating { get; set; }
        public int PositiveReviews { get; set; }
        public int ThisMonthReviews { get; set; }
    }
}
