namespace LaptopRentalManagement.DAL.Entities;

public class ChatMessage
{
    public int ChatMessageId { get; set; }
    public int ChatRoomId { get; set; }
    public int SenderId { get; set; }
    public string Content { get; set; } = null!;
    public string MessageType { get; set; } = "Text"; // Text, Image, File
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
    public bool IsRead { get; set; } = false;
    public bool IsDeleted { get; set; } = false;

    // Navigation properties
    public virtual ChatRoom ChatRoom { get; set; } = null!;
    public virtual Account Sender { get; set; } = null!;
} 