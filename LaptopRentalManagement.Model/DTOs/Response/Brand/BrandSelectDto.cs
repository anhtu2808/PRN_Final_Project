using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopRentalManagement.Model.DTOs.Response.Brand
{
    public class BrandSelectDto
    {
        public int BrandId { get; set; }
        public string Name { get; set; } = null!;
        public string? LogoUrl { get; set; }
        public string? Country { get; set; }
    }
}
