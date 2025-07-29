using LaptopRentalManagement.DAL.Entities;

namespace LaptopRentalManagement.DAL.Interfaces;

public interface ITicketChatMessageRepository
{
    Task<IEnumerable<TicketChatMessage>> GetByTicketIdAsync(int ticketId);
    Task<TicketChatMessage> CreateAsync(TicketChatMessage message);
}
