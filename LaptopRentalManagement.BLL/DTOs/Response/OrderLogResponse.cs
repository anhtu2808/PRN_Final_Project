using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopRentalManagement.BLL.DTOs.Response
{
    public class OrderLogResponse
    {

        public int Id { get; set; }

        public string Content { get; set; }

        public string OldStatus { get; set; } 

        public string NewStatus { get; set; }

        public DateTime CreatedAt { get; set; }

        public IList<string>? LogImg { get; set; }

    }
}
