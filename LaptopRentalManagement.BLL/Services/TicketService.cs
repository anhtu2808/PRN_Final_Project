using AutoMapper;
using LaptopRentalManagement.BLL.DTOs.Request;
using LaptopRentalManagement.BLL.DTOs.Response;
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
		private readonly IAccountRepository _accountRepository;
		private readonly IMapper _mapper;
		public TicketService(ITicketRepository ticketRepository, IMapper mapper, IOrderRepository orderRepository, IAccountRepository accountRepository)
		{
			_ticketRepository = ticketRepository;
			_mapper = mapper;
			_orderRepository = orderRepository;
			_accountRepository = accountRepository;
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

        public async Task<IList<TicketResponse>> GetAllByOrderIdAsync(int id)
        {
                var tickets = await _ticketRepository.GetAllByOrderIdAsync(id);
                IList<TicketResponse> responses = new List<TicketResponse>();
			foreach (Ticket ticket in tickets)
			{
				var response = _mapper.Map<TicketResponse>(ticket);
				var renter = _mapper.Map<AccountResponse>(await _accountRepository.GetByIdAsync(ticket.RenterId));
				var order = _mapper.Map<OrderResponse>(await _orderRepository.GetByIdAsync(id));
				AccountResponse owner;
				if (ticket.OwnerId.HasValue)
				{
					owner = _mapper.Map<AccountResponse>(await _accountRepository.GetByIdAsync(ticket.OwnerId.Value));
					response.Owner = owner;
				}
				response.Renter = renter;
				response.Order = order;
				responses.Add(response);
			}
                return responses;
        }

        public async Task<IList<TicketResponse>> GetByRenterIdAsync(int renterId)
        {
                var tickets = await _ticketRepository.GetByRenterIdAsync(renterId);
                IList<TicketResponse> responses = new List<TicketResponse>();

                foreach (Ticket ticket in tickets)
                {
                        var response = _mapper.Map<TicketResponse>(ticket);
                        var renter = _mapper.Map<AccountResponse>(await _accountRepository.GetByIdAsync(ticket.RenterId));
                        var order = _mapper.Map<OrderResponse>(await _orderRepository.GetByIdAsync(ticket.OrderId));

                        AccountResponse? owner = null;
                        if (ticket.OwnerId.HasValue)
                        {
                                owner = _mapper.Map<AccountResponse>(await _accountRepository.GetByIdAsync(ticket.OwnerId.Value));
                        }

                        response.Renter = renter;
                        response.Owner = owner;
                        response.Order = order;
                        responses.Add(response);
                }

                return responses;
        }

        public async Task<IList<TicketResponse>> GetByAccountIdAsync(int accountId)
        {
                var tickets = await _ticketRepository.GetByAccountIdAsync(accountId);
                IList<TicketResponse> responses = new List<TicketResponse>();

                foreach (Ticket ticket in tickets)
                {
                        var response = _mapper.Map<TicketResponse>(ticket);
                        var renter = _mapper.Map<AccountResponse>(await _accountRepository.GetByIdAsync(ticket.RenterId));
                        var order = _mapper.Map<OrderResponse>(await _orderRepository.GetByIdAsync(ticket.OrderId));

                        AccountResponse? owner = null;
                        if (ticket.OwnerId.HasValue)
                        {
                                owner = _mapper.Map<AccountResponse>(await _accountRepository.GetByIdAsync(ticket.OwnerId.Value));
                        }

                        response.Renter = renter;
                        response.Owner = owner;
                        response.Order = order;
                        responses.Add(response);
                }

                return responses;
        }

		public async Task<IList<TicketResponse>> GetAllAsync()
		{
			var tickets = await _ticketRepository.GetAllAsync();
			IList<TicketResponse> responses = new List<TicketResponse>();
			
			foreach (Ticket ticket in tickets)
			{
				var response = _mapper.Map<TicketResponse>(ticket);
				var renter = _mapper.Map<AccountResponse>(await _accountRepository.GetByIdAsync(ticket.RenterId));
				var order = _mapper.Map<OrderResponse>(await _orderRepository.GetByIdAsync(ticket.OrderId));
				
				AccountResponse? owner = null;
				if (ticket.OwnerId.HasValue)
				{
					owner = _mapper.Map<AccountResponse>(await _accountRepository.GetByIdAsync(ticket.OwnerId.Value));
				}
				
				response.Renter = renter;
				response.Owner = owner;
				response.Order = order;
				responses.Add(response);
			}
			
			return responses;
		}

		public async Task<TicketResponse?> GetByIdAsync(int id)
		{
			var ticket = await _ticketRepository.GetByIdAsync(id);
			if (ticket == null) return null;
			
			var response = _mapper.Map<TicketResponse>(ticket);
			var renter = _mapper.Map<AccountResponse>(await _accountRepository.GetByIdAsync(ticket.RenterId));
			var order = _mapper.Map<OrderResponse>(await _orderRepository.GetByIdAsync(ticket.OrderId));
			
			AccountResponse? owner = null;
			if (ticket.OwnerId.HasValue)
			{
				owner = _mapper.Map<AccountResponse>(await _accountRepository.GetByIdAsync(ticket.OwnerId.Value));
			}
			
			response.Renter = renter;
			response.Owner = owner;
			response.Order = order;
			
			return response;
		}

		public async Task UpdateAsync(UpdateTicketRequest request)
		{
			var ticket = await _ticketRepository.GetByIdAsync(request.TicketId);
			if (ticket == null) 
				throw new KeyNotFoundException($"Ticket with ID {request.TicketId} not found.");
			
			ticket.Status = request.Status;
			ticket.Response = request.Response;
			ticket.RespondedAt = DateTime.UtcNow;
			
			await _ticketRepository.UpdateAsync(ticket);
		}

		public async Task DeleteAsync(int id)
		{
			await _ticketRepository.DeleteAsync(id);
		}
	}
}
