using System.Text.Json;
using LaptopRentalManagement.BLL.Interfaces;
using LaptopRentalManagement.Models.Dashboard;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LaptopRentalManagement.Pages.Manage;

public class IndexModel : PageModel
{
    private readonly IDashboardService _dashboardService;

    public IndexModel(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    // Tạo một thuộc tính để giữ toàn bộ dữ liệu cho Dashboard
    public DashboardViewModel DashboardData { get; set; }

    // Các thuộc tính để chuyển dữ liệu sang JSON cho JavaScript
    public string MonthlyRevenueChartDataJson { get; private set; }
    public string OrderStatusChartDataJson { get; private set; }

    public async Task<IActionResult> OnGetAsync()
    {
        // Gọi phương thức chính để lấy tất cả dữ liệu cùng lúc
        DashboardData = await _dashboardService.GetDashboardDataAsync();

        if (DashboardData == null)
            // Nếu không có dữ liệu, khởi tạo các đối tượng rỗng để tránh lỗi null reference trong view
            DashboardData = new DashboardViewModel();

        // Chuyển đổi dữ liệu biểu đồ sang chuỗi JSON để sử dụng trong <script>
        // Biểu đồ Doanh thu (Monthly Revenue)
        var monthlyRevenueLabels = DashboardData.MonthlyRevenue.Select(d => d.Label).ToList();
        var monthlyRevenueValues = DashboardData.MonthlyRevenue.Select(d => d.Value).ToList();
        MonthlyRevenueChartDataJson =
            JsonSerializer.Serialize(new { labels = monthlyRevenueLabels, values = monthlyRevenueValues });

        // Biểu đồ Trạng thái Đơn hàng (Order Status) - Dùng cho biểu đồ Popular Laptops
        var orderStatusLabels = DashboardData.OrderStatus.Select(d => d.Label).ToList();
        var orderStatusValues = DashboardData.OrderStatus.Select(d => d.Count).ToList();
        OrderStatusChartDataJson =
            JsonSerializer.Serialize(new { labels = orderStatusLabels, values = orderStatusValues });

        return Page();
    }
}