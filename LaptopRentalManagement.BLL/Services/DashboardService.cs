using Microsoft.EntityFrameworkCore;
using LaptopRentalManagement.BLL.Interfaces;
using LaptopRentalManagement.DAL.LaptopRentalManagement.DAL.Context;
using LaptopRentalManagement.BLL.DTOs.Dashboard;

namespace LaptopRentalManagement.BLL.Services
{
    /// <summary>
    /// Service for dashboard data operations
    /// </summary>
    public class DashboardService : IDashboardService
    {
        private readonly LaptopRentalDbContext _context;

        public DashboardService(LaptopRentalDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardStats> GetDashboardStatsAsync()
        {
            try
            {
                var currentDate = DateTime.Now;
                var currentMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
                var previousMonth = currentMonth.AddMonths(-1);
                var currentWeekStart = currentDate.AddDays(-(int)currentDate.DayOfWeek);

                // Current month statistics
                var currentMonthOrders = await _context.Orders
                    .Where(o => o.CreatedAt >= currentMonth)
                    .CountAsync();

                var currentMonthRevenue = await _context.Orders
                    .Where(o => o.CreatedAt >= currentMonth && o.Status == "Completed")
                    .SumAsync(o => o.TotalCharge);

                // Previous month statistics for comparison
                var previousMonthOrders = await _context.Orders
                    .Where(o => o.CreatedAt >= previousMonth && o.CreatedAt < currentMonth)
                    .CountAsync();

                var previousMonthRevenue = await _context.Orders
                    .Where(o => o.CreatedAt >= previousMonth && o.CreatedAt < currentMonth && o.Status == "Completed")
                    .SumAsync(o => o.TotalCharge);

                // Active rentals (confirmed orders that haven't ended yet)
                var activeRentals = await _context.Orders
                    .Where(o => o.Status == "Confirmed" && o.EndDate >= DateOnly.FromDateTime(currentDate))
                    .CountAsync();

                // Due today
                var dueToday = await _context.Orders
                    .Where(o => o.Status == "Confirmed" && o.EndDate == DateOnly.FromDateTime(currentDate))
                    .CountAsync();

                // Total customers
                var totalCustomers = await _context.Accounts
                    .Where(a => a.Role == "Customer")
                    .CountAsync();

                // New customers this week
                var newCustomersThisWeek = await _context.Accounts
                    .Where(a => a.Role == "Customer" && a.CreatedAt >= currentWeekStart)
                    .CountAsync();

                // Calculate percentage changes
                var ordersChangePercent = previousMonthOrders > 0 
                    ? ((decimal)(currentMonthOrders - previousMonthOrders) / previousMonthOrders) * 100 
                    : 0;

                var revenueChangePercent = previousMonthRevenue > 0 
                    ? ((currentMonthRevenue - previousMonthRevenue) / previousMonthRevenue) * 100 
                    : 0;

                return new DashboardStats
                {
                    TotalOrders = currentMonthOrders,
                    Revenue = currentMonthRevenue,
                    ActiveRentals = activeRentals,
                    TotalCustomers = totalCustomers,
                    OrdersChangePercent = ordersChangePercent,
                    RevenueChangePercent = revenueChangePercent,
                    DueToday = dueToday,
                    NewCustomersThisWeek = newCustomersThisWeek
                };
            }
            catch (Exception)
            {
                // Return default values if database error occurs
                return new DashboardStats();
            }
        }

        public async Task<List<RecentOrder>> GetRecentOrdersAsync(int count = 5)
        {
            try
            {
                var recentOrders = await _context.Orders
                    .Include(o => o.Renter)
                    .Include(o => o.Laptop)
                    .OrderByDescending(o => o.CreatedAt)
                    .Take(count)
                    .Select(o => new RecentOrder
                    {
                        OrderId = o.OrderId,
                        CustomerName = o.Renter.Name ?? "Unknown",
                        CustomerEmail = o.Renter.Email,
                        LaptopName = o.Laptop.Name,
                        Duration = (o.EndDate.ToDateTime(TimeOnly.MinValue) - o.StartDate.ToDateTime(TimeOnly.MinValue)).Days,
                        Amount = o.TotalCharge,
                        Status = o.Status,
                        CreatedAt = o.CreatedAt
                    })
                    .ToListAsync();

                return recentOrders;
            }
            catch (Exception)
            {
                return new List<RecentOrder>();
            }
        }

        public async Task<List<PopularLaptop>> GetPopularLaptopsAsync(int count = 5)
        {
            try
            {
                var totalOrders = await _context.Orders.CountAsync();
                if (totalOrders == 0) return new List<PopularLaptop>();

                var popularLaptops = await _context.Orders
                    .Include(o => o.Laptop)
                    .ThenInclude(l => l.Brand)
                    .GroupBy(o => new { LaptopName = o.Laptop.Name, BrandName = o.Laptop.Brand.Name })
                    .Select(g => new PopularLaptop
                    {
                        LaptopName = g.Key.LaptopName,
                        BrandName = g.Key.BrandName,
                        OrderCount = g.Count(),
                        Percentage = (decimal)g.Count() / totalOrders * 100
                    })
                    .OrderByDescending(p => p.OrderCount)
                    .Take(count)
                    .ToListAsync();

                return popularLaptops;
            }
            catch (Exception)
            {
                return new List<PopularLaptop>();
            }
        }

        public async Task<RevenueChartData> GetRevenueDataAsync()
        {
            try
            {
                var currentDate = DateTime.Now;
                
                // Weekly data (last 7 days)
                var weeklyData = new List<RevenueData>();
                for (int i = 6; i >= 0; i--)
                {
                    var date = currentDate.AddDays(-i);
                    var revenue = await _context.Orders
                        .Where(o => o.CreatedAt.Date == date.Date && o.Status == "Completed")
                        .SumAsync(o => o.TotalCharge);

                    weeklyData.Add(new RevenueData
                    {
                        Date = date,
                        Revenue = revenue,
                        Label = date.ToString("ddd")
                    });
                }

                // Monthly data (last 12 months)
                var monthlyData = new List<RevenueData>();
                for (int i = 11; i >= 0; i--)
                {
                    var monthStart = new DateTime(currentDate.Year, currentDate.Month, 1).AddMonths(-i);
                    var monthEnd = monthStart.AddMonths(1);
                    
                    var revenue = await _context.Orders
                        .Where(o => o.CreatedAt >= monthStart && o.CreatedAt < monthEnd && o.Status == "Completed")
                        .SumAsync(o => o.TotalCharge);

                    monthlyData.Add(new RevenueData
                    {
                        Date = monthStart,
                        Revenue = revenue,
                        Label = monthStart.ToString("MMM")
                    });
                }

                // Yearly data (last 5 years)
                var yearlyData = new List<RevenueData>();
                for (int i = 4; i >= 0; i--)
                {
                    var year = currentDate.Year - i;
                    var yearStart = new DateTime(year, 1, 1);
                    var yearEnd = yearStart.AddYears(1);
                    
                    var revenue = await _context.Orders
                        .Where(o => o.CreatedAt >= yearStart && o.CreatedAt < yearEnd && o.Status == "Completed")
                        .SumAsync(o => o.TotalCharge);

                    yearlyData.Add(new RevenueData
                    {
                        Date = yearStart,
                        Revenue = revenue,
                        Label = year.ToString()
                    });
                }

                return new RevenueChartData
                {
                    WeeklyData = weeklyData,
                    MonthlyData = monthlyData,
                    YearlyData = yearlyData
                };
            }
            catch (Exception)
            {
                return new RevenueChartData();
            }
        }
    }
}