namespace LaptopRentalManagement.Model.DTOs.Response.Chat;

public class ChatSummaryResponse
{
    public int TotalChatRooms { get; set; }
    public int OpenChatRooms { get; set; }
    public int InProgressChatRooms { get; set; }
    public int ResolvedChatRooms { get; set; }
    public int ClosedChatRooms { get; set; }
    public int UnassignedChatRooms { get; set; }
    public int TotalUnreadMessages { get; set; }
    public List<ChatRoomResponse> RecentChatRooms { get; set; } = new List<ChatRoomResponse>();
} 