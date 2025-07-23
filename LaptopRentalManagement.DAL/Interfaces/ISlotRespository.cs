using LaptopRentalManagement.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopRentalManagement.DAL.Interfaces
{
	public interface ISlotRespository
	{
		Task<Slot> CreateAsync(Slot slot);
        Task<Slot> Update(Slot slot);
        Task<Slot?> GetById(int id);
        Task<IList<Slot>> GetByOrderId(int orderId);
        Task<IList<Slot>> GetByLaptopId(int laptopId);

		Task DeleteAsync(int id);
	}
}
