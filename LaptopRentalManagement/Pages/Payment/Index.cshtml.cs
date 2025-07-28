using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LaptopRentalManagement.BLL.Interfaces;
using LaptopRentalManagement.BLL.DTOs.Response;

namespace LaptopRentalManagement.Pages.Payment;

public class IndexModel : PageModel
{
    private readonly IOrderService _orderService;

    public IndexModel(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public OrderResponse? Order { get; set; }
    public string PaymentStatus { get; set; } = string.Empty;
    public string StatusMessage { get; set; } = string.Empty;
    public string StatusClass { get; set; } = string.Empty;

    public async Task<IActionResult> OnGetAsync(int orderId)
    {
        try
        {
            Order = await _orderService.GetByIdAsync(orderId);
            
            if (Order == null)
            {
                return NotFound("Order not found");
            }

            // Determine payment status based on order status
            switch (Order.Status?.ToLower())
            {
                case "pending":
                    PaymentStatus = "success";
                    StatusMessage = "Payment Successful!";
                    StatusClass = "success";
                    break;
                case "unpaid":
                    PaymentStatus = "failed";
                    StatusMessage = "Payment Failed!";
                    StatusClass = "danger";
                    break;
                default:
                    PaymentStatus = "processing";
                    StatusMessage = "Payment Processing...";
                    StatusClass = "warning";
                    break;
            }

            return Page();
        }
        catch (Exception ex)
        {
            PaymentStatus = "error";
            StatusMessage = $"Error loading payment information: {ex.Message}";
            StatusClass = "danger";
            return Page();
        }
    }
} 