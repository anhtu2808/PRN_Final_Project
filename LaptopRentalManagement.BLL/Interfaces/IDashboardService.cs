using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaptopRentalManagement.Model.DTOs.Response.Dashboard;

namespace LaptopRentalManagement.BLL.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardDataRespone> GetDashboardDataAsync();
        Task<DashboardStats> GetStatsAsync();
        Task<List<RecentOrder>> GetRecentOrdersAsync(int count = 10);
        Task<List<TopLaptop>> GetTopLaptopsAsync(int count = 5);
        Task<List<ChartData>> GetMonthlyRevenueAsync(int months = 12);
        Task<List<ChartData>> GetOrderStatusChartAsync();
    }
}
