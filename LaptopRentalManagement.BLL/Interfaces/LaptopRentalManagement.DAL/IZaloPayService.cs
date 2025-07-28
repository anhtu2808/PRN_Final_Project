using LaptopRentalManagement.Model.DTOs.Response.Order;
using Microsoft.AspNetCore.Http;

namespace LaptopRentalManagement.BLL.Interfaces.LaptopRentalManagement.DAL;

public interface IZaloPayService
{
    Task<ZaloPayCreateOrderResponse> CreateOrderAsync(long amount, string orderDescription, string redirectUrl);
    bool VerifyCallback(IQueryCollection query);
}