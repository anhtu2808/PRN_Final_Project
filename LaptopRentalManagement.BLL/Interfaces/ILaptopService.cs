using LaptopRentalManagement.BLL.DTOs.Request;
using LaptopRentalManagement.BLL.DTOs.Response;

namespace LaptopRentalManagement.BLL.Interfaces
{
    public interface ILaptopService
    {
        Task<BaseResponse<LaptopResponse>> CreateLaptopAsync(CreateLaptopRequest request);
        Task<BaseResponse<LaptopResponse>> UpdateLaptopAsync(int laptopId, UpdateLaptopRequest request);
        Task<BaseResponse<bool>> DeleteLaptopAsync(int laptopId);
        Task<BaseResponse<LaptopDetailResponse>> GetLaptopByIdAsync(int laptopId);
        Task<BaseResponse<PagedResponse<LaptopResponse>>> SearchLaptopsAsync(LaptopSearchRequest request);
        Task<BaseResponse<List<LaptopResponse>>> GetAvailableLaptopsAsync();
        Task<BaseResponse<List<BrandResponse>>> GetAllBrandsAsync();
        Task<BaseResponse<List<CategoryResponse>>> GetAllCategoriesAsync();
        Task<BaseResponse<bool>> UpdateAvailabilityAsync(int laptopId, bool isAvailable);
        Task<BaseResponse<List<SlotResponse>>> GetAvailableSlotsAsync(int laptopId, DateTime startDate, DateTime endDate);
    }
} 