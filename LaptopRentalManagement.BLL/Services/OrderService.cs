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

		public async Task<OrderResponse> CreateAsync(CreateOrderRequest request)
		{
			Order order = _mapper.Map<Order>(request);
			order.StartDate = DateOnly.FromDateTime(request.StartDate);
			order.EndDate = DateOnly.FromDateTime(request.EndDate);
			order = await _orderRepository.CreateAsync(order);
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
                IList<SlotResponse> slotResponse = _mapper.Map<IList<SlotResponse>>(await _slotRepository.GetByOrderId(orderId));
				response.Slots = slotResponse;
            }
			return response;
        }

		private async Task<OrderResponse> buildOrderResponse(Order order)
		{
            OrderResponse response = _mapper.Map<OrderResponse>(order);
            LaptopResponse laptopResponse = _mapper.Map<LaptopResponse>(await _laptopRepository.GetByIdAsync(order.LaptopId));
            AccountResponse ownerResponse = _mapper.Map<AccountResponse>(await _accountRepository.GetByIdAsync(order.OwnerId));
            if (order.RenterId.HasValue)
            {
                AccountResponse? renterResponse = _mapper.Map<AccountResponse>(await _accountRepository.GetByIdAsync(order.RenterId.Value));
                response.Renter = renterResponse;
            }

            response.Owner = ownerResponse;
            response.Laptop = laptopResponse;

			return response;
        }
    }
}