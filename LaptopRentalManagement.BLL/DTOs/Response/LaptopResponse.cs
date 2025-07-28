namespace LaptopRentalManagement.BLL.DTOs.Response;

public class LaptopResponse
{
    public int LaptopId { get; set; }
    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;
    public BrandResponse Brand { get; set; } = null!;
    public List<CategoryResponse> Categories { get; set; } = new();
    public AccountResponse Owner { get; set; } = null!;

    public string ImageURL { get; set; } =
        "https://images.unsplash.com/photo-1541807084-5c52b6b3adef?w=400&h=300&fit=crop";

    public decimal PricePerDay { get; set; }
    public decimal Deposit { get; set; }
    public string Cpu { get; set; } = null!;
    public int Ram { get; set; }
    public int Storage { get; set; }
    public string Status { get; set; } = null!;
    public DateTime CreatedAt { get; set; }

    public IList<SlotResponse>? Slots { get; set; }
}