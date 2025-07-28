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
            // Browser redirect from ZaloPay - process and redirect to payment result page
            var status = Request.Query["status"].ToString();
            var appTransId = Request.Query["apptransid"].ToString();

            if (string.IsNullOrEmpty(status) || string.IsNullOrEmpty(appTransId))
            {
                // Redirect to home with error if missing parameters
                TempData["PaymentError"] = "Invalid payment parameters";
                return RedirectToPage("/Index");
            }

            // Extract Order ID from appTransId (format: 250729_OrderId_timestamp)
            var parts = appTransId.Split('_');
            if (parts.Length < 2 || !int.TryParse(parts[1], out int orderId))
            {
                TempData["PaymentError"] = "Invalid transaction ID format";
                return RedirectToPage("/Index");
            }

            // Update order status based on payment result
            if (status == "1") // Success
            {
                await _orderService.UpdateStatusAsync(orderId, "Pending");
            }
            else
            {
                await _orderService.UpdateStatusAsync(orderId, "Cancelled");
            }

            // Redirect to payment result page
            return RedirectToPage("/Payment/Index", new { orderId = orderId });
        }
        catch (Exception ex)
        {
            // On error, redirect to home with error message
            TempData["PaymentError"] = $"An error occurred: {ex.Message}";
            return RedirectToPage("/Index");
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
                        await _orderService.UpdateStatusAsync(int.Parse(orderId), "Pending");
                    }
                    else
                    {
                        await _orderService.UpdateStatusAsync(int.Parse(orderId), "Cancelled");
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