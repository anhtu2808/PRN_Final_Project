using System.Text.Json;
using LaptopRentalManagement.BLL.Interfaces;
using LaptopRentalManagement.Model.DTOs.Response.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LaptopRentalManagement.Pages.Manage;
[Authorize(Policy = "AdminOnly")]
public class IndexModel : PageModel
{

    private readonly IDashboardService _dashboardService;

    public IndexModel(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    public DashboardDataRespone DashboardData { get; set; }

    public string MonthlyRevenueChartDataJson { get; private set; }
    public string OrderStatusChartDataJson { get; private set; }

    public async Task<IActionResult> OnGetAsync()
    {
        DashboardData = await _dashboardService.GetDashboardDataAsync();

        if (DashboardData == null)
            DashboardData = new DashboardDataRespone();
        var monthlyRevenueLabels = DashboardData.MonthlyRevenue.Select(d => d.Label).ToList();
        var monthlyRevenueValues = DashboardData.MonthlyRevenue.Select(d => d.Value).ToList();
        MonthlyRevenueChartDataJson =
            JsonSerializer.Serialize(new { labels = monthlyRevenueLabels, values = monthlyRevenueValues });
        var orderStatusLabels = DashboardData.OrderStatus.Select(d => d.Label).ToList();
        var orderStatusValues = DashboardData.OrderStatus.Select(d => d.Count).ToList();
        OrderStatusChartDataJson =
            JsonSerializer.Serialize(new { labels = orderStatusLabels, values = orderStatusValues });

        return Page();
    }
}