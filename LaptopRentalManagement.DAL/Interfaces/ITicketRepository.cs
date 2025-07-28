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
	}
}
