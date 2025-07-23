using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopRentalManagement.BLL.DTOs.Request
{
	public class CreateOrderRequest
	{
		public int LaptopId { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public int TotalDate {  get; set; }
		public int OwnerId { get; set; }
	}
}
