using AutoMapper;
using LaptopRentalManagement.BLL.DTOs.Request;
using LaptopRentalManagement.BLL.DTOs.Response;
using LaptopRentalManagement.BLL.Interfaces;
using LaptopRentalManagement.DAL.Entities;
using LaptopRentalManagement.DAL.Interfaces;
using LaptopRentalManagement.Model.DTOs.Request;

namespace LaptopRentalManagement.BLL.Services;

public class LaptopService : ILaptopService
{
    private readonly ILaptopRepository _laptopRepository;
    private readonly IMapper _mapper;

    public LaptopService(ILaptopRepository laptopRepository, IMapper mapper)
    {
        _mapper = mapper;
        _laptopRepository = laptopRepository;
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
        var response = _mapper.Map<LaptopResponse?>(laptops);
        return response;
    }

    public Task<LaptopResponse> CreateAsync(CreateLaptopRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<LaptopResponse> UpdateAsync(EditLaptopRequest request)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }
}