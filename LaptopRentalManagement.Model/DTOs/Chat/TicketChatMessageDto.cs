namespace LaptopRentalManagement.Model.DTOs.Chat;

public class TicketChatMessageDto
{
    public int TicketChatMessageId { get; set; }
    public int TicketId { get; set; }
    public int SenderId { get; set; }
    public string SenderName { get; set; } = null!;
    public string Content { get; set; } = null!;
    public DateTime SentAt { get; set; }
}
