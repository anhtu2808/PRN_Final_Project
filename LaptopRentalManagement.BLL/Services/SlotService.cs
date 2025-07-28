using AutoMapper;
using LaptopRentalManagement.BLL.DTOs.Request;
using LaptopRentalManagement.BLL.DTOs.Response;
using LaptopRentalManagement.BLL.Interfaces;
using LaptopRentalManagement.DAL.Entities;
using LaptopRentalManagement.DAL.Interfaces;
using LaptopRentalManagement.Model.DTOs.Request;


namespace LaptopRentalManagement.BLL.Services
{
    public class SlotService : ISlotService
    {
        private readonly ISlotRespository _slotRepository;
        private readonly IMapper _mapper;
        public SlotService(ISlotRespository slotRepository, IMapper mapper)
        {
            _slotRepository = slotRepository;
            _mapper = mapper;
        }
        public async Task CreateAsync(CreateSlotRequest request)
        {
            for (var i = request.StartDate; i <= request.EndDate; i = i.AddDays(1))
            {
                Slot slot = _mapper.Map<Slot>(request);
                slot.SlotDate = DateOnly.FromDateTime(i);
                await _slotRepository.CreateAsync(slot);
            }
        }

        public async Task<IList<SlotResponse>> GetAllAsync(SlotFilter slotFilter)
        {
            return _mapper.Map<IList<SlotResponse>>(await _slotRepository.GetAllAsync(slotFilter));
        }
        
        public async Task<List<SlotResponse>> GetByIdsAsync(List<int> slotIds)
        {
            var slots = await _slotRepository.GetByIdsAsync(slotIds);
            return _mapper.Map<List<SlotResponse>>(slots);
        }
    }
}
