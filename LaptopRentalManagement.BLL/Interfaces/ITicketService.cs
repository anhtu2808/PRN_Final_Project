using LaptopRentalManagement.BLL.DTOs.Request;
using LaptopRentalManagement.BLL.DTOs.Response;
using LaptopRentalManagement.Model.DTOs.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopRentalManagement.BLL.Interfaces
{
	public interface ITicketService
	{
		Task CreateTicketAsync(CreateTicketRequest request);

		Task<IList<TicketResponse>> GetAllByOrderIdAsync(int id);
	}
}
