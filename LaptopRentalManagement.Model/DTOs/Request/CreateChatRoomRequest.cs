namespace LaptopRentalManagement.Model.DTOs.Request;

public class CreateChatRoomRequest
{
    public int CustomerId { get; set; }
    public string Subject { get; set; } = null!;
} 