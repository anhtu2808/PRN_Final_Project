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
    public class OrderLogImgRepository : IOrderLogImgRepository
    {
        private readonly LaptopRentalDbContext _context;

        public OrderLogImgRepository(LaptopRentalDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(OrderLogImg img)
        {
            _context.OrderLogImgs.Add(img);
            await _context.SaveChangesAsync();
        }

        public async Task<IList<string>?> GetByLogId(int logId)
        {
            var query = _context.OrderLogImgs
                .Where(ol => ol.OrderLogId == logId)
                .AsQueryable();

            return await query.Select(i => i.ImgUrl).ToListAsync();
        }
    }
}
