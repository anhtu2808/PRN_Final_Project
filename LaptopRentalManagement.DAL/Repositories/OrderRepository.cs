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
	public class OrderRepository : IOrderRepository
	{
		private readonly LaptopRentalDbContext _context;

		public OrderRepository(LaptopRentalDbContext context)
		{
			_context = context;
		}
		public async Task<Order> CreateAsync(Order order)
		{
			order.CreatedAt = DateTime.UtcNow;
			order.UpdatedAt = DateTime.UtcNow;
			order.Status = "PendingApproval";
			_context.Orders.Add(order);
			await _context.SaveChangesAsync();
			return order;
		}

		public Task DeleteAsync(int id)
		{
			throw new NotImplementedException();
		}

		public async Task<IList<Order>> GetAllAsync(OrderFilter orderFilter)
		{
			var query = _context.Set<Order>().AsQueryable();

			if (!string.IsNullOrWhiteSpace(orderFilter.Status))
				query = query.Where(o => o.Status == orderFilter.Status);

			if (orderFilter.TotalCharge.HasValue)
				query = query.Where(o => o.TotalCharge == orderFilter.TotalCharge.Value);

			if (orderFilter.StartDate.HasValue)
				query = query.Where(o => o.StartDate >= orderFilter.StartDate.Value);

			if (orderFilter.EndDate.HasValue)
				query = query.Where(o => o.EndDate <= orderFilter.EndDate.Value);

			if (orderFilter.OwnerId.HasValue)
				query = query.Where(o => o.OwnerId == orderFilter.OwnerId.Value);

			if (orderFilter.RenterId.HasValue)
				query = query.Where(o => o.RenterId == orderFilter.RenterId.Value);

			if (orderFilter.LaptopId.HasValue)
				query = query.Where(o => o.LaptopId == orderFilter.LaptopId.Value);

			return await query
				.Include(o => o.Laptop)
				.Include(o => o.Owner)
				.Include(o => o.Renter)
				.ToListAsync();
		}

		public async Task<Order?> GetByIdAsync(int id)
		{
			return await _context.Orders
				.Include(o => o.Laptop)
				.Include(o => o.Owner)
				.Include(o => o.Renter)
				.FirstOrDefaultAsync(o => o.OrderId == id);

        }

		public Task<Order> UpdateAsync(Order order)
		{
			throw new NotImplementedException();
		}
	}
}
