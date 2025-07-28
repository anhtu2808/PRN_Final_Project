namespace LaptopRentalManagement.DAL.Entities;

public class ChatRoom
{
    public int ChatRoomId { get; set; }
    public int CustomerId { get; set; }
    public int? StaffId { get; set; }
    public string Subject { get; set; } = null!;
    public string Status { get; set; } = "Open"; // Open, InProgress, Resolved, Closed
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastMessageAt { get; set; }
    public bool IsCustomerActive { get; set; } = true;
    public bool IsStaffActive { get; set; } = false;

    // Navigation properties
    public virtual Account Customer { get; set; } = null!;
    public virtual Account? Staff { get; set; }
    public virtual ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
} 