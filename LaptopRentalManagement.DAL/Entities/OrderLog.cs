using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopRentalManagement.DAL.Entities
{
    public class OrderLog
    {
        public int Id { get; set; }

        public string Content { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public int OrderId { get; set; }

        public virtual Order Order { get; set; } = null!;

        public virtual ICollection<OrderLogImg> OrderLogImgs { get; set; } = new List<OrderLogImg>();
    }
}
