using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopRentalManagement.BLL.DTOs.Response
{
    public class AccountDetailResponse
    {
        public int AccountId { get; set; }

        public string Email { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;

        public string Role { get; set; } = null!;

        public string? Name { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
