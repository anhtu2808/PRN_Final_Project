using LaptopRentalManagement.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopRentalManagement.DAL.Interfaces
{
    public interface IOrderLogImgRepository
    {

        Task CreateAsync(OrderLogImg img);
        Task<IList<string>?> GetByLogId(int logId);
    }
}
