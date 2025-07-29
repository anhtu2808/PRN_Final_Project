using LaptopRentalManagement.BLL.Interfaces;
using LaptopRentalManagement.DAL;
using LaptopRentalManagement.Model.DTOs.Response.Dashboard;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using LaptopRentalManagement.DAL.Context;

namespace LaptopRentalManagement.BLL.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly LaptopRentalDbContext _context;

        public DashboardService(LaptopRentalDbContext context)
        {
            _context = context;
        }

        // Phương thức chính để tổng hợp dữ liệu
        public async Task<DashboardDataRespone> GetDashboardDataAsync()
        {
            // Thay vì dùng Task.WhenAll, chúng ta sẽ await từng cái một
            var stats = await GetStatsAsync();
            var recentOrders = await GetRecentOrdersAsync();
            var topLaptops = await GetTopLaptopsAsync();
            var monthlyRevenue = await GetMonthlyRevenueAsync();
            var orderStatus = await GetOrderStatusChartAsync();

            return new DashboardDataRespone
            {
                Stats = stats,
                RecentOrders = recentOrders,
                TopLaptops = topLaptops,
                MonthlyRevenue = monthlyRevenue,
                OrderStatus = orderStatus
            };
        }

        // Triển khai logic cho các chỉ số thống kê phức tạp
        public async Task<DashboardStats> GetStatsAsync()
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var startOfThisMonth = new DateOnly(today.Year, today.Month, 1);
            var startOfLastMonth = startOfThisMonth.AddMonths(-1);
            var endOfLastMonth = startOfThisMonth.AddDays(-1);
            var startOfWeek = today.AddDays(-(int)DateTime.UtcNow.DayOfWeek); // Giả sử tuần bắt đầu từ Chủ nhật

            // Doanh thu và đơn hàng tháng này và tháng trước
            var thisMonthOrders = await _context.Orders
                .Where(o => o.EndDate >= startOfThisMonth)
                .ToListAsync();

            var lastMonthOrders = await _context.Orders
                .Where(o => o.EndDate >= startOfLastMonth && o.EndDate <= endOfLastMonth)
                .ToListAsync();

            var thisMonthRevenue = thisMonthOrders.Where(o => o.Status == "Completed").Sum(o => o.RentalFee);
            var lastMonthRevenue = lastMonthOrders.Where(o => o.Status == "Completed").Sum(o => o.RentalFee);

            // Tính toán tăng trưởng
            double revenueGrowth = (lastMonthRevenue > 0)
                ? (double)((thisMonthRevenue - lastMonthRevenue) / lastMonthRevenue) * 100
                : (thisMonthRevenue > 0 ? 100.0 : 0.0);

            double orderGrowth = (lastMonthOrders.Count > 0)
                ? (double)((thisMonthOrders.Count - lastMonthOrders.Count) / (double)lastMonthOrders.Count) * 100
                : (thisMonthOrders.Count > 0 ? 100.0 : 0.0);

            // Các chỉ số khác
            var totalCustomers = await _context.Accounts.CountAsync(a => a.Role == "User"); // Giả sử role 'User' là khách hàng
            var activeRentals = await _context.Orders.CountAsync(o => o.Status == "Renting"); // Giả sử có status "Renting"
            var dueToday = await _context.Orders.CountAsync(o => o.EndDate == today && o.Status == "Renting");

            // Khách hàng mới trong tuần
            var newCustomersThisWeek = await _context.Accounts
                .Where(a => a.Role == "User" && a.CreatedAt >= startOfWeek.ToDateTime(TimeOnly.MinValue))
                .CountAsync();

            return new DashboardStats
            {
                TotalOrders = await _context.Orders.CountAsync(),
                Revenue = await _context.Orders.Where(o => o.Status == "Completed").SumAsync(o => o.RentalFee),
                ActiveRentals = activeRentals,
                TotalCustomers = totalCustomers,
                RevenueGrowth = Math.Round(revenueGrowth, 2),
                OrderGrowth = Math.Round(orderGrowth, 2),
                DueToday = dueToday,
                NewCustomersThisWeek = newCustomersThisWeek
            };
        }

        // Triển khai cho các đơn hàng gần đây
        public async Task<List<RecentOrder>> GetRecentOrdersAsync(int count = 10)
        {
            return await _context.Orders
                .OrderByDescending(o => o.CreatedAt)
                .Take(count)
                .Select(o => new RecentOrder
                {
                    Id = o.OrderId,
                    // CustomerName = o.Renter.Name, // Lấy tên từ Account renter
                    LaptopModel = o.Laptop.Name,  // Lấy tên từ Laptop
                    OrderDate = o.CreatedAt,
                    Status = o.Status,
                    Amount = o.RentalFee
                })
                .ToListAsync();
        }

        // Triển khai cho các laptop hàng đầu
        public async Task<List<TopLaptop>> GetTopLaptopsAsync(int count = 5)
        {
            return await _context.Orders
                .GroupBy(o => o.Laptop)
                .Select(g => new TopLaptop
                {
                    Model = g.Key.Name,
                    Brand = g.Key.Brand.Name, // Lấy tên từ Brand
                    RentCount = g.Count(),
                    Revenue = g.Where(o => o.Status == "Completed").Sum(o => o.RentalFee)
                })
                .OrderByDescending(t => t.RentCount)
                .ThenByDescending(t => t.Revenue)
                .Take(count)
                .ToListAsync();
        }

        // Triển khai cho biểu đồ doanh thu hàng tháng
        public async Task<List<ChartData>> GetMonthlyRevenueAsync(int months = 12)
        {
            var result = new List<ChartData>();
            var today = DateTime.UtcNow;

            for (int i = 0; i < months; i++)
            {
                var date = today.AddMonths(-i);
                var startOfMonth = new DateOnly(date.Year, date.Month, 1);
                var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

                var monthlyRevenue = await _context.Orders
                    .Where(o => o.Status == "Completed" && o.EndDate >= startOfMonth && o.EndDate <= endOfMonth)
                    .SumAsync(o => o.RentalFee);

                result.Add(new ChartData
                {
                    Label = startOfMonth.ToString("MMM yyyy", CultureInfo.InvariantCulture),
                    Value = monthlyRevenue,
                    Count = 0 // Thuộc tính Count không áp dụng ở đây
                });
            }

            return result.OrderBy(r => DateTime.ParseExact(r.Label, "MMM yyyy", CultureInfo.InvariantCulture)).ToList();
        }

        // Triển khai cho biểu đồ trạng thái đơn hàng
        public async Task<List<ChartData>> GetOrderStatusChartAsync()
        {
            return await _context.Orders
                .GroupBy(o => o.Status)
                .Select(g => new ChartData
                {
                    Label = g.Key,
                    Value = 0, // Thuộc tính Value không áp dụng ở đây
                    Count = g.Count()
                })
                .ToListAsync();
        }
    }
}