namespace LaptopRentalManagement.BLL.DTOs.Dashboard
{
    /// <summary>
    /// Dashboard overview statistics model
    /// </summary>
    public class DashboardStats
    {
        public int TotalOrders { get; set; }
        public decimal Revenue { get; set; }
        public int ActiveRentals { get; set; }
        public int TotalCustomers { get; set; }
        
        // Percentage changes compared to previous month
        public decimal OrdersChangePercent { get; set; }
        public decimal RevenueChangePercent { get; set; }
        public int DueToday { get; set; }
        public int NewCustomersThisWeek { get; set; }
        
        // Formatted values for display
        public string FormattedRevenue => Revenue.ToString("C0", new System.Globalization.CultureInfo("vi-VN"));
        public string FormattedOrdersChange => OrdersChangePercent >= 0 ? $"+{OrdersChangePercent:F0}%" : $"{OrdersChangePercent:F0}%";
        public string FormattedRevenueChange => RevenueChangePercent >= 0 ? $"+{RevenueChangePercent:F0}%" : $"{RevenueChangePercent:F0}%";
    }
}