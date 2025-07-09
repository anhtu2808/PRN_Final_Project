namespace LaptopRentalManagement.BLL.DTOs.Response
{
    public class LaptopResponse
    {
        public int LaptopId { get; set; }
        public string Model { get; set; } = string.Empty;
        public string Specifications { get; set; } = string.Empty;
        public decimal PricePerDay { get; set; }
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        
        // Brand information
        public BrandResponse Brand { get; set; } = new BrandResponse();
        
        // Category information
        public CategoryResponse Category { get; set; } = new CategoryResponse();
        
        // Reviews summary
        public double AverageRating { get; set; }
        public int ReviewCount { get; set; }
    }

    public class BrandResponse
    {
        public int BrandId { get; set; }
        public string BrandName { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public class CategoryResponse
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public class LaptopDetailResponse
    {
        public int LaptopId { get; set; }
        public string Model { get; set; } = string.Empty;
        public string Specifications { get; set; } = string.Empty;
        public decimal PricePerDay { get; set; }
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        
        // Brand information
        public BrandResponse Brand { get; set; } = new BrandResponse();
        
        // Category information
        public CategoryResponse Category { get; set; } = new CategoryResponse();
        
        // Reviews
        public List<ReviewResponse> Reviews { get; set; } = new List<ReviewResponse>();
        public double AverageRating { get; set; }
        public int ReviewCount { get; set; }
        
        // Available slots
        public List<SlotResponse> AvailableSlots { get; set; } = new List<SlotResponse>();
    }

    public class ReviewResponse
    {
        public int ReviewId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime ReviewDate { get; set; }
        public string CustomerName { get; set; } = string.Empty;
    }

    public class SlotResponse
    {
        public int SlotId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsAvailable { get; set; }
    }
} 