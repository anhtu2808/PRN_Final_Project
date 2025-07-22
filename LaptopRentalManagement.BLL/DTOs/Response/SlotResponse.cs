using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopRentalManagement.BLL.DTOs.Response
{
    public class SlotResponse
    {
        public int SlotId { get; set; }

        public LaptopResponse? Laptop { get; set; }

        public DateOnly SlotDate { get; set; }

        public string Status { get; set; } = null!;

        public OrderResponse? Order { get; set; }

    }
}
