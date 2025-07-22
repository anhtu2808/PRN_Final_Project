namespace LaptopRentalManagement.Model.DTOs.Request;

public class LaptopFilter
{
    public int? CategoryId { get; set; }
    public int? AccountId { get; set; }
    public int? BrandId { get; set; }
    public string? Name { get; set; }
    public string? Cpu { get; set; }
    public int? Ram { get; set; }
    public int? Storage { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public string? Status { get; set; }
    public DateTime? CreatedFrom { get; set; }

    public DateTime? CreatedTo { get; set; }
}