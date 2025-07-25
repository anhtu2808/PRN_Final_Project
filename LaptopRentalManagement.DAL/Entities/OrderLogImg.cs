using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopRentalManagement.DAL.Entities
{
    public class OrderLogImg
    {
        public int Id { get; set; }

        public int OrderLogId { get; set; }

        public string ImgUrl { get; set; } = null!;

        public virtual OrderLog OrderLog { get; set; } = null!;
    }
}
