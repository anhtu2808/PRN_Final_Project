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
    public class OrderLogRepository : IOrderLogRepository
    {
        private readonly LaptopRentalDbContext _context;

        public OrderLogRepository(LaptopRentalDbContext context)
        {
            _context = context;
        }
        public async Task<OrderLog> CreateAsync(OrderLog log)
        {
            log.CreatedAt = DateTime.UtcNow;
            _context.OrderLogs.Add(log);
            await _context.SaveChangesAsync();
            return log;
        }

        public async Task<IList<OrderLog>> GetByOrderId(int orderId)
        {
            return await _context.OrderLogs
                .Include(ol => ol.Order)
                .Where(ol => ol.OrderId == orderId)
                .ToListAsync();
        }
    }
}
