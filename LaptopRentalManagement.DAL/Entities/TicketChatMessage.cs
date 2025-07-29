namespace LaptopRentalManagement.DAL.Entities;

public class TicketChatMessage
{
    public int TicketChatMessageId { get; set; }
    public int TicketId { get; set; }
    public int SenderId { get; set; }
    public string Content { get; set; } = null!;
    public DateTime SentAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual Ticket Ticket { get; set; } = null!;
    public virtual Account Sender { get; set; } = null!;
}
