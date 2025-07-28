using LaptopRentalManagement.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopRentalManagement.BLL.DTOs.Response
{
	public class TicketResponse
	{
		public int TicketId { get; set; }
		public OrderResponse Order { get; set; }

		public AccountResponse Renter { get; set; }
		public AccountResponse Owner { get; set; }

		public string Content { get; set; }   
		public TicketStatus Status { get; set; }
		public DateTime CreatedAt { get; set; } 
		public DateTime? RespondedAt { get; set; }    
		public string? Response { get; set; } 

	}
}
