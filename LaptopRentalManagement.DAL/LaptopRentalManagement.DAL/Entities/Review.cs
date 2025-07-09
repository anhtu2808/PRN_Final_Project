using System;
using System.Collections.Generic;

namespace LaptopRentalManagement.DAL.LaptopRentalManagement.DAL.Entities;

public partial class Review
{
    public int ReviewId { get; set; }

    public int OrderId { get; set; }

    public int RaterId { get; set; }

    public byte Rating { get; set; }

    public string? Comment { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Account Rater { get; set; } = null!;
}
