namespace LaptopRentalManagement.DAL.Entities;

public partial class Category
{
    public int CategoryId { get; set; }

    public string Name { get; set; } = null!;

    public string IconClass { get; set; } = "fas fa-tag";

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Laptop> Laptops { get; set; } = new List<Laptop>();
}