using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaptopRentalManagement.DAL.Entities;

namespace LaptopRentalManagement.DAL.Interfaces
{
    public interface IAccountRepository
    {
        Task<Account> Login(String email, String password);
        Task<Account?> GetByIdAsync(int id);
        Task<IEnumerable<Account>> GetAllAsync();
        Task<Account> CreateAsync(Account account);
        Task<Account> UpdateAsync(Account account);
        Task<bool> DeleteAsync(int id);
    }
}
