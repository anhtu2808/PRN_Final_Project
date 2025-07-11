using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopRentalManagement.BLL.DTOs.Response
{
    public class ReviewResponse
    {
        public int ReviewId { get; set; }
        public int OrderId { get; set; }
        public int RaterId { get; set; }
        public string RaterName { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }

        // Thông tin laptop được review
        public string LaptopName { get; set; } = string.Empty;
        public string? LaptopImageUrl { get; set; }
    }
}
