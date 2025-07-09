namespace LaptopRentalManagement.BLL.DTOs.Response
{
    public class OrderResponse
    {
        public int OrderId { get; set; }
        public int AccountId { get; set; }
        public int LaptopId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public string? AdminNotes { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        
        // Related entities
        public AccountResponse Customer { get; set; } = new AccountResponse();
        public LaptopResponse Laptop { get; set; } = new LaptopResponse();
    }

    public class OrderDetailResponse
    {
        public int OrderId { get; set; }
        public int AccountId { get; set; }
        public int LaptopId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public string? AdminNotes { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        
        // Related entities with full details
        public AccountResponse Customer { get; set; } = new AccountResponse();
        public LaptopDetailResponse Laptop { get; set; } = new LaptopDetailResponse();
        
        // Order history/status changes
        public List<OrderStatusHistoryResponse> StatusHistory { get; set; } = new List<OrderStatusHistoryResponse>();
    }

    public class OrderStatusHistoryResponse
    {
        public int Id { get; set; }
        public string FromStatus { get; set; } = string.Empty;
        public string ToStatus { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public DateTime ChangeDate { get; set; }
        public string ChangedBy { get; set; } = string.Empty;
    }

    public class OrderSummaryResponse
    {
        public int TotalOrders { get; set; }
        public int PendingOrders { get; set; }
        public int ApprovedOrders { get; set; }
        public int CompletedOrders { get; set; }
        public int CancelledOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal PendingRevenue { get; set; }
    }
} 