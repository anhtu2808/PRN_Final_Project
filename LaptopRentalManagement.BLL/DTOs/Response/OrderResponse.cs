using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopRentalManagement.BLL.DTOs.Response
{
	public class OrderResponse
	{
		public int OrderId { get; set; }
		public string Status { get; set; } = null!;

		public decimal TotalCharge { get; set; }
		public decimal DepositAmount { get; set; }
		public DateOnly StartDate { get; set; }

		public DateOnly EndDate { get; set; }

		public AccountResponse Owner { get; set; } = null!;

		public AccountResponse? Renter { get; set; }

		public LaptopResponse Laptop { get; set; } = null!;

        public IList<SlotResponse> Slots { get; set; }
        public string redirectUrl { get; set; } = null!;
    }
}
