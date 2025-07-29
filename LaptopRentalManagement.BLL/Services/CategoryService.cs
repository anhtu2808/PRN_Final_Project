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
            // Check if category name already exists
            if (await _categoryRepository.ExistsByNameAsync(request.Name))
            {
                throw new InvalidOperationException($"A category with name '{request.Name}' already exists.");
            }

            var category = _mapper.Map<Category>(request);
            var createdCategory = await _categoryRepository.CreateAsync(category);

            var dto = _mapper.Map<CategoryResponse>(createdCategory);
            dto.LaptopCount = 0;
            return dto;
        }

        public async Task<CategoryResponse> UpdateCategoryAsync(UpdateCategoryRequest request)
        {
            // Check if category exists
            var existingCategory = await _categoryRepository.GetByIdAsync(request.CategoryId);
            if (existingCategory == null)
            {
                throw new ArgumentException($"Category with ID {request.CategoryId} not found.");
            }

            // Check if category name already exists (excluding current one)
            if (await _categoryRepository.ExistsByNameAsync(request.Name, request.CategoryId))
            {
                throw new InvalidOperationException($"A category with name '{request.Name}' already exists.");
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
            // Check if the category can be deleted
            if (!await CanDeleteCategoryAsync(id))
            {
                throw new InvalidOperationException("Cannot delete this category because laptops are assigned to it.");
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