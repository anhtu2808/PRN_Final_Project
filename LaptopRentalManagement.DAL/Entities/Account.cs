using System.ComponentModel.DataAnnotations;

namespace LaptopRentalManagement.DAL.Entities;

public partial class Account
{
    public int AccountId { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Please input valid email")]
    [Display(Name = "Email")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Length must be greater than 6")]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string PasswordHash { get; set; } = null!;

    public string Role { get; set; } = null!;
    [Required(ErrorMessage = "Full name is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Length must be greater than 2 and lower than 100")]
    [Display(Name = "Full Name")]
    public string? Name { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<Order> OrderOwners { get; set; } = new List<Order>();

    public virtual ICollection<Order> OrderRenters { get; set; } = new List<Order>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<Laptop> Laptops { get; set; } = new List<Laptop>();
}