using LaptopRentalManagement.BLL.Interfaces;
using LaptopRentalManagement.DAL.Entities;
using LaptopRentalManagement.DAL.Interfaces;
using LaptopRentalManagement.Model.DTOs.Chat;
using System.Linq;

namespace LaptopRentalManagement.BLL.Services;

public class TicketChatService : ITicketChatService
{
    private readonly ITicketChatMessageRepository _messageRepo;
    private readonly IAccountRepository _accountRepo;

    public TicketChatService(ITicketChatMessageRepository messageRepo, IAccountRepository accountRepo)
    {
        _messageRepo = messageRepo;
        _accountRepo = accountRepo;
    }

    public async Task<IEnumerable<TicketChatMessageDto>> GetMessagesAsync(int ticketId)
    {
        var messages = await _messageRepo.GetByTicketIdAsync(ticketId);
        return messages.Select(m => new TicketChatMessageDto
        {
            TicketChatMessageId = m.TicketChatMessageId,
            TicketId = m.TicketId,
            SenderId = m.SenderId,
            SenderName = m.Sender?.Name ?? "Unknown",
            Content = m.Content,
            SentAt = m.SentAt
        });
    }

    public async Task<TicketChatMessageDto> SendMessageAsync(int ticketId, int senderId, string content)
    {
        var message = new TicketChatMessage
        {
            TicketId = ticketId,
            SenderId = senderId,
            Content = content,
            SentAt = DateTime.UtcNow
        };
        var created = await _messageRepo.CreateAsync(message);
        var sender = await _accountRepo.GetByIdAsync(senderId);
        return new TicketChatMessageDto
        {
            TicketChatMessageId = created.TicketChatMessageId,
            TicketId = created.TicketId,
            SenderId = senderId,
            SenderName = sender?.Name ?? "Unknown",
            Content = created.Content,
            SentAt = created.SentAt
        };
    }
}
