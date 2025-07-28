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

    // Chat relationships
    public virtual ICollection<ChatRoom> CustomerChatRooms { get; set; } = new List<ChatRoom>();
    public virtual ICollection<ChatRoom> StaffChatRooms { get; set; } = new List<ChatRoom>();
    public virtual ICollection<ChatMessage> SentMessages { get; set; } = new List<ChatMessage>();
}