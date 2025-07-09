using System.ComponentModel.DataAnnotations;

namespace LaptopRentalManagement.BLL.DTOs.Request
{
    public class CreateLaptopRequest
    {
        [Required(ErrorMessage = "Model is required")]
        [StringLength(100, ErrorMessage = "Model cannot exceed 100 characters")]
        public string Model { get; set; } = string.Empty;

        [Required(ErrorMessage = "Brand ID is required")]
        public int BrandId { get; set; }

        [Required(ErrorMessage = "Category ID is required")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Specifications are required")]
        [StringLength(1000, ErrorMessage = "Specifications cannot exceed 1000 characters")]
        public string Specifications { get; set; } = string.Empty;

        [Required(ErrorMessage = "Price per day is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price per day must be greater than 0")]
        public decimal PricePerDay { get; set; }

        public string? ImageUrl { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Availability status is required")]
        public bool IsAvailable { get; set; } = true;
    }

    public class UpdateLaptopRequest
    {
        [Required(ErrorMessage = "Model is required")]
        [StringLength(100, ErrorMessage = "Model cannot exceed 100 characters")]
        public string Model { get; set; } = string.Empty;

        [Required(ErrorMessage = "Brand ID is required")]
        public int BrandId { get; set; }

        [Required(ErrorMessage = "Category ID is required")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Specifications are required")]
        [StringLength(1000, ErrorMessage = "Specifications cannot exceed 1000 characters")]
        public string Specifications { get; set; } = string.Empty;

        [Required(ErrorMessage = "Price per day is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price per day must be greater than 0")]
        public decimal PricePerDay { get; set; }

        public string? ImageUrl { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Availability status is required")]
        public bool IsAvailable { get; set; }
    }

    public class LaptopSearchRequest
    {
        public string? SearchTerm { get; set; }
        public int? BrandId { get; set; }
        public int? CategoryId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public bool? IsAvailable { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SortBy { get; set; } = "Model";
        public string? SortOrder { get; set; } = "ASC";
    }
} 