using AutoMapper;
using LaptopRentalManagement.DAL.Entities;
using LaptopRentalManagement.BLL.DTOs.Request;
using LaptopRentalManagement.BLL.DTOs.Response;

namespace LaptopRentalManagement.BLL.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Account mappings
            CreateMap<Account, AccountResponse>();
            CreateMap<Account, ProfileResponse>();
            CreateMap<Account, LoginResponse>()
                .ForMember(dest => dest.Token, opt => opt.Ignore())
                .ForMember(dest => dest.TokenExpiration, opt => opt.Ignore());
            CreateMap<RegisterRequest, Account>()
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.AccountId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => "Customer"));
            CreateMap<UpdateProfileRequest, Account>()
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.UtcNow));

            // Brand mappings
            CreateMap<Brand, BrandResponse>();

            // Category mappings
            CreateMap<Category, CategoryResponse>();

            // Laptop mappings
            CreateMap<Laptop, LaptopResponse>()
                .ForMember(dest => dest.AverageRating, opt => opt.MapFrom(src => 
                    src.Reviews.Any() ? src.Reviews.Average(r => r.Rating) : 0))
                .ForMember(dest => dest.ReviewCount, opt => opt.MapFrom(src => src.Reviews.Count));
            CreateMap<Laptop, LaptopDetailResponse>()
                .ForMember(dest => dest.AverageRating, opt => opt.MapFrom(src => 
                    src.Reviews.Any() ? src.Reviews.Average(r => r.Rating) : 0))
                .ForMember(dest => dest.ReviewCount, opt => opt.MapFrom(src => src.Reviews.Count));
            CreateMap<CreateLaptopRequest, Laptop>()
                .ForMember(dest => dest.LaptopId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow));
            CreateMap<UpdateLaptopRequest, Laptop>()
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.UtcNow));

            // Order mappings
            CreateMap<Order, OrderResponse>();
            CreateMap<Order, OrderDetailResponse>();
            CreateMap<CreateOrderRequest, Order>()
                .ForMember(dest => dest.OrderId, opt => opt.Ignore())
                .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Pending"));

            // Review mappings
            CreateMap<Review, ReviewResponse>()
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Account.FullName));

            // Slot mappings
            CreateMap<Slot, SlotResponse>();

            // Notification mappings
            CreateMap<Notification, object>(); // Can be customized based on notification response structure
        }
    }
} 