namespace LaptopRentalManagement.Model.DTOs.Response.Dashboard
{
    public class DashboardStats
    {
        public int TotalOrders { get; set; }
        public decimal Revenue { get; set; }
        public int ActiveRentals { get; set; }
        public int TotalCustomers { get; set; }
        public double OrderGrowth { get; set; }
        public double RevenueGrowth { get; set; }
        public int DueToday { get; set; }
        public int NewCustomersThisWeek { get; set; }
    }
}
