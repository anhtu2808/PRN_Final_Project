using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopRentalManagement.BLL.DTOs.Request
{
	public class CreateSlotRequest
	{
		public int LaptopId { get; set; }

		public DateOnly SlotDate { get; set; }

		public string Status { get; set; } = null!;

		public int? OrderId { get; set; }
	}
}
