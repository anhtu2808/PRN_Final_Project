using LaptopRentalManagement.DAL.Context;
using LaptopRentalManagement.DAL.Entities;
using LaptopRentalManagement.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace LaptopRentalManagement.DAL.Repositories;

public class TicketChatMessageRepository : ITicketChatMessageRepository
{
    private readonly LaptopRentalDbContext _context;

    public TicketChatMessageRepository(LaptopRentalDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TicketChatMessage>> GetByTicketIdAsync(int ticketId)
    {
        return await _context.TicketChatMessages
            .Include(m => m.Sender)
            .Where(m => m.TicketId == ticketId)
            .OrderBy(m => m.SentAt)
            .ToListAsync();
    }

    public async Task<TicketChatMessage> CreateAsync(TicketChatMessage message)
    {
        _context.TicketChatMessages.Add(message);
        await _context.SaveChangesAsync();
        return message;
    }
}
