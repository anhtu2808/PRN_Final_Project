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
    public class BrandRepository : IBrandRepository
    {
        private readonly LaptopRentalDbContext _context;

        public BrandRepository(LaptopRentalDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Brand>> GetAllAsync()
        {
            return await _context.Brands
                .Include(b => b.Laptops)
                .OrderBy(b => b.Name)
                .ToListAsync();
        }

        public async Task<Brand?> GetByIdAsync(int id)
        {
            return await _context.Brands
                .FirstOrDefaultAsync(b => b.BrandId == id);
        }

        public async Task<Brand?> GetByIdWithLaptopsAsync(int id)
        {
            return await _context.Brands
                .Include(b => b.Laptops)
                .FirstOrDefaultAsync(b => b.BrandId == id);
        }

        public async Task<Brand> CreateAsync(Brand brand)
        {
            brand.CreatedAt = DateTime.UtcNow;
            brand.UpdatedAt = DateTime.UtcNow;

            _context.Brands.Add(brand);
            await _context.SaveChangesAsync();
            return brand;
        }

        public async Task<Brand> UpdateAsync(Brand brand)
        {
            brand.UpdatedAt = DateTime.UtcNow;

            _context.Brands.Update(brand);
            await _context.SaveChangesAsync();
            return brand;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var brand = await _context.Brands.FindAsync(id);
            if (brand == null)
                return false;

            _context.Brands.Remove(brand);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsByNameAsync(string name, int? excludeId = null)
        {
            var query = _context.Brands.Where(b => b.Name.ToLower() == name.ToLower());

            if (excludeId.HasValue)
            {
                query = query.Where(b => b.BrandId != excludeId.Value);
            }

            return await query.AnyAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Brands.AnyAsync(b => b.BrandId == id);
        }

        public async Task<int> GetLaptopCountAsync(int brandId)
        {
            return await _context.Laptops.CountAsync(l => l.BrandId == brandId);
        }

        public async Task<IEnumerable<Brand>> SearchAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllAsync();

            searchTerm = searchTerm.ToLower();
            return await _context.Brands
                .Where(b => b.Name.ToLower().Contains(searchTerm) ||
                           (b.Description != null && b.Description.ToLower().Contains(searchTerm)) ||
                           (b.Country != null && b.Country.ToLower().Contains(searchTerm)))
                .Include(b => b.Laptops)
                .OrderBy(b => b.Name)
                .ToListAsync();
        }
    }
}
