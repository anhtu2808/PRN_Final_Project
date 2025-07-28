using LaptopRentalManagement.DAL.Context;
using LaptopRentalManagement.DAL.Entities;
using LaptopRentalManagement.DAL.Interfaces;
using LaptopRentalManagement.Model.DTOs.Request;
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

        public async Task<Slot> Update(Slot slot)
        {
            _context.Slots.Update(slot);
            await _context.SaveChangesAsync();
            return slot;
        }

        public async Task<Slot?> GetById(int id)
        {
            return await _context.Slots
                .FirstOrDefaultAsync(s => s.SlotId == id);
        }

        public async Task<IList<Slot>> GetByLaptopId(int laptopId)
        {
            return await _context.Slots
                .Where(s => s.LaptopId == laptopId)
                .ToListAsync();
        }

        public async Task<IList<Slot>> GetByOrderId(int orderId)
        {
            return await _context.Slots
                .Where(s => s.OrderId == orderId)
                .ToListAsync();
        }

        public async Task<List<Slot>> GetByIdsAsync(List<int> slotIds)
        {
            return await _context.Slots
                .Where(s => EF.Constant(slotIds).Contains(s.SlotId))
                .ToListAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var slot = await _context.Slots.FindAsync(id);
            if (slot != null)
            {
                _context.Slots.Remove(slot);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IList<Slot>> GetAllAsync(SlotFilter slotFilter)
        {
            var query = _context.Set<Slot>().AsQueryable();

            if (!string.IsNullOrWhiteSpace(slotFilter.Status))
                query = query.Where(o => o.Status == slotFilter.Status);

            if (slotFilter.LaptopId.HasValue)
                query = query.Where(o => o.LaptopId == slotFilter.LaptopId.Value);

            if (slotFilter.Month.HasValue)
                query = query.Where(o => o.SlotDate.Month == slotFilter.Month.Value);

            if (slotFilter.Year.HasValue)
                query = query.Where(o => o.SlotDate.Year == slotFilter.Year.Value);

            return await query
                .Include(o => o.Laptop)
                .Include(o => o.Order)
                .ToListAsync();
        }
    }
}