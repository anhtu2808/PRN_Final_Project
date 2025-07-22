namespace LaptopRentalManagement.DAL.Entities;

public partial class Brand
{
    public int BrandId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? LogoUrl { get; set; }

    public string? Country { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<Laptop> Laptops { get; set; } = new List<Laptop>();
}
