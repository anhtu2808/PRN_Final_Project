namespace LaptopRentalManagement.BLL.DTOs.Dashboard
{
    /// <summary>
    /// Popular laptop model for dashboard chart
    /// </summary>
    public class PopularLaptop
    {
        public string LaptopName { get; set; } = string.Empty;
        public int OrderCount { get; set; }
        public decimal Percentage { get; set; }
        public string BrandName { get; set; } = string.Empty;
        
        // For chart display
        public string DisplayName => LaptopName.Length > 15 ? $"{LaptopName[..12]}..." : LaptopName;
        public string FormattedPercentage => $"{Percentage:F1}%";
    }
}