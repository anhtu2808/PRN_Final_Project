using LaptopRentalManagement.DAL.Context;
using LaptopRentalManagement.DAL.Entities;
using LaptopRentalManagement.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopRentalManagement.DAL.Repositories
{
	public class TicketRepository : ITicketRepository
	{
		private readonly LaptopRentalDbContext _context;
		public TicketRepository(LaptopRentalDbContext context)
		{
			_context = context;
		}
		public async Task<Ticket> CreateAsync(Ticket ticket)
		{
			ticket.CreatedAt = DateTime.UtcNow;
			_context.Tickets.Add(ticket);
			await _context.SaveChangesAsync();
			return ticket;
		}

		public async Task<IList<Ticket>> GetAllByOrderIdAsync(int id)
		{
			return await _context.Tickets
				.Where(t => t.OrderId == id)
				.ToListAsync();
		}
	}
}
