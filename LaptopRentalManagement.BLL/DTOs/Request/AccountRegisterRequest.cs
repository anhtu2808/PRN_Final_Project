using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopRentalManagement.BLL.DTOs.Request
{
    public class AccountRegisterRequest
    {
        [Required]
        public string Email { get; set; } = null!;
        [Required]
        public string PasswordHash { get; set; } = null!;

        public string? Role { get; set; } = null!;
        public string? Name { get; set; }
    }
}
