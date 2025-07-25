using LaptopRentalManagement.BLL.DTOs.Request;
using LaptopRentalManagement.BLL.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopRentalManagement.BLL.Interfaces
{
    public interface IOrderLogService
    {

        Task<OrderLogResponse> CreateAsync(CreateOrderLogRequest request, List<string>? imgURL);
        Task<IList<OrderLogResponse>> GetByOrderIdAsync(int orderId);
    }
}
