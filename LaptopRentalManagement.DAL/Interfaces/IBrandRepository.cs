using LaptopRentalManagement.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopRentalManagement.DAL.Interfaces
{
    public interface IBrandRepository
    {
        Task<IEnumerable<Brand>> GetAllAsync();
        Task<Brand?> GetByIdAsync(int id);
        Task<Brand?> GetByIdWithLaptopsAsync(int id);
        Task<Brand> CreateAsync(Brand brand);
        Task<Brand> UpdateAsync(Brand brand);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsByNameAsync(string name, int? excludeId = null);
        Task<bool> ExistsAsync(int id);
        Task<int> GetLaptopCountAsync(int brandId);
        Task<IEnumerable<Brand>> SearchAsync(string searchTerm);
    }
}
