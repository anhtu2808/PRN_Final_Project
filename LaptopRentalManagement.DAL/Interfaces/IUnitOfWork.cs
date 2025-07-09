using LaptopRentalManagement.DAL.Entities;

namespace LaptopRentalManagement.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Account> AccountRepository { get; }
        IGenericRepository<Laptop> LaptopRepository { get; }
        IGenericRepository<Order> OrderRepository { get; }
        IGenericRepository<Brand> BrandRepository { get; }
        IGenericRepository<Category> CategoryRepository { get; }
        IGenericRepository<Review> ReviewRepository { get; }
        IGenericRepository<Slot> SlotRepository { get; }
        IGenericRepository<Notification> NotificationRepository { get; }
        
        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
} 