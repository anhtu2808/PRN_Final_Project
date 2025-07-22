using AutoMapper;
using LaptopRentalManagement.BLL.Interfaces;
using LaptopRentalManagement.DAL.Interfaces;
using LaptopRentalManagement.Model.DTOs.Request;
using LaptopRentalManagement.Model.DTOs.Response.Brand;
using LaptopRentalManagement.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopRentalManagement.BLL.Services
{
   public class BrandService : IBrandService
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IMapper _mapper;

        public BrandService(IBrandRepository brandRepository, IMapper mapper)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BrandResponseDTO>> GetAllBrandsAsync()
        {
            var brands = await _brandRepository.GetAllAsync();
            var brandResponses = _mapper.Map<IEnumerable<BrandResponseDTO>>(brands);

            // Set laptop count for each brand
            foreach (var brandResponse in brandResponses)
            {
                brandResponse.LaptopCount = await _brandRepository.GetLaptopCountAsync(brandResponse.BrandId);
            }

            return brandResponses;
        }

        public async Task<BrandResponseDTO?> GetBrandByIdAsync(int id)
        {
            var brand = await _brandRepository.GetByIdWithLaptopsAsync(id);
            if (brand == null)
                return null;

            var brandResponse = _mapper.Map<BrandResponseDTO>(brand);
            brandResponse.LaptopCount = brand.Laptops.Count;

            return brandResponse;
        }

        public async Task<BrandResponseDTO> CreateBrandAsync(CreateBrandRequest request)
        {
            // Check if brand name already exists
            if (await _brandRepository.ExistsByNameAsync(request.Name))
            {
                throw new InvalidOperationException($"Brand with name '{request.Name}' already exists.");
            }

            var brand = _mapper.Map<Brand>(request);
            var createdBrand = await _brandRepository.CreateAsync(brand);

            var brandResponse = _mapper.Map<BrandResponseDTO>(createdBrand);
            brandResponse.LaptopCount = 0;

            return brandResponse;
        }

        public async Task<BrandResponseDTO> UpdateBrandAsync(UpdateBrandRequest request)
        {
            // Check if brand exists
            var existingBrand = await _brandRepository.GetByIdAsync(request.BrandId);
            if (existingBrand == null)
            {
                throw new KeyNotFoundException($"Brand with ID {request.BrandId} not found.");
            }

            // Check if brand name already exists (excluding current brand)
            if (await _brandRepository.ExistsByNameAsync(request.Name, request.BrandId))
            {
                throw new InvalidOperationException($"Brand with name '{request.Name}' already exists.");
            }

            // Map update request to existing entity
            _mapper.Map(request, existingBrand);
            var updatedBrand = await _brandRepository.UpdateAsync(existingBrand);

            var brandResponse = _mapper.Map<BrandResponseDTO>(updatedBrand);
            brandResponse.LaptopCount = await _brandRepository.GetLaptopCountAsync(updatedBrand.BrandId);

            return brandResponse;
        }

        public async Task<bool> DeleteBrandAsync(int id)
        {
            // Check if brand exists
            if (!await _brandRepository.ExistsAsync(id))
            {
                throw new KeyNotFoundException($"Brand with ID {id} not found.");
            }

            // Check if brand has laptops
            var laptopCount = await _brandRepository.GetLaptopCountAsync(id);
            if (laptopCount > 0)
            {
                throw new InvalidOperationException($"Cannot delete brand. It has {laptopCount} associated laptop(s).");
            }

            return await _brandRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<BrandSelectDto>> GetBrandsForSelectAsync()
        {
            var brands = await _brandRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<BrandSelectDto>>(brands);
        }

        public async Task<IEnumerable<BrandResponseDTO>> SearchBrandsAsync(string searchTerm)
        {
            var brands = await _brandRepository.SearchAsync(searchTerm);
            var brandResponses = _mapper.Map<IEnumerable<BrandResponseDTO>>(brands);

            // Set laptop count for each brand
            foreach (var brandResponse in brandResponses)
            {
                brandResponse.LaptopCount = await _brandRepository.GetLaptopCountAsync(brandResponse.BrandId);
            }

            return brandResponses;
        }
    }
}
