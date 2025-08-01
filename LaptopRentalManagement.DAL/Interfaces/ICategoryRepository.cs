using LaptopRentalManagement.DAL.Entities;

namespace LaptopRentalManagement.DAL.Interfaces;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAllAsync();
    Task<List<Category>> GetByIds(List<int> ids);
    Task<Category?> GetByIdAsync(int id);
    Task<Category?> GetByNameAsync(string name);
    Task<Category> CreateAsync(Category category);
    Task<Category> UpdateAsync(Category category);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsByNameAsync(string name, int? excludeId = null);
    Task<int> GetLaptopCountByCategoryAsync(int categoryId);
}