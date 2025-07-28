using System.Security.Cryptography;
using System.Text;

namespace LaptopRentalManagement.BLL.Helpers;

public static class HmacHelper
{
    public static string Compute(string key, string data)
    {
        using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key)))
        {
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
}