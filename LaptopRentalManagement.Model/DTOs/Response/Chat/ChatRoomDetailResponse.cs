namespace LaptopRentalManagement.Model.DTOs.Response.Chat;

public class ChatRoomDetailResponse
{
    public ChatRoomResponse ChatRoom { get; set; } = null!;
    public List<ChatMessageResponse> Messages { get; set; } = new List<ChatMessageResponse>();
    public int TotalMessages { get; set; }
    public bool HasMoreMessages { get; set; }
} 