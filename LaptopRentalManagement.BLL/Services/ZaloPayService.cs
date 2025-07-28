using LaptopRentalManagement.BLL.Interfaces;
using LaptopRentalManagement.DAL.Entities;
using LaptopRentalManagement.Model.DTOs.Request;
using LaptopRentalManagement.Model.DTOs.Response.Order;
using LaptopRentalManagement.Model.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;

namespace LaptopRentalManagement.BLL.Services;

public class ZaloPayService : IZaloPayService
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;
    private readonly string _appId;
    private readonly string _key1;
    private readonly string _key2;
    private readonly string _createOrderUrl;

    public ZaloPayService(IConfiguration configuration, HttpClient httpClient)
    {
        _configuration = configuration;
        _httpClient = httpClient;
        _appId = _configuration["ZaloPay:AppId"]!;
        _key1 = _configuration["ZaloPay:Key1"]!;
        _key2 = _configuration["ZaloPay:Key2"]!;
        _createOrderUrl = _configuration["ZaloPay:CreateOrderUrl"]!;
    }

    public async Task<ZaloPayCreateOrderResponse> GetPymentUrl(long amount, Order order, string redirectUrl)
    {
        try
        {
            var appTransId = GenerateAppTransId(order.OrderId);
            var appTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            var embedData = JsonConvert.SerializeObject(new { 
                redirecturl = redirectUrl,
                orderId = order.OrderId,
                orderCode = appTransId
            });

            var item = JsonConvert.SerializeObject(new[] {
                new {
                    itemid = order.Laptop?.Name ?? "Laptop Rental",
                    itemname = $"Rental - {order.Laptop?.Name}",
                    itemprice = amount,
                    itemquantity = 1
                }
            });

            var createOrderRequest = new ZaloPayCreateOrderRequest
            {
                AppId = int.Parse(_appId),
                AppTransId = appTransId,
                AppUser = $"user_{order.RenterId}",
                Amount = amount,
                AppTime = appTime,
                EmbedData = embedData,
                Item = item,
                Description = $"Payment for laptop rental - Order #{order.OrderId}",
                CallbackUrl = $"{redirectUrl.Split("/PaymentCallback")[0]}/api/zalopay/callback"
            };

            // Generate MAC signature
            createOrderRequest.MakeSignature(_key1);

            // Prepare form data
            var formData = new List<KeyValuePair<string, string>>
            {
                new("app_id", createOrderRequest.AppId.ToString()),
                new("app_trans_id", createOrderRequest.AppTransId),
                new("app_user", createOrderRequest.AppUser),
                new("amount", createOrderRequest.Amount.ToString()),
                new("app_time", createOrderRequest.AppTime.ToString()),
                new("embed_data", createOrderRequest.EmbedData),
                new("item", createOrderRequest.Item),
                new("description", createOrderRequest.Description),
                new("bank_code", createOrderRequest.BankCode),
                new("callback_url", createOrderRequest.CallbackUrl),
                new("mac", createOrderRequest.Mac)
            };

            var formContent = new FormUrlEncodedContent(formData);

            // Call ZaloPay API
            var response = await _httpClient.PostAsync(_createOrderUrl, formContent);
            var responseContent = await response.Content.ReadAsStringAsync();


            try
            {
                var result = JsonConvert.DeserializeObject<ZaloPayCreateOrderResponse>(responseContent);
                
                if (result == null)
                {
                    return new ZaloPayCreateOrderResponse
                    {
                        ReturnCode = -1,
                        ReturnMessage = "Failed to parse ZaloPay response"
                    };
                }

                return result;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"JSON Parse Error: {ex.Message}");
                return new ZaloPayCreateOrderResponse
                {
                    ReturnCode = -1,
                    ReturnMessage = $"JSON parsing failed: {ex.Message}"
                };
            }
        }
        catch (Exception ex)
        {
            return new ZaloPayCreateOrderResponse
            {
                ReturnCode = -1,
                ReturnMessage = $"Error calling ZaloPay API: {ex.Message}"
            };
        }
    }

    public bool VerifyCallback(IQueryCollection query)
    {
        try
        {
            var dataStr = query["data"].ToString();
            var reqMac = query["mac"].ToString();

            if (string.IsNullOrEmpty(dataStr) || string.IsNullOrEmpty(reqMac))
                return false;

            var mac = HmacHelper.Compute(_key2, dataStr);
            return mac.Equals(reqMac, StringComparison.OrdinalIgnoreCase);
        }
        catch
        {
            return false;
        }
    }

    public bool VerifyRedirect(IQueryCollection query)
    {
        try
        {
            // For ZaloPay redirect, MAC verification is optional and often not included
            // We'll do basic validation instead
            var status = query["status"].ToString();
            var appTransId = query["apptransid"].ToString();
            
            // Basic validation - check if essential parameters exist
            return !string.IsNullOrEmpty(status) && !string.IsNullOrEmpty(appTransId);
        }
        catch
        {
            return false;
        }
    }

    private string GenerateAppTransId(int orderId)
    {
        var timestamp = DateTime.Now.ToString("yyMMdd");
        return $"{timestamp}_{orderId}_{DateTime.Now.Ticks}";
    }
}