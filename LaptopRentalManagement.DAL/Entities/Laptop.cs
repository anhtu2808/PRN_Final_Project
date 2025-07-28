namespace LaptopRentalManagement.DAL.Entities;

public partial class Laptop
{
    public int LaptopId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal PricePerDay { get; set; }

    public decimal Deposit { get; set; }

    public int BrandId { get; set; }

    public string Cpu { get; set; } = null!;

    public int Ram { get; set; }

    public int Storage { get; set; }

    public string? ImageUrl { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int AccountId { get; set; }

    public virtual Brand Brand { get; set; } = null!;


    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Slot> Slots { get; set; } = new List<Slot>();

    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();

    public virtual Account Account { get; set; } = null!;
}