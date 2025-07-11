namespace LaptopRentalManagement.BLL.DTOs.Dashboard
{
    /// <summary>
    /// Recent order model for dashboard display
    /// </summary>
    public class RecentOrder
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string LaptopName { get; set; } = string.Empty;
        public int Duration { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        
        // Computed properties for display
        public string FormattedOrderId => $"#ORD-{OrderId:D3}";
        public string FormattedAmount => Amount.ToString("C0", new System.Globalization.CultureInfo("vi-VN"));
        public string FormattedDuration => $"{Duration} day{(Duration > 1 ? "s" : "")}";
        public string CustomerInitials => GetInitials(CustomerName);
        public string StatusBadgeClass => Status switch
        {
            "Pending" => "bg-warning",
            "Confirmed" => "bg-success",
            "Completed" => "bg-primary",
            _ => "bg-secondary"
        };
        
        private static string GetInitials(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return "?";
            
            var parts = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 1) return parts[0][0].ToString().ToUpper();
            
            return $"{parts[0][0]}{parts[^1][0]}".ToUpper();
        }
    }
}