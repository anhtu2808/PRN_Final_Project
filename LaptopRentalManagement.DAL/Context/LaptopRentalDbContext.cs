using LaptopRentalManagement.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace LaptopRentalManagement.DAL.Context;

public partial class LaptopRentalDbContext : DbContext
{
    public LaptopRentalDbContext()
    {
    }

    public LaptopRentalDbContext(DbContextOptions<LaptopRentalDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Laptop> Laptops { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<OrderLog> OrderLogs { get; set; }
    public virtual DbSet<OrderLogImg> OrderLogImgs { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Slot> Slots { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Account__349DA5A6B038BC0D");

            entity.ToTable("Account");

            entity.HasIndex(e => e.Email, "UQ__Account__A9D10534465320BF").IsUnique();
            entity.HasMany(a => a.Laptops)
                .WithOne(l => l.Account)
                .HasForeignKey(l => l.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Laptop_Account");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.Role).HasMaxLength(20);
        });

        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.BrandId).HasName("PK__Brand__DAD4F05EA61E0654");

            entity.ToTable("Brand");

            entity.HasIndex(e => e.Name, "UQ__Brand__737584F6D23B21A2").IsUnique();

            entity.Property(e => e.Country).HasMaxLength(100);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.LogoUrl).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysutcdatetime())");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__19093A0B11FB5713");

            entity.ToTable("Category");

            entity.HasIndex(e => e.Name, "UQ__Category__737584F6123D8C29").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Laptop>(entity =>
        {
            entity.HasKey(e => e.LaptopId).HasName("PK__Laptop__19F02684D2D04B06");

            entity.ToTable("Laptop");

            entity.HasIndex(e => e.BrandId, "ix_Laptop_BrandId");
            entity.HasIndex(e => e.AccountId, "ix_Laptop_AccountId");
            entity.Property(e => e.Cpu)
                .HasMaxLength(100)
                .HasColumnName("CPU");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.ImageUrl).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.PricePerDay).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Ram).HasColumnName("RAM");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("PendingApproval");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Brand).WithMany(p => p.Laptops)
                .HasForeignKey(d => d.BrandId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Laptop_Brand");
            entity.HasOne(d => d.Account)
                .WithMany(a => a.Laptops)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull) // hoặc Cascade tuỳ ý
                .HasConstraintName("fk_Laptop_Account");

            entity.HasMany(d => d.Categories).WithMany(p => p.Laptops)
                .UsingEntity<Dictionary<string, object>>(
                    "LaptopCategory",
                    r => r.HasOne<Category>().WithMany()
                        .HasForeignKey("CategoryId")
                        .HasConstraintName("fk_LaptopCategory_Category"),
                    l => l.HasOne<Laptop>().WithMany()
                        .HasForeignKey("LaptopId")
                        .HasConstraintName("fk_LaptopCategory_Laptop"),
                    j =>
                    {
                        j.HasKey("LaptopId", "CategoryId").HasName("pk_LaptopCategory");
                        j.ToTable("LaptopCategory");
                    });
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK__Notifica__20CF2E127411AEBF");

            entity.ToTable("Notification");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Payload).HasMaxLength(1000);
            entity.Property(e => e.Type).HasMaxLength(50);

            entity.HasOne(d => d.Account).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("fk_Notification_Account");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Order__C3905BCFAA91515F");

            entity.ToTable("Order");

            entity.HasIndex(e => e.LaptopId, "ix_Order_LaptopId");

            entity.HasIndex(e => e.OwnerId, "ix_Order_OwnerId");

            entity.HasIndex(e => e.RenterId, "ix_Order_RenterId");

            entity.HasIndex(e => e.StartDate, "ix_Order_StartDate");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Pending");
            entity.Property(e => e.TotalCharge).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Laptop).WithMany(p => p.Orders)
                .HasForeignKey(d => d.LaptopId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Order_Laptop");

            entity.HasOne(d => d.Owner).WithMany(p => p.OrderOwners)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Order_Owner");

            entity.HasOne(d => d.Renter).WithMany(p => p.OrderRenters)
                .HasForeignKey(d => d.RenterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Order_Renter");
        });

        modelBuilder.Entity<OrderLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_OrderLog");
            entity.Property(e => e.Content)
                .HasMaxLength(1000);
            entity.Property(e => e.OldStatus)
                .HasMaxLength(50);
            entity.Property(e => e.NewStatus)
                .HasMaxLength(50);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(e => e.Order)
                .WithMany(o => o.OrderLogs)
                .HasForeignKey(e => e.OrderId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_OrderLog_Order");
        });

        modelBuilder.Entity<OrderLogImg>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_OrderLogImg");

            entity.Property(e => e.ImgUrl)
                .HasMaxLength(500);

            entity.HasOne(e => e.OrderLog)
                .WithMany(l => l.OrderLogImgs)
                .HasForeignKey(e => e.OrderLogId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_OrderLogImg_OrderLog");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PK__Review__74BC79CEB4CFF5C5");

            entity.ToTable("Review");

            entity.Property(e => e.Comment).HasMaxLength(1000);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Order).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("fk_Review_Order");

            entity.HasOne(d => d.Rater).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.RaterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Review_Rater");
        });

        modelBuilder.Entity<Slot>(entity =>
        {
            entity.HasKey(e => e.SlotId).HasName("PK__Slot__0A124AAFFF5C6A81");

            entity.ToTable("Slot");

            entity.HasIndex(e => new { e.LaptopId, e.SlotDate, e.Status }, "ix_Slot_LaptopDate_Status");

            entity.HasIndex(e => new { e.LaptopId, e.SlotDate }, "uc_Slot_LaptopDate").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Available");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Laptop).WithMany(p => p.Slots)
                .HasForeignKey(d => d.LaptopId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Slot_Laptop");

            entity.HasOne(d => d.Order).WithMany(p => p.Slots)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_Slot_Order");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}