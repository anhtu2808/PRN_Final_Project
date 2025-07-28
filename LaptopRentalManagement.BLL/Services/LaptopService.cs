using AutoMapper;
using LaptopRentalManagement.BLL.DTOs.Request;
using LaptopRentalManagement.BLL.DTOs.Response;
using LaptopRentalManagement.BLL.Interfaces;
using LaptopRentalManagement.DAL.Entities;
using LaptopRentalManagement.DAL.Interfaces;
using LaptopRentalManagement.DAL.Repositories;
using LaptopRentalManagement.Model.DTOs.Request;

namespace LaptopRentalManagement.BLL.Services;

public class LaptopService : ILaptopService
{
    private readonly ILaptopRepository _laptopRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;
    private readonly IAccountRepository _accountRepository;
    private readonly ISlotRespository _slotRespository;
    private readonly IFileUploadService _fileUploadService;
    private readonly IBrandRepository _brandRepository;

    public LaptopService(
        ILaptopRepository laptopRepository,
        IMapper mapper,
        IAccountRepository accountRepository,
        ISlotRespository slotRespository,
        ICategoryRepository categoryRepository,
        IFileUploadService fileUploadService,
        IBrandRepository brandRepository
    )
    {
        _mapper = mapper;
        _laptopRepository = laptopRepository;
        _accountRepository = accountRepository;
        _slotRespository = slotRespository;
        _categoryRepository = categoryRepository;
        _fileUploadService = fileUploadService;
        _brandRepository = brandRepository;
    }

    public async Task<IList<LaptopResponse>> GetAllAsync(LaptopFilter filter)
    {
        var laptops = await _laptopRepository.GetAllAsync(filter);
        var response = _mapper.Map<IList<LaptopResponse>>(laptops);
        return response;
    }

    public async Task<LaptopResponse?> GetByIdAsync(int id)
    {
        var laptops = await _laptopRepository.GetByIdAsync(id);
        var ownerResponse = _mapper.Map<AccountResponse>(await _accountRepository.GetByIdAsync(laptops.AccountId));
		var slotResponse = _mapper.Map<List<SlotResponse>>(await _slotRespository.GetAllAsync(new() { LaptopId = laptops.LaptopId}));

		var response = _mapper.Map<LaptopResponse?>(laptops);
        response.Owner = ownerResponse;
        response.Slots = slotResponse;
        return response;
    }

    public async Task<IList<LaptopResponse>> GetTopRentedLaptopsAsync(int top = 3)
    {
        var laptops = await _laptopRepository.GetTopRentedLaptopsAsync(top);
        var response = _mapper.Map<IList<LaptopResponse>>(laptops);
        return response;
    }

    public async Task<LaptopResponse> CreateAsync(CreateLaptopRequest request)
    {
        // Handle image upload if provided
        string? imageUrl = null;
        if (request.ImageFile != null && request.ImageFile.Length > 0)
        {
            imageUrl = await _fileUploadService.UploadImageAsync(request.ImageFile, "laptops");
        }

        // Map the basic properties
        var laptop = _mapper.Map<Laptop>(request);
        laptop.BrandId = request.BrandId;
        laptop.AccountId = request.AccountId;
        laptop.ImageUrl = imageUrl; // Set the uploaded image URL

        var categories = new List<Category>();
        if (request.CategoryIds != null)
        {
            categories = await _categoryRepository.GetByIds(request.CategoryIds);
        }

        laptop.Categories = categories;

        // Save laptop (repository must Add + SaveChangesAsync)
        var created = await _laptopRepository.CreateAsync(laptop);
        return _mapper.Map<LaptopResponse>(created);
    }


    public async Task<LaptopResponse> UpdateAsync(EditLaptopRequest request)
    {
        var laptop = await _laptopRepository.GetByIdAsync(request.LaptopId);
        if (laptop == null)
        {
            throw new KeyNotFoundException($"Laptop with ID {request.LaptopId} not found.");
        }

        // Handle image upload if provided
        string? imageUrl = null;
        if (request.ImageFile != null && request.ImageFile.Length > 0)
        {
            imageUrl = await _fileUploadService.UploadImageAsync(request.ImageFile, "laptops");
        }

        _mapper.Map(request, laptop);
        laptop.ImageUrl = imageUrl ?? laptop.ImageUrl;

        var categories = new List<Category>();
        var brand = new Brand();
        if (request.CategoryIds != null)
        {
            categories = await _categoryRepository.GetByIds(request.CategoryIds);
        }

        if (request.BrandId != null)
        {
            brand = await _brandRepository.GetByIdAsync(request.BrandId.Value);
        }

        laptop.Categories = categories;
        laptop.Brand = brand;
        var updatedLaptop = await _laptopRepository.UpdateAsync(laptop);
        var response = _mapper.Map<LaptopResponse>(updatedLaptop);
        return response;
    }

    public async Task DeleteAsync(int id)
    {
        await _laptopRepository.DeleteAsync(id);
    }
}