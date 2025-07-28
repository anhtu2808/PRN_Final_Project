using LaptopRentalManagement.DAL.Entities;
using LaptopRentalManagement.Model.DTOs.Response.Order;
using Microsoft.AspNetCore.Http;

namespace LaptopRentalManagement.BLL.Interfaces;

public interface IZaloPayService
{
    Task<ZaloPayCreateOrderResponse> GetPymentUrl(long amount, Order order, string redirectUrl);
    bool VerifyCallback(IQueryCollection query);
    bool VerifyRedirect(IQueryCollection query);
}