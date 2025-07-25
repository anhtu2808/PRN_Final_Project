namespace LaptopRentalManagement.Model.DTOs.Response.Dashboard
{
    public class DashboardDataRespone
    {
        public DashboardStats Stats { get; set; } = new();
        public List<RecentOrder> RecentOrders { get; set; } = new();
        public List<TopLaptop> TopLaptops { get; set; } = new();
        public List<ChartData> MonthlyRevenue { get; set; } = new();
        public List<ChartData> OrderStatus { get; set; } = new();
    }
}
