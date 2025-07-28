using System.ComponentModel.DataAnnotations;

namespace LaptopRentalManagement.DAL.Entities;

public partial class Account
{
    public int AccountId { get; set; }
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string Role { get; set; } = null!;
    public string? Name { get; set; }
    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<Order> OrderOwners { get; set; } = new List<Order>();

    public virtual ICollection<Order> OrderRenters { get; set; } = new List<Order>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<Laptop> Laptops { get; set; } = new List<Laptop>();

	public virtual ICollection<Ticket> OwnerTickets { get; set; } = new List<Ticket>();

	public virtual ICollection<Ticket> RenterTickets { get; set; } = new List<Ticket>();
}