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
	public class SlotRepository : ISlotRespository
	{
		private readonly LaptopRentalDbContext _context;

		public SlotRepository(LaptopRentalDbContext context)
		{
			_context = context;
		}
		public async Task<Slot> CreateAsync(Slot slot)
		{
			slot.CreatedAt = DateTime.UtcNow;
			slot.UpdatedAt = DateTime.UtcNow;
			_context.Slots.Add(slot);
			await _context.SaveChangesAsync();
			return slot;
		}

        public async Task<IList<Slot>> GetByOrderIdAndStatus(int orderId, string status)
        {
			return await _context.Slots
				.Where(s => s.OrderId == orderId && s.Status == status)
				.ToListAsync();
        }
    }
}
