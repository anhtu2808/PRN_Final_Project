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
	public class AccountRepository : IAccountRepository
	{
		private readonly LaptopRentalDbContext _context;

		public AccountRepository(LaptopRentalDbContext context)
		{
			_context = context;
		}
		public async Task<Account?> GetByIdAsync(int id)
		{
			return await _context.Accounts
					.FirstOrDefaultAsync(acc => acc.AccountId == id);
		}
	}
}
