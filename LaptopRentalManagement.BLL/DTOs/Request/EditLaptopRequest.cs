using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace LaptopRentalManagement.BLL.DTOs.Request;

public class EditLaptopRequest
{
    [Required]
    public int LaptopId { get; set; }
    
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? BrandId { get; set; }
    public int? AccountId { get; set; }
    public decimal? PricePerDay { get; set; }
    public string? Cpu { get; set; }
    public int? Ram { get; set; }
    public int? Storage { get; set; }
    public List<int>? CategoryIds { get; set; }
    public IFormFile? ImageFile { get; set; }
}