using LaptopRentalManagement.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopRentalManagement.DAL.Interfaces
{
    public interface ITicketRepository
    {
        Task<Ticket> CreateAsync(Ticket ticket);
        Task<IList<Ticket>> GetAllByOrderIdAsync(int id);
        Task<IList<Ticket>> GetByRenterIdAsync(int renterId);
        Task<IList<Ticket>> GetAllAsync();
		Task<Ticket?> GetByIdAsync(int id);
		Task<Ticket> UpdateAsync(Ticket ticket);
		Task DeleteAsync(int id);
	}
}
