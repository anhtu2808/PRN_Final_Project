using LaptopRentalManagement.BLL.Interfaces;
using LaptopRentalManagement.BLL.DTOs.Response;
using LaptopRentalManagement.Model.DTOs.Chat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace LaptopRentalManagement.Pages.Chat;

[Authorize(Roles = "Customer")]
public class IndexModel : PageModel
{
    private readonly ITicketService _ticketService;
    private readonly ITicketChatService _chatService;

    public IndexModel(ITicketService ticketService, ITicketChatService chatService)
    {
        _ticketService = ticketService;
        _chatService = chatService;
    }

    public List<TicketResponse> Tickets { get; set; } = new();
    public List<TicketChatMessageDto> Messages { get; set; } = new();
    public int? SelectedTicketId { get; set; }
    public int CurrentUserId { get; set; }

    public async Task<IActionResult> OnGetAsync(int? ticketId)
    {
        if(!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("AccountId"), out var userId))
        {
            return RedirectToPage("/Account/Login");
        }
        CurrentUserId = userId;
        Tickets = (await _ticketService.GetByAccountIdAsync(userId)).ToList();
        SelectedTicketId = ticketId;
        if (ticketId.HasValue)
        {
            Messages = (await _chatService.GetMessagesAsync(ticketId.Value)).ToList();
        }
        return Page();
    }
}
