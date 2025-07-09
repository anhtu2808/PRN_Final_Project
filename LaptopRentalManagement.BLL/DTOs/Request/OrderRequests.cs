using System.ComponentModel.DataAnnotations;

namespace LaptopRentalManagement.BLL.DTOs.Request
{
    public class CreateOrderRequest
    {
        [Required(ErrorMessage = "Laptop ID is required")]
        public int LaptopId { get; set; }

        [Required(ErrorMessage = "Start date is required")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required")]
        public DateTime EndDate { get; set; }

        public string? Notes { get; set; }
    }

    public class UpdateOrderStatusRequest
    {
        [Required(ErrorMessage = "Status is required")]
        public string Status { get; set; } = string.Empty;

        public string? AdminNotes { get; set; }
    }

    public class OrderSearchRequest
    {
        public int? AccountId { get; set; }
        public int? LaptopId { get; set; }
        public string? Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SortBy { get; set; } = "OrderDate";
        public string? SortOrder { get; set; } = "DESC";
    }

    public class ExtendOrderRequest
    {
        [Required(ErrorMessage = "New end date is required")]
        public DateTime NewEndDate { get; set; }

        public string? Reason { get; set; }
    }
} 