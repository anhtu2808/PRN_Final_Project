using AutoMapper;
using LaptopRentalManagement.BLL.DTOs.Request;
using LaptopRentalManagement.BLL.DTOs.Response;
using LaptopRentalManagement.BLL.Interfaces;
using LaptopRentalManagement.DAL.Entities;
using LaptopRentalManagement.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopRentalManagement.BLL.Services
{
    public class OrderLogService : IOrderLogService
    {
        private readonly IOrderLogRepository _orderLogRepository;
        private readonly IOrderLogImgRepository _orderLogImgRepository;
        private readonly IMapper _mapper;

        public OrderLogService(IOrderLogRepository orderLogRepository, IMapper mapper, IOrderLogImgRepository orderLogImgRepository)
        {
            _orderLogRepository = orderLogRepository;
            _mapper = mapper;
            _orderLogImgRepository = orderLogImgRepository;
        }

        public async Task<OrderLogResponse> CreateAsync(CreateOrderLogRequest request, List<string>? imgURL)
        {
            OrderLog orderLog = _mapper.Map<OrderLog>(request);
            orderLog = await _orderLogRepository.CreateAsync(orderLog);

            if(imgURL != null)
            {
                foreach(string img in imgURL)
                {
                    OrderLogImg logImg = new()
                    {
                        ImgUrl = img,
                        OrderLogId = orderLog.Id
                    };
                    await _orderLogImgRepository.CreateAsync(logImg);
                }
            }

            return _mapper.Map<OrderLogResponse>(orderLog);
        }

        public async Task<IList<OrderLogResponse>> GetByOrderIdAsync(int orderId)
        {
            IList<OrderLog> logs = await _orderLogRepository.GetByOrderId(orderId);
            IList<OrderLogResponse> responses = new List<OrderLogResponse>();

            foreach(OrderLog log in logs)
            {
                OrderLogResponse response = _mapper.Map<OrderLogResponse>(log);
                IList<string>? imgs = await _orderLogImgRepository.GetByLogId(log.Id);
                response.LogImg = imgs;
                responses.Add(response);
            }
            return responses;
        }
    }
}
