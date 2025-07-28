using LaptopRentalManagement.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopRentalManagement.BLL.DTOs.Response
{
    public class RentalSlotResponse
    {
		public IList<SlotResponse>? Slots { get; set; }
		public IList<DateTime>? DaysInMonth { get; set; }
	}
}
