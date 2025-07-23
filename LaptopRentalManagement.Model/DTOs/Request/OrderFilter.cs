using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopRentalManagement.Model.DTOs.Request
{
	public class OrderFilter
	{
		public string? Status { get; set; }

		public decimal? TotalCharge { get; set; }

		public DateOnly? StartDate { get; set; }

		public DateOnly? EndDate { get; set; }

		public int? OwnerId { get; set; }

		public int? RenterId { get; set; }

		public int? LaptopId { get; set; }
	}
}
