using LaptopRentalManagement.DAL.Entities;
using LaptopRentalManagement.Model.DTOs.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopRentalManagement.DAL.Interfaces
{
	public interface IOrderRepository
	{
		Task<IList<Order>> GetAllAsync(OrderFilter orderFilter);
		Task<Order?> GetByIdAsync(int id);
		Task<Order> CreateAsync(Order order);
		Task<Order> UpdateAsync(Order order);
		Task DeleteAsync(int id);
		
		// ZaloPay methods
		Task<Order?> GetByZaloPayTransactionIdAsync(string transactionId);
		Task UpdateZaloPayTransactionIdAsync(int orderId, string transactionId);
	}
}
