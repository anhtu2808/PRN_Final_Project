namespace LaptopRentalManagement.Model.DTOs.Request;

public class SendMessageRequest
{
    public int ChatRoomId { get; set; }
    public string Content { get; set; } = null!;
    public string MessageType { get; set; } = "Text"; // Text, Image, File
} 