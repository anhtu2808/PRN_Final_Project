using AutoMapper;
using Azure.Core;
using LaptopRentalManagement.BLL.DTOs.Request;
using LaptopRentalManagement.BLL.DTOs.Response;
using LaptopRentalManagement.BLL.Interfaces;
using LaptopRentalManagement.DAL.Entities;
using LaptopRentalManagement.DAL.Interfaces;
using LaptopRentalManagement.Model.DTOs.Request;
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
		private readonly IMapper _mapper;

		public OrderService(IOrderRepository orderRepository, ISlotRespository slotRepository, IMapper mapper, ILaptopRepository laptopRepository, IAccountRepository accountRepository)
		{
			_orderRepository = orderRepository;
			_slotRepository = slotRepository;
			_laptopRepository = laptopRepository;
			_accountRepository = accountRepository;
			_mapper = mapper;
		}

        public async Task<OrderResponse?> ApproveAsync(int orderId)
        {
			OrderResponse? response = null;
			Order? order = await _orderRepository.GetByIdAsync(orderId);
			if (order != null) 
			{
				order.Status = "Renting";
				IList<Slot> slots = await _slotRepository.GetByOrderId(orderId);
				foreach (Slot slot in slots)
				{
					slot.Status = "Booked";
					await _slotRepository.Update(slot);
				}
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
			Order? order = await _orderRepository.GetByIdAsync(orderId);
			if (order != null)
			{
				order.Status = "Completed";
				IList<Slot> slots = await _slotRepository.GetByOrderId(orderId);
				foreach (Slot slot in slots)
				{
					await _slotRepository.DeleteAsync(slot.SlotId);
				}
				await _orderRepository.UpdateAsync(order);
				response = _mapper.Map<OrderResponse>(order);
			}
			return response;
		}
	}
}