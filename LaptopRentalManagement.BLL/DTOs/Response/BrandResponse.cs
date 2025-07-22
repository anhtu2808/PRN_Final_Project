namespace LaptopRentalManagement.BLL.DTOs.Response;

public class BrandResponse
{
    public int BrandId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? LogoUrl { get; set; }
    public string? Country { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int LaptopCount { get; set; }

}