using LaptopRentalManagement.DAL.Entities;
using LaptopRentalManagement.Model.DTOs.Response.Order;
using LaptopRentalManagement.Model.DTOs.Response.Refund;
using Microsoft.AspNetCore.Http;

namespace LaptopRentalManagement.BLL.Interfaces;

public interface IZaloPayService
{
    Task<ZaloPayCreateOrderResponse> GetPymentUrl(long amount, Order order, string redirectUrl);
    Task<ZaloPayRefundResponse> RefundAsync(string zpTransId, long amount, string description);
    bool VerifyCallback(IQueryCollection query);
    bool VerifyRedirect(IQueryCollection query);
}