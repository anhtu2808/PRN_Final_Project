using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LaptopRentalManagement.BLL.Interfaces;
using LaptopRentalManagement.BLL.DTOs.Dashboard;

namespace LaptopRentalManagement.Pages.Manage
{
    public class IndexModel : PageModel
    {
        private readonly IDashboardService _dashboardService;

        public IndexModel(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        // Dashboard data properties
        public DashboardStats Stats { get; set; } = new DashboardStats();
        public List<RecentOrder> RecentOrders { get; set; } = new List<RecentOrder>();
        public List<PopularLaptop> PopularLaptops { get; set; } = new List<PopularLaptop>();
        public RevenueChartData RevenueData { get; set; } = new RevenueChartData();

        public async Task OnGetAsync()
        {
            try
            {
                // Load dashboard data from service
                Stats = await _dashboardService.GetDashboardStatsAsync();
                RecentOrders = await _dashboardService.GetRecentOrdersAsync(5);
                PopularLaptops = await _dashboardService.GetPopularLaptopsAsync(5);
                RevenueData = await _dashboardService.GetRevenueDataAsync();
            }
            catch (Exception)
            {
                // If there's an error loading data, keep default empty values
                // Log error in production
            }
        }
    }
} 