using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopRentalManagement.Model.DTOs.Request
{
    public class UpdateBrandRequest
    {
        [Required(ErrorMessage = "Brand ID is required")]
        public int BrandId { get; set; }

        [Required(ErrorMessage = "Brand name is required")]
        [StringLength(100, ErrorMessage = "Brand name cannot exceed 100 characters")]
        public string Name { get; set; } = null!;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }

        [Url(ErrorMessage = "Invalid URL format")]
        public string? LogoUrl { get; set; }

        [StringLength(100, ErrorMessage = "Country cannot exceed 100 characters")]
        public string? Country { get; set; }
    }
}
