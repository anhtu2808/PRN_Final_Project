using LaptopRentalManagement.Model.DTOs.Chat;

namespace LaptopRentalManagement.BLL.Interfaces;

public interface ITicketChatService
{
    Task<IEnumerable<TicketChatMessageDto>> GetMessagesAsync(int ticketId);
    Task<TicketChatMessageDto> SendMessageAsync(int ticketId, int senderId, string content);
}
