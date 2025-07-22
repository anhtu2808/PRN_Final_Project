using AutoMapper;
using LaptopRentalManagement.BLL.DTOs.Request;
using LaptopRentalManagement.BLL.DTOs.Response;
using LaptopRentalManagement.BLL.Interfaces;
using LaptopRentalManagement.DAL.Entities;
using LaptopRentalManagement.DAL.Interfaces;

namespace LaptopRentalManagement.BLL.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryResponse>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            var result = new List<CategoryResponse>();

            foreach (var category in categories)
            {
                var dto = _mapper.Map<CategoryResponse>(category);
                dto.LaptopCount = category.Laptops?.Count ?? 0;
                result.Add(dto);
            }

            return result;
        }

        public async Task<IEnumerable<CategorySelectDto>> GetCategoriesForSelectAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CategorySelectDto>>(categories);
        }

        public async Task<CategoryResponse?> GetCategoryByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
                return null;

            var dto = _mapper.Map<CategoryResponse>(category);
            dto.LaptopCount = category.Laptops?.Count ?? 0;
            return dto;
        }

        public async Task<CategoryResponse?> GetCategoryByNameAsync(string name)
        {
            var category = await _categoryRepository.GetByNameAsync(name);
            if (category == null)
                return null;

            var dto = _mapper.Map<CategoryResponse>(category);
            dto.LaptopCount = category.Laptops?.Count ?? 0;
            return dto;
        }

        public async Task<CategoryResponse> CreateCategoryAsync(CreateCategoryRequest request)
        {
            // Kiểm tra tên category đã tồn tại chưa
            if (await _categoryRepository.ExistsByNameAsync(request.Name))
            {
                throw new InvalidOperationException($"Danh mục với tên '{request.Name}' đã tồn tại.");
            }

            var category = _mapper.Map<Category>(request);
            var createdCategory = await _categoryRepository.CreateAsync(category);

            var dto = _mapper.Map<CategoryResponse>(createdCategory);
            dto.LaptopCount = 0;
            return dto;
        }

        public async Task<CategoryResponse> UpdateCategoryAsync(UpdateCategoryRequest request)
        {
            // Kiểm tra category có tồn tại không
            var existingCategory = await _categoryRepository.GetByIdAsync(request.CategoryId);
            if (existingCategory == null)
            {
                throw new ArgumentException($"Không tìm thấy danh mục với ID {request.CategoryId}");
            }

            // Kiểm tra tên category đã tồn tại chưa (trừ chính nó)
            if (await _categoryRepository.ExistsByNameAsync(request.Name, request.CategoryId))
            {
                throw new InvalidOperationException($"Danh mục với tên '{request.Name}' đã tồn tại.");
            }

            // Cập nhật thông tin
            existingCategory.Name = request.Name;
            existingCategory.Description = request.Description;

            var updatedCategory = await _categoryRepository.UpdateAsync(existingCategory);

            var dto = _mapper.Map<CategoryResponse>(updatedCategory);
            dto.LaptopCount = updatedCategory.Laptops?.Count ?? 0;
            return dto;
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            // Kiểm tra có thể xóa được không
            if (!await CanDeleteCategoryAsync(id))
            {
                throw new InvalidOperationException("Không thể xóa danh mục này vì đang có laptop thuộc danh mục.");
            }

            return await _categoryRepository.DeleteAsync(id);
        }

        public async Task<bool> CategoryExistsByNameAsync(string name, int? excludeId = null)
        {
            return await _categoryRepository.ExistsByNameAsync(name, excludeId);
        }

        public async Task<bool> CanDeleteCategoryAsync(int id)
        {
            var laptopCount = await _categoryRepository.GetLaptopCountByCategoryAsync(id);
            return laptopCount == 0;
        }
    }
}