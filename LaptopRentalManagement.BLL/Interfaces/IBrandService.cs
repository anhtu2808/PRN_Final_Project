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
        Task<IEnumerable<BrandResponse>> GetAllBrandsAsync();
        Task<BrandResponse?> GetBrandByIdAsync(int id);
        Task<BrandResponse> CreateBrandAsync(CreateBrandRequest request);
        Task<BrandResponse> UpdateBrandAsync(UpdateBrandRequest request);
        Task<bool> DeleteBrandAsync(int id);
        Task<IEnumerable<BrandSelectDto>> GetBrandsForSelectAsync();
        Task<IEnumerable<BrandResponse>> SearchBrandsAsync(string searchTerm);
    }
}
