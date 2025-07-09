using LaptopRentalManagement.BLL.DTOs.Request;
using LaptopRentalManagement.BLL.DTOs.Response;

namespace LaptopRentalManagement.BLL.Interfaces
{
    public interface IOrderService
    {
        Task<BaseResponse<OrderResponse>> CreateOrderAsync(int accountId, CreateOrderRequest request);
        Task<BaseResponse<OrderDetailResponse>> GetOrderByIdAsync(int orderId);
        Task<BaseResponse<PagedResponse<OrderResponse>>> GetOrdersByAccountAsync(int accountId, OrderSearchRequest request);
        Task<BaseResponse<PagedResponse<OrderResponse>>> GetAllOrdersAsync(OrderSearchRequest request);
        Task<BaseResponse<OrderResponse>> UpdateOrderStatusAsync(int orderId, UpdateOrderStatusRequest request);
        Task<BaseResponse<OrderResponse>> ExtendOrderAsync(int orderId, ExtendOrderRequest request);
        Task<BaseResponse<bool>> CancelOrderAsync(int orderId, string reason);
        Task<BaseResponse<OrderSummaryResponse>> GetOrderSummaryAsync();
        Task<BaseResponse<List<OrderResponse>>> GetPendingOrdersAsync();
        Task<BaseResponse<bool>> ApproveOrderAsync(int orderId, string? adminNotes = null);
        Task<BaseResponse<bool>> RejectOrderAsync(int orderId, string reason);
    }
} 