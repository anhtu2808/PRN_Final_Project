namespace LaptopRentalManagement.BLL.DTOs.Dashboard
{
    /// <summary>
    /// Revenue data model for dashboard charts
    /// </summary>
    public class RevenueData
    {
        public DateTime Date { get; set; }
        public decimal Revenue { get; set; }
        public string Label { get; set; } = string.Empty;
        
        // For chart display
        public string FormattedRevenue => (Revenue / 1000000).ToString("F1"); // Convert to millions for chart
        public string DateLabel => Label;
    }
    
    /// <summary>
    /// Container for revenue chart data with different time periods
    /// </summary>
    public class RevenueChartData
    {
        public List<RevenueData> WeeklyData { get; set; } = new List<RevenueData>();
        public List<RevenueData> MonthlyData { get; set; } = new List<RevenueData>();
        public List<RevenueData> YearlyData { get; set; } = new List<RevenueData>();
    }
}