namespace LaptopRentalManagement.Model.DTOs.Response.Chat;

public class ChatMessageResponse
{
    public int ChatMessageId { get; set; }
    public int ChatRoomId { get; set; }
    public int SenderId { get; set; }
    public string SenderName { get; set; } = null!;
    public string SenderRole { get; set; } = null!;
    public string Content { get; set; } = null!;
    public string MessageType { get; set; } = null!;
    public DateTime SentAt { get; set; }
    public bool IsRead { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsOwnMessage { get; set; }
} 