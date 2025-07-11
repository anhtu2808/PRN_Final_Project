using LaptopRentalManagement.BLL.DTOs.Dashboard;

namespace LaptopRentalManagement.BLL.Interfaces
{
    /// <summary>
    /// Interface for dashboard data operations
    /// </summary>
    public interface IDashboardService
    {
        /// <summary>
        /// Get dashboard overview statistics
        /// </summary>
        /// <returns>Dashboard statistics including totals and changes</returns>
        Task<DashboardStats> GetDashboardStatsAsync();
        
        /// <summary>
        /// Get recent orders for dashboard display
        /// </summary>
        /// <param name="count">Number of recent orders to retrieve</param>
        /// <returns>List of recent orders</returns>
        Task<List<RecentOrder>> GetRecentOrdersAsync(int count = 5);
        
        /// <summary>
        /// Get popular laptops for dashboard chart
        /// </summary>
        /// <param name="count">Number of popular laptops to retrieve</param>
        /// <returns>List of popular laptops with rental counts</returns>
        Task<List<PopularLaptop>> GetPopularLaptopsAsync(int count = 5);
        
        /// <summary>
        /// Get revenue data for dashboard charts
        /// </summary>
        /// <returns>Revenue data for different time periods</returns>
        Task<RevenueChartData> GetRevenueDataAsync();
    }
}