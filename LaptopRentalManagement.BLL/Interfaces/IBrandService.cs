using LaptopRentalManagement.BLL.DTOs.Response;
using LaptopRentalManagement.Model.DTOs.Request;
using LaptopRentalManagement.Model.DTOs.Response.Brand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopRentalManagement.BLL.Interfaces
{
    public interface IBrandService
    {
        Task<IEnumerable<BrandResponseDTO>> GetAllBrandsAsync();
        Task<BrandResponseDTO?> GetBrandByIdAsync(int id);
        Task<BrandResponseDTO> CreateBrandAsync(CreateBrandRequest request);
        Task<BrandResponseDTO> UpdateBrandAsync(UpdateBrandRequest request);
        Task<bool> DeleteBrandAsync(int id);
        Task<IEnumerable<BrandSelectDto>> GetBrandsForSelectAsync();
        Task<IEnumerable<BrandResponseDTO>> SearchBrandsAsync(string searchTerm);
    }
}
