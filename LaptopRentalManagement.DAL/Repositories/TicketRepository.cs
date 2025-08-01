﻿using LaptopRentalManagement.DAL.Context;
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

        public async Task<IList<Ticket>> GetByRenterIdAsync(int renterId)
        {
                return await _context.Tickets
                        .Include(t => t.Order)
                        .Include(t => t.Renter)
                        .Include(t => t.Owner)
                        .Where(t => t.RenterId == renterId)
                        .OrderByDescending(t => t.CreatedAt)
                        .ToListAsync();
        }

        public async Task<IList<Ticket>> GetByAccountIdAsync(int accountId)
        {
                return await _context.Tickets
                        .Include(t => t.Order)
                        .Include(t => t.Renter)
                        .Include(t => t.Owner)
                        .Where(t => t.RenterId == accountId || t.OwnerId == accountId)
                        .OrderByDescending(t => t.CreatedAt)
                        .ToListAsync();
        }

		public async Task<IList<Ticket>> GetAllAsync()
		{
			return await _context.Tickets
				.Include(t => t.Order)
				.Include(t => t.Renter)
				.Include(t => t.Owner)
				.OrderByDescending(t => t.CreatedAt)
				.ToListAsync();
		}

		public async Task<Ticket?> GetByIdAsync(int id)
		{
			return await _context.Tickets
				.Include(t => t.Order)
				.Include(t => t.Renter)
				.Include(t => t.Owner)
				.FirstOrDefaultAsync(t => t.TicketId == id);
		}

		public async Task<Ticket> UpdateAsync(Ticket ticket)
		{
			_context.Tickets.Update(ticket);
			await _context.SaveChangesAsync();
			return ticket;
		}

		public async Task DeleteAsync(int id)
		{
			var ticket = await _context.Tickets.FindAsync(id);
			if (ticket != null)
			{
				_context.Tickets.Remove(ticket);
				await _context.SaveChangesAsync();
			}
		}
	}
}
