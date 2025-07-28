using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopRentalManagement.BLL.DTOs.Request
{
	public class OrderLogRequest
	{

		public int OrderId { get; set; }
		public string NewStatus { get; set; }

		public string? Reason { get; set; }
		public IList<IFormFile>? Forms { get; set; }

	}
}
