using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace LaptopRentalManagement.BLL.DTOs.Request;

public class CreateLaptopRequest
{
    [Required] 
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public IFormFile? ImageFile { get; set; }

    [Required] 
    public int BrandId { get; set; }

    [Required] 
    public int AccountId { get; set; }
    
    public List<int> CategoryIds { get; set; } = new();

    [Required] 
    public decimal PricePerDay { get; set; }

    [Required] 
    public decimal Deposit { get; set; }

    [Required] 
    public string Cpu { get; set; } = null!;

    [Required] 
    public int Ram { get; set; }

    [Required] 
    public int Storage { get; set; }
    
    public string? Status { get; set; }
}