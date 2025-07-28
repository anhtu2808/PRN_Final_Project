using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopRentalManagement.Model.DTOs.Request
{
    public class SlotFilter
    {
        public int? LaptopId { get; set; }

        public int? Month { get; set; }

        public int? Year { get; set; }

        public string? Status { get; set; } 

    }
}
