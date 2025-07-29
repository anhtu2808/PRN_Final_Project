using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using LaptopRentalManagement.BLL.Interfaces;
using LaptopRentalManagement.BLL.DTOs.Request;
using LaptopRentalManagement.BLL.DTOs.Response;
using LaptopRentalManagement.DAL.Entities;

namespace LaptopRentalManagement.Pages.Manage.Tickets;

// [Authorize(Roles = "Admin,Staff")]
public class IndexModel : PageModel
{
    private readonly ITicketService _ticketService;

    public IndexModel(ITicketService ticketService)
    {
        _ticketService = ticketService;
    }

    [BindProperty]
    public UpdateTicketRequest UpdateForm { get; set; } = new();

    public IList<TicketResponse> Tickets { get; set; } = new List<TicketResponse>();

    public async Task OnGetAsync()
    {
        try
        {
            Tickets = await _ticketService.GetAllAsync();
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error loading tickets: {ex.Message}";
            Tickets = new List<TicketResponse>();
        }
    }

    public async Task<IActionResult> OnPostUpdateAsync()
    {
        if (!ModelState.IsValid)
        {
            TempData["ErrorMessage"] = "Invalid data provided.";
            return RedirectToPage();
        }

        try
        {
            await _ticketService.UpdateAsync(UpdateForm);
            TempData["SuccessMessage"] = "Ticket updated successfully!";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error updating ticket: {ex.Message}";
        }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        try
        {
            await _ticketService.DeleteAsync(id);
            TempData["SuccessMessage"] = "Ticket deleted successfully!";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error deleting ticket: {ex.Message}";
        }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnGetTicketAsync(int id)
    {
        try
        {
            var ticket = await _ticketService.GetByIdAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            return new JsonResult(new
            {
                ticketId = ticket.TicketId,
                content = ticket.Content,
                status = ticket.Status.ToString(),
                response = ticket.Response,
                createdAt = ticket.CreatedAt.ToString("yyyy-MM-dd HH:mm"),
                respondedAt = ticket.RespondedAt?.ToString("yyyy-MM-dd HH:mm"),
                renterId = ticket.Renter?.AccountId,
                renterName = ticket.Renter?.Name,
                ownerId = ticket.Owner?.AccountId,
                ownerName = ticket.Owner?.Name,
                orderId = ticket.Order?.OrderId
            });
        }
        catch (Exception ex)
        {
            return BadRequest($"Error loading ticket: {ex.Message}");
        }
    }
} 