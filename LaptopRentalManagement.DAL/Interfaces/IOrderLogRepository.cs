using LaptopRentalManagement.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopRentalManagement.DAL.Interfaces
{
    public interface IOrderLogRepository
    {
        Task<OrderLog> CreateAsync(OrderLog log);
        Task<IList<OrderLog>> GetByOrderId(int orderId);

    }
}
