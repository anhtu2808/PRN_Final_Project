using LaptopRentalManagement.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace LaptopRentalManagement.Hubs;

[Authorize]
public class TicketChatHub : Hub
{
    private readonly ITicketChatService _chatService;

    public TicketChatHub(ITicketChatService chatService)
    {
        _chatService = chatService;
    }

    public async Task JoinTicket(int ticketId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"Ticket_{ticketId}");
    }

    public async Task LeaveTicket(int ticketId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Ticket_{ticketId}");
    }

    public async Task SendMessage(int ticketId, string content)
    {
        var userIdStr = Context.User?.FindFirst("AccountId")?.Value;
        if (!int.TryParse(userIdStr, out var userId)) return;

        var message = await _chatService.SendMessageAsync(ticketId, userId, content);
        await Clients.Group($"Ticket_{ticketId}").SendAsync("ReceiveMessage", message);
    }
}
