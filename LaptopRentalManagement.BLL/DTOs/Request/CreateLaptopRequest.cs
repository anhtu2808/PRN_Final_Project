using System.ComponentModel.DataAnnotations;

namespace LaptopRentalManagement.BLL.DTOs.Request;

public class CreateLaptopRequest
{
    [Required] public string Name { get; set; } = null!;

    public string? Description { get; set; }
    [Required] public string? ImageURL { get; set; }

    [Required] public int BrandId { get; set; }

    [Required] public int AccountId { get; set; }

    public List<int> CategoryIds { get; set; } = new();

    [Required] public decimal PricePerDay { get; set; }

    [Required] public string Cpu { get; set; } = null!;

    [Required] public int Ram { get; set; }

    [Required] public int Storage { get; set; }
}