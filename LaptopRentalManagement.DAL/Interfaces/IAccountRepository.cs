using LaptopRentalManagement.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopRentalManagement.DAL.Interfaces
{
	public interface IAccountRepository
	{
		Task<Account?> GetByIdAsync(int id);
	}
}
