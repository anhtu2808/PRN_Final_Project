namespace LaptopRentalManagement.DAL.Entities;

public partial class Order
{
    public int OrderId { get; set; }

    public string Status { get; set; } = null!;

    public decimal TotalCharge { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public int OwnerId { get; set; }

    public int? RenterId { get; set; }

    public int LaptopId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Laptop Laptop { get; set; } = null!;

    public virtual Account Owner { get; set; } = null!;

    public virtual Account? Renter { get; set; }

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<Slot> Slots { get; set; } = new List<Slot>();
    public virtual ICollection<OrderLog> OrderLogs { get; set; } = new List<OrderLog>();

	public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
