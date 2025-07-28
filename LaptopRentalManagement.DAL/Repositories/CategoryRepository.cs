using LaptopRentalManagement.DAL.Context;
using LaptopRentalManagement.DAL.Entities;
using LaptopRentalManagement.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LaptopRentalManagement.DAL.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly LaptopRentalDbContext _context;

    public CategoryRepository(LaptopRentalDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _context.Categories
            .Include(c => c.Laptops)
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<List<Category>> GetByIds(List<int> ids)
    {
        return await _context.Categories
            .Include(c => c.Laptops)
            .Where(c => ids.Contains(c.CategoryId))
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        return await _context.Categories
            .Include(c => c.Laptops)
            .FirstOrDefaultAsync(c => c.CategoryId == id);
    }

    public async Task<Category?> GetByNameAsync(string name)
    {
        return await _context.Categories
            .Include(c => c.Laptops)
            .FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower());
    }

    public async Task<Category> CreateAsync(Category category)
    {
        category.CreatedAt = DateTime.UtcNow;
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task<Category> UpdateAsync(Category category)
    {
        _context.Categories.Update(category);
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
            return false;

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsByNameAsync(string name, int? excludeId = null)
    {
        var query = _context.Categories.Where(c => c.Name.ToLower() == name.ToLower());

        if (excludeId.HasValue)
            query = query.Where(c => c.CategoryId != excludeId.Value);

        return await query.AnyAsync();
    }

    public async Task<int> GetLaptopCountByCategoryAsync(int categoryId)
    {
        return await _context.Categories
            .Where(c => c.CategoryId == categoryId)
            .Select(c => c.Laptops.Count)
            .FirstOrDefaultAsync();
    }
}