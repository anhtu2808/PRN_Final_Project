namespace LaptopRentalManagement.Models.Dashboard
{
    public class DashboardViewModel
    {
        public DashboardStats Stats { get; set; } = new();
        public List<RecentOrder> RecentOrders { get; set; } = new();
        public List<TopLaptop> TopLaptops { get; set; } = new();
        public List<ChartData> MonthlyRevenue { get; set; } = new();
        public List<ChartData> OrderStatus { get; set; } = new();
    }
}
