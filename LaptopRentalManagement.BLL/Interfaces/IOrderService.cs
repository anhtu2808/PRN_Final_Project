﻿using LaptopRentalManagement.BLL.DTOs.Request;
using LaptopRentalManagement.BLL.DTOs.Response;
using LaptopRentalManagement.DAL.Entities;
using LaptopRentalManagement.Model.DTOs.Request;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopRentalManagement.BLL.Interfaces
{
	public interface IOrderService
	{
		Task<IList<OrderResponse>> GetAllAsync(OrderFilter orderFilter);
		Task<OrderResponse> CreateAsync(CreateOrderRequest request);
        Task<OrderResponse?> GetByIdAsync(int orderId);
        Task<OrderResponse?> RejectAsync(int orderId);
        Task<OrderResponse?> ApproveAsync(int orderId);
		Task<OrderResponse?> ConfirmReturn(int orderId);
        Task SetStatusAsync(OrderLogRequest request);
        Task UpdateStatusAsync(int orderId, string newStatus);

        // ZaloPay payment methods
        Task<OrderResponse> CreateOrderForPaymentAsync(CreateOrderRequest request);
        Task<bool> UpdateOrderPaymentStatusAsync(string zaloPayTransactionId, string status);
        Task<Order?> GetOrderByZaloPayTransactionIdAsync(string transactionId);
        Task<Order?> GetOrderEntityByIdAsync(int orderId);

        Task<decimal> GetCompletedRevenueAsync(int ownerId);
    }
}
