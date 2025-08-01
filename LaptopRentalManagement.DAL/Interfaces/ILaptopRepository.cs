using LaptopRentalManagement.DAL.Entities;
using LaptopRentalManagement.Model.DTOs.Request;


namespace LaptopRentalManagement.DAL.Interfaces;

public interface ILaptopRepository
{
    Task<List<Laptop>> GetAllAsync(LaptopFilter filter);
    Task<Laptop?> GetByIdAsync(int id);
    Task<List<Laptop>> GetTopRentedLaptopsAsync(int top = 3);
    Task<Laptop> CreateAsync(Laptop laptop);
    Task<Laptop> UpdateAsync(Laptop laptop);
    Task DeleteAsync(int id);
}