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

        public async Task<Account> Login(String email, String password)
        {
            var acc = await _context.Accounts
                .FirstOrDefaultAsync(a => a.Email.ToLower() == email.ToLower() && a.PasswordHash == password);
            return acc;
        }

        public async Task<Account?> GetByIdAsync(int id)
        {
            return await _context.Accounts
                .FirstOrDefaultAsync(a => a.AccountId == id);
        }

        public async Task<IEnumerable<Account>> GetAllAsync()
        {
            return await _context.Accounts
                .OrderBy(a => a.Name)
                .ToListAsync();
        }

        public async Task<Account> CreateAsync(Account account)
        {
            account.CreatedAt = DateTime.UtcNow;
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
            return account;
        }

        public async Task<Account> UpdateAsync(Account account)
        {
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
            return account;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
                return false;
            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
