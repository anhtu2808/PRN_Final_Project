using AutoMapper;
using Azure.Core;
using LaptopRentalManagement.BLL.DTOs.Request;
using LaptopRentalManagement.BLL.DTOs.Response;
using LaptopRentalManagement.BLL.Interfaces;
using LaptopRentalManagement.DAL.Entities;
using LaptopRentalManagement.DAL.Interfaces;
using LaptopRentalManagement.Model.DTOs.Request;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopRentalManagement.BLL.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ISlotRespository _slotRepository;
        private readonly ILaptopRepository _laptopRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IOrderLogRepository _orderLogRepository;
        private readonly IMapper _mapper;
        private readonly IFileUploadService _fileUploadService;
		private readonly IOrderLogImgRepository _orderLogImgRepository;

		public OrderService(IOrderRepository orderRepository, ISlotRespository slotRepository, IMapper mapper, ILaptopRepository laptopRepository, IAccountRepository accountRepository, IOrderLogRepository orderLogRepository, IFileUploadService fileUploadService, IOrderLogImgRepository orderLogImgRepository)
        {
            _orderRepository = orderRepository;
            _slotRepository = slotRepository;
            _laptopRepository = laptopRepository;
            _accountRepository = accountRepository;
            _mapper = mapper;
            _orderLogRepository = orderLogRepository;
            _fileUploadService = fileUploadService;
			_orderLogImgRepository = orderLogImgRepository;
		}

        public async Task<OrderResponse?> ApproveAsync(int orderId)
        {
            OrderResponse? response = null;
            Order? order = await _orderRepository.GetByIdAsync(orderId);
            if (order != null)
            {

                OrderLog log = new()
                {
                    OldStatus = order.Status,
                    NewStatus = "Approved",
                    Content = "Rental request has been accepted",
                    OrderId = order.OrderId
                };
                IList<Slot> slots = await _slotRepository.GetByOrderId(orderId);
                foreach (Slot slot in slots)
                {
                    slot.Status = "Booked";
                    await _slotRepository.Update(slot);
                }

                await _orderLogRepository.CreateAsync(log);

                order.Status = "Approved";
                await _orderRepository.UpdateAsync(order);
                response = _mapper.Map<OrderResponse>(order);
            }
            return response;
        }

        public async Task<OrderResponse?> RejectAsync(int orderId)
        {
            OrderResponse? response = null;
            Order? order = await _orderRepository.GetByIdAsync(orderId);
            if (order != null)
            {
                IList<Slot> slots = await _slotRepository.GetByOrderId(orderId);
                foreach (Slot slot in slots)
                {
                    slot.Status = "Available";
                    await _slotRepository.Update(slot);
                }
                await _orderRepository.DeleteAsync(order.OrderId);
                response = _mapper.Map<OrderResponse>(order);
            }
            return response;
        }

        public async Task<OrderResponse> CreateAsync(CreateOrderRequest request)
        {
            Order order = _mapper.Map<Order>(request);
            var laptop = await _laptopRepository.GetByIdAsync(request.LaptopId);
            var owner = await _accountRepository.GetByIdAsync(laptop.AccountId);
            order.OwnerId = owner.AccountId;
            order = await _orderRepository.CreateAsync(order);

            foreach (int index in request.SlotIds)             
            {
                Slot? slot = await _slotRepository.GetById(index);
                if (slot != null)
                {
                    slot.Status = "Unavailable";
                    slot.OrderId = order.OrderId;
                    await _slotRepository.Update(slot);
                }
            }

            OrderResponse response = _mapper.Map<OrderResponse>(order);

            return response;
        }

        public async Task SetStatusAsync(OrderLogRequest request)
        {
            var order = await _orderRepository.GetByIdAsync(request.OrderId);
            IList<string> urls = new List<string>();
            var lastStatus = "";
            if (order == null)
                return;
            var content = "";
            if (request.NewStatus == "Delivering")
            {
                content = "Laptop is being delivered to you";
            }
            else if (request.NewStatus == "Pending")
            {
                content ="Your rental request is pending approval";
            }
            else if (request.NewStatus == "Renting")
            {
                content = "Laptop has been delivered successfully";
            }
            else
            {
                content = "Laptop has been delivered failed.\nReason: " + request.Reason;
                lastStatus = request.NewStatus;
                foreach(IFormFile form in request.Forms)
                {
                    var url = await _fileUploadService.UploadImageAsync(form, "laptops");
                    urls.Add(url);
                }
				request.NewStatus = "Delivering";
            }

            OrderLog log = new()
            {
                OldStatus = order.Status,
                NewStatus = request.NewStatus,
                Content = content,
                OrderId = order.OrderId
            };

            var orderLog = await _orderLogRepository.CreateAsync(log);
            if (!String.IsNullOrWhiteSpace(lastStatus))
            {
                foreach (var url in urls) 
                {
                    await _orderLogImgRepository.CreateAsync(new()
                    {
                        OrderLogId = orderLog.Id,
                        ImgUrl = url,
                    });
				}
            }

            order.Status = request.NewStatus;
            await _orderRepository.UpdateAsync(order);
        }

        public async Task<IList<OrderResponse>> GetAllAsync(OrderFilter orderFilter)
        {
            IList<Order> orders = await _orderRepository.GetAllAsync(orderFilter);
            IList<OrderResponse> responses = new List<OrderResponse>();

            foreach (Order order in orders)
            {
                responses.Add(await buildOrderResponse(order));
            }

            return responses;

        }

        public async Task<OrderResponse?> GetByIdAsync(int orderId)
        {
            Order? order = await _orderRepository.GetByIdAsync(orderId);
            OrderResponse? response = null;
            if (order != null)
            {
                response = await buildOrderResponse(order);
            }
            return response;
        }

        private async Task<OrderResponse> buildOrderResponse(Order order)
        {
            OrderResponse response = _mapper.Map<OrderResponse>(order);
            LaptopResponse laptopResponse = _mapper.Map<LaptopResponse>(await _laptopRepository.GetByIdAsync(order.LaptopId));
            AccountResponse ownerResponse = _mapper.Map<AccountResponse>(await _accountRepository.GetByIdAsync(order.OwnerId));
            IList<SlotResponse> slotResponse = _mapper.Map<IList<SlotResponse>>(await _slotRepository.GetByOrderId(order.OrderId));
            if (order.RenterId.HasValue)
            {
                AccountResponse? renterResponse = _mapper.Map<AccountResponse>(await _accountRepository.GetByIdAsync(order.RenterId.Value));
                response.Renter = renterResponse;
            }

            response.Owner = ownerResponse;
            response.Laptop = laptopResponse;
            response.Slots = slotResponse;

            return response;
        }

        public async Task<OrderResponse?> ConfirmReturn(int orderId)
        {
            OrderResponse? response = null;
            bool stillRent = false;
            Order? order = await _orderRepository.GetByIdAsync(orderId);
            if (order != null)
            {
                DateTime date = DateTime.UtcNow;
                IList<Slot> slots = await _slotRepository.GetByOrderId(orderId);
                foreach (Slot slot in slots)
                {
                    DateTime slotStart = slot.SlotDate.ToDateTime(new TimeOnly(8, 0));       // 8:00 sáng SlotDate
                    DateTime slotEnd = slot.SlotDate.AddDays(1).ToDateTime(new TimeOnly(7, 0)); // 7:00 sáng hôm sau

                    if (date >= slotStart && date < slotEnd)
                    {
                        //slot.Status = "Completed"; check có bị phạt hay không
                    }
                    DateOnly dateOnly = DateOnly.FromDateTime(date);
                    stillRent = dateOnly <= slot.SlotDate ? true : false;

                }
                order.Status = stillRent ? "Approved" : "Completed";

                await _orderRepository.UpdateAsync(order);
                response = _mapper.Map<OrderResponse>(order);
            }
            return response;
        }

        // ZaloPay payment methods
        public async Task<OrderResponse> CreateOrderForPaymentAsync(CreateOrderRequest request)
        {
            // Create order with "Unpaid" status
            var order = new Order
            {
                LaptopId = request.LaptopId,
                RenterId = request.RenterId,
                TotalCharge = request.TotalCharge,
                Status = "Unpaid",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Calculate start and end dates from slots
            var slots = await _slotRepository.GetByIdsAsync(request.SlotIds.ToList());
            if (slots.Any())
            {
                order.StartDate = slots.Min(s => s.SlotDate);
                order.EndDate = slots.Max(s => s.SlotDate);
            }

            // Get laptop owner ID
            var laptop = await _laptopRepository.GetByIdAsync(request.LaptopId);
            if (laptop != null)
            {
                order.OwnerId = laptop.AccountId;
            }

            var createdOrder = await _orderRepository.CreateAsync(order);

            // Update slots with the new order - for now we'll skip this
            // This would need to be implemented in SlotRepository if needed
            // foreach (var slotId in request.SlotIds)
            // {
            //     await _slotRepository.UpdateOrderIdAsync(slotId, createdOrder.OrderId);
            // }
            foreach (int index in request.SlotIds)             
            {
                Slot? slot = await _slotRepository.GetById(index);
                if (slot != null)
                {
                    slot.Status = "Unavailable";
                    slot.OrderId = order.OrderId;
                    await _slotRepository.Update(slot);
                }
            }

            return await buildOrderResponse(createdOrder);
        }

        public async Task<bool> UpdateOrderPaymentStatusAsync(string zaloPayTransactionId, string status)
        {
            var order = await _orderRepository.GetByZaloPayTransactionIdAsync(zaloPayTransactionId);
            if (order == null) return false;

            order.Status = status;
            order.UpdatedAt = DateTime.UtcNow;

            await _orderRepository.UpdateAsync(order);
            return true;
        }

        public async Task<Order?> GetOrderByZaloPayTransactionIdAsync(string transactionId)
        {
            return await _orderRepository.GetByZaloPayTransactionIdAsync(transactionId);
        }

        public async Task<Order?> GetOrderEntityByIdAsync(int orderId)
        {
            return await _orderRepository.GetByIdAsync(orderId);
        }
    }
}