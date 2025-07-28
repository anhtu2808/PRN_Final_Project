using AutoMapper;
using LaptopRentalManagement.BLL.DTOs.Request;
using LaptopRentalManagement.BLL.Interfaces;
using LaptopRentalManagement.DAL.Entities;
using LaptopRentalManagement.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopRentalManagement.BLL.Services
{
	public class TicketService : ITicketService
	{
		private readonly ITicketRepository _ticketRepository;
		private readonly IOrderRepository _orderRepository;
		private readonly IMapper _mapper;
		public TicketService(ITicketRepository ticketRepository, IMapper mapper, IOrderRepository orderRepository)
		{
			_ticketRepository = ticketRepository;
			_mapper = mapper;
			_orderRepository = orderRepository;
		}
		public async Task CreateTicketAsync(CreateTicketRequest request)
		{
			var order = await _orderRepository.GetByIdAsync(request.OrderId);

			Ticket ticket = new()
			{
				OrderId = request.OrderId,
				Content = request.Content,
				OwnerId = order.OwnerId,
				RenterId = order.RenterId.Value,
			};
			await _ticketRepository.CreateAsync(ticket);

		}
	}
}
