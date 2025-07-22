using LaptopRentalManagement.BLL.DTOs.Request;
using LaptopRentalManagement.BLL.DTOs.Response;

namespace LaptopRentalManagement.BLL.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryResponseDto>> GetAllCategoriesAsync();
        Task<IEnumerable<CategorySelectDto>> GetCategoriesForSelectAsync();
        Task<CategoryResponseDto?> GetCategoryByIdAsync(int id);
        Task<CategoryResponseDto?> GetCategoryByNameAsync(string name);
        Task<CategoryResponseDto> CreateCategoryAsync(CreateCategoryRequest request);
        Task<CategoryResponseDto> UpdateCategoryAsync(UpdateCategoryRequest request);
        Task<bool> DeleteCategoryAsync(int id);
        Task<bool> CategoryExistsByNameAsync(string name, int? excludeId = null);
        Task<bool> CanDeleteCategoryAsync(int id);
    }
} 