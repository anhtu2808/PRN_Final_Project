using System;
using System.Collections.Generic;

namespace LaptopRentalManagement.DAL.LaptopRentalManagement.DAL.Entities;

public partial class Notification
{
    public int NotificationId { get; set; }

    public int AccountId { get; set; }

    public string Type { get; set; } = null!;

    public string? Payload { get; set; }

    public bool IsRead { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Account Account { get; set; } = null!;
}
