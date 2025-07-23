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
    private readonly IMapper _mapper;
    private readonly IAccountRepository _accountRepository;
    private readonly ISlotRespository _slotRespository;
    public LaptopService(ILaptopRepository laptopRepository, IMapper mapper, IAccountRepository accountRepository, ISlotRespository slotRespository)
    {
        _mapper = mapper;
        _laptopRepository = laptopRepository;
        _accountRepository = accountRepository;
        _slotRespository = slotRespository;
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
        var slotResponse = _mapper.Map<List<SlotResponse>>(await _slotRespository.GetByLaptopId(laptops.LaptopId));

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