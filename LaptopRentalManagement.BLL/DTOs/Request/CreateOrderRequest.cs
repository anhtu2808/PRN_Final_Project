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
        public IList<int> SlotIds { get; set; }
        public decimal DepositAmount { get; set; }
        public decimal TotalCharge { get; set; }
        public int RenterId { get; set; }
    }
}