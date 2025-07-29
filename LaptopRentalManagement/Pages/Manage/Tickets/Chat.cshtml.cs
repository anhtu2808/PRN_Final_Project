using LaptopRentalManagement.BLL.Interfaces;
using LaptopRentalManagement.Model.DTOs.Chat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

namespace LaptopRentalManagement.Pages.Manage.Tickets;

[Authorize(Roles = "Admin,Staff")]
public class ChatModel : PageModel
{
    private readonly ITicketChatService _chatService;

    public ChatModel(ITicketChatService chatService)
    {
        _chatService = chatService;
    }

    public int TicketId { get; set; }
    public List<TicketChatMessageDto> Messages { get; set; } = new();

    public async Task OnGetAsync(int ticketId)
    {
        TicketId = ticketId;
        Messages = (await _chatService.GetMessagesAsync(ticketId)).ToList();
    }
}
