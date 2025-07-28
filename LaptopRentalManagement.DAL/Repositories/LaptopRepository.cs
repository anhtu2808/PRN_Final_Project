using LaptopRentalManagement.DAL.Context;
using LaptopRentalManagement.DAL.Entities;
using LaptopRentalManagement.DAL.Interfaces;
using LaptopRentalManagement.Model.DTOs.Request;
using Microsoft.EntityFrameworkCore;

namespace LaptopRentalManagement.DAL.Repositories;

public class LaptopRepository : ILaptopRepository
{
    private readonly LaptopRentalDbContext _context;

    public LaptopRepository(LaptopRentalDbContext context)
    {
        _context = context;
    }

    public async Task<List<Laptop>> GetAllAsync(LaptopFilter filter)
    {
        var query = _context.Laptops
            .Include(l => l.Brand)
            .Include(l => l.Categories)
            .Include(l => l.Account)
            .AsQueryable();
      

        if (filter.CategoryIds?.Count > 0)
        	query = query.Where(l => l.Categories.Any(c => filter.CategoryIds.Contains(c.CategoryId)));
        if (filter.BrandId.HasValue)
            query = query.Where(l => l.BrandId == filter.BrandId.Value);

        if (filter.BrandIds != null && filter.BrandIds.Any())
            query = query.Where(l => filter.BrandIds.Contains(l.BrandId));

        // Các filter còn lại giữ nguyên
        if (filter.AccountId.HasValue)
            query = query.Where(l => l.AccountId == filter.AccountId.Value);

        if (!string.IsNullOrWhiteSpace(filter.Name))
            query = query.Where(l => EF.Functions.Like(l.Name, $"%{filter.Name}%"));

        if (!string.IsNullOrWhiteSpace(filter.Cpu))
            query = query.Where(l => EF.Functions.Like(l.Cpu, $"%{filter.Cpu}%"));

        if (filter.Ram.HasValue)
            query = query.Where(l => l.Ram == filter.Ram.Value);

        if (filter.Storage.HasValue)
            query = query.Where(l => l.Storage == filter.Storage.Value);

        if (filter.MinPrice.HasValue)
            query = query.Where(l => l.PricePerDay >= filter.MinPrice.Value);

        if (filter.MaxPrice.HasValue)
            query = query.Where(l => l.PricePerDay <= filter.MaxPrice.Value);

        if (!string.IsNullOrWhiteSpace(filter.Status))
            query = query.Where(l => l.Status == filter.Status);

        if (filter.CreatedFrom.HasValue)
            query = query.Where(l => l.CreatedAt >= filter.CreatedFrom.Value);

        if (filter.CreatedTo.HasValue)
            query = query.Where(l => l.CreatedAt <= filter.CreatedTo.Value);

        return await query
            .OrderByDescending(l => l.Status == "Available")
            .ToListAsync();
    }


    public async Task<Laptop?> GetByIdAsync(int id)
    {
        return await _context.Laptops
            .Include(l => l.Brand)
            .Include(l => l.Account)
            .Include(l => l.Categories)
            .FirstOrDefaultAsync(l => l.LaptopId == id);
    }

    public async Task<List<Laptop>> GetTopRentedLaptopsAsync(int top = 3)
    {
        return await _context.Laptops
            .AsNoTracking()
            .Include(l => l.Brand)
            .OrderByDescending(l => l.Orders.Count)
            .Take(top)
            .ToListAsync();
    }

    public async Task<Laptop> CreateAsync(Laptop laptop)
    {
        _context.Laptops.Add(laptop);
        await _context.SaveChangesAsync();
        return laptop;
    }

    public async Task<Laptop> UpdateAsync(Laptop laptop)
    {
        _context.Laptops.Update(laptop);
        await _context.SaveChangesAsync();
        return laptop;
    }


    public async Task DeleteAsync(int id)
    {
        var laptop = await _context.Laptops.FindAsync(id);
        if (laptop != null)
        {
            _context.Laptops.Remove(laptop);
            await _context.SaveChangesAsync();
        }
    }
}