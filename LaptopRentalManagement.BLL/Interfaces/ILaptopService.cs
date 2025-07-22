using LaptopRentalManagement.BLL.DTOs.Request;
using LaptopRentalManagement.BLL.DTOs.Response;
using LaptopRentalManagement.DAL.Entities;
using LaptopRentalManagement.Model.DTOs.Request;

namespace LaptopRentalManagement.BLL.Interfaces;

public interface ILaptopService
{
    Task<IList<LaptopResponse>> GetAllAsync(LaptopFilter filter);
    Task<LaptopResponse?> GetByIdAsync(int id);
    Task<LaptopResponse> CreateAsync(CreateLaptopRequest request);
    Task<LaptopResponse> UpdateAsync(EditLaptopRequest request);
    Task DeleteAsync(int id);
}