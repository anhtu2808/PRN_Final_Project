namespace LaptopRentalManagement.Model.DTOs.Response.Chat;

public class ChatRoomResponse
{
    public int ChatRoomId { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = null!;
    public string CustomerEmail { get; set; } = null!;
    public int? StaffId { get; set; }
    public string? StaffName { get; set; }
    public string? StaffEmail { get; set; }
    public string Subject { get; set; } = null!;
    public string Status { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime? LastMessageAt { get; set; }
    public bool IsCustomerActive { get; set; }
    public bool IsStaffActive { get; set; }
    public int UnreadMessagesCount { get; set; }
    public string? LastMessage { get; set; }
} 