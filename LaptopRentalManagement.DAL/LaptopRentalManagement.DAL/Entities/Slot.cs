using System;
using System.Collections.Generic;

namespace LaptopRentalManagement.DAL.LaptopRentalManagement.DAL.Entities;

public partial class Slot
{
    public int SlotId { get; set; }

    public int LaptopId { get; set; }

    public DateOnly SlotDate { get; set; }

    public string Status { get; set; } = null!;

    public int? OrderId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Laptop Laptop { get; set; } = null!;

    public virtual Order? Order { get; set; }
}
