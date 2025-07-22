namespace LaptopRentalManagement.BLL.DTOs.Response;

public class AccountResponse
{
    public int AccountId { get; set; }
    public string Email { get; set; } = null!;
    public string Role { get; set; } = null!;
    public string? Name { get; set; }
    public DateTime CreatedAt { get; set; }
}