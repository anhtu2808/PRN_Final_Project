namespace LaptopRentalManagement.Model.DTOs.Request;

public class ChatRoomFilter
{
    public string? Status { get; set; } // Open, InProgress, Resolved, Closed
    public int? CustomerId { get; set; }
    public int? StaffId { get; set; }
    public string? Subject { get; set; }
    public DateTime? CreatedFrom { get; set; }
    public DateTime? CreatedTo { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
} 