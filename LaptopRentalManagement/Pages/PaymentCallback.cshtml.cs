using LaptopRentalManagement.BLL.Interfaces;
using LaptopRentalManagement.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace LaptopRentalManagement.Pages;

public class PaymentCallbackModel : PageModel
{
    private readonly IOrderService _orderService;
    private readonly IZaloPayService _zaloPayService;

    public PaymentCallbackModel(IOrderService orderService, IZaloPayService zaloPayService)
    {
        _orderService = orderService;
        _zaloPayService = zaloPayService;
    }

    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public int OrderId { get; set; }

    // Handle browser redirect from ZaloPay (GET)
    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            // Browser redirect from ZaloPay - show result page
            var status = Request.Query["status"].ToString();
            var appTransId = Request.Query["apptransid"].ToString();

            // Extract Order ID from appTransId for display (format: 250729_OrderId_timestamp)
            if (!string.IsNullOrEmpty(appTransId))
            {
                var parts = appTransId.Split('_');
                if (parts.Length >= 2 && int.TryParse(parts[1], out int orderId))
                {
                    OrderId = orderId;
                }
            }

            if (status == "1")
            {
                IsSuccess = true;
                Message = OrderId > 0
                    ? $"Payment completed! Your order #{OrderId} is being processed."
                    : "Payment completed! Please wait for confirmation.";
            }
            else
            {
                IsSuccess = false;
                Message = "Payment was cancelled or failed.";
            }
            if (status == "1") // Success
            {
				await _orderService.SetStatusAsync(new()
				{
					OrderId = OrderId,
					NewStatus = "Pending"
				});
			}
            else
            {
				await _orderService.SetStatusAsync(new()
				{
					OrderId = OrderId,
					NewStatus = "Cancelled"
				});
			}

            return Page();
        }
        catch (Exception ex)
        {
            IsSuccess = false;
            Message = $"An error occurred: {ex.Message}";
            return Page();
        }
    }

    // Handle ZaloPay server callback (POST)
    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            // ZaloPay server callback - verify and update order
            var body = await new StreamReader(Request.Body).ReadToEndAsync();

            // Parse form data
            var formData = new Dictionary<string, string>();
            var pairs = body.Split('&');
            foreach (var pair in pairs)
            {
                var keyValue = pair.Split('=');
                if (keyValue.Length == 2)
                {
                    formData[Uri.UnescapeDataString(keyValue[0])] = Uri.UnescapeDataString(keyValue[1]);
                }
            }

            if (!formData.ContainsKey("data") || !formData.ContainsKey("mac"))
            {
                return new JsonResult(new { return_code = -1, return_message = "Invalid callback data" });
            }

            var data = formData["data"];
            var mac = formData["mac"];


            // Parse callback data
            var callbackData = JsonConvert.DeserializeObject<dynamic>(data);
            var appTransId = callbackData?.app_trans_id?.ToString();
            var status = callbackData?.status?.ToString();

            if (string.IsNullOrEmpty(appTransId))
            {
                return new JsonResult(new { return_code = -1, return_message = "Missing transaction ID" });
            }

            // Extract Order ID from app_trans_id (format: 250729_OrderId_timestamp)
            var parts = appTransId.Split('_');
            var orderId = parts.Length > 1 ? parts[1] : null;
            {
                var order = await _orderService.GetOrderEntityByIdAsync(orderId);

                if (order != null)
                {
                    // Update order status based on ZaloPay callback
                    if (status == "1") // Success
                    {
						await _orderService.SetStatusAsync(new()
						{
							OrderId = orderId,
							NewStatus = "Pending"
						});
					}
                    else
                    {
						await _orderService.SetStatusAsync(new()
						{
							OrderId = orderId,
							NewStatus = "Cancelled"
						});
					}

                    return new JsonResult(new { return_code = 1, return_message = "Success" });
                }
                else
                {
                    return new JsonResult(new { return_code = -1, return_message = "Order not found" });
                }
            }
        }
        catch (Exception ex)
        {
            return new JsonResult(new { return_code = -1, return_message = "Server error" });
        }
    }
}