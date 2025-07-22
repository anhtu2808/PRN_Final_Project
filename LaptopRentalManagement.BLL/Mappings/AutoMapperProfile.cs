using AutoMapper;
using LaptopRentalManagement.BLL.DTOs.Request;
using LaptopRentalManagement.BLL.DTOs.Response;
using LaptopRentalManagement.DAL.Entities;

namespace LaptopRentalManagement.BLL.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Account mappings
            CreateMap<Account, Account>()
                .ForMember(dest => dest.AccountId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

            // Laptop mappings  
            CreateMap<Laptop, Laptop>()
                .ForMember(dest => dest.LaptopId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            // Order mappings
            CreateMap<Order, Order>()
                .ForMember(dest => dest.OrderId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            // Brand mappings
            CreateMap<Brand, Brand>()
                .ForMember(dest => dest.BrandId, opt => opt.Ignore());

            // Category mappings
            CreateMap<Category, Category>()
                .ForMember(dest => dest.CategoryId, opt => opt.Ignore());

            // Review mappings

            //CreateMap<Review, Review>()
            //    .ForMember(dest => dest.ReviewId, opt => opt.Ignore());

            // Review Entity -> ReviewResponse
            CreateMap<Review, ReviewResponse>()
                .ForMember(dest => dest.RaterName, opt => opt.MapFrom(src => src.Rater.Name ?? "Anonymous"))
                .ForMember(dest => dest.LaptopName, opt => opt.MapFrom(src => src.Order.Laptop.Name))
                .ForMember(dest => dest.LaptopImageUrl, opt => opt.MapFrom(src => src.Order.Laptop.ImageUrl))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => (int)src.Rating));

            // CreateReviewRequest -> Review Entity
            CreateMap<CreateReviewRequest, Review>()
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => (byte)src.Rating))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            // Slot mappings
            CreateMap<Slot, Slot>()
                .ForMember(dest => dest.SlotId, opt => opt.Ignore());

            // Notification mappings
            CreateMap<Notification, Notification>()
                .ForMember(dest => dest.NotificationId, opt => opt.Ignore());

            // View Model mappings (for API responses)
            CreateMap<Account, object>()
                .ForMember("Id", opt => opt.MapFrom(src => src.AccountId))
                .ForMember("Email", opt => opt.MapFrom(src => src.Email))
                .ForMember("Name", opt => opt.MapFrom(src => src.Name))
                .ForMember("Role", opt => opt.MapFrom(src => src.Role));

            CreateMap<Laptop, object>()
                .ForMember("Id", opt => opt.MapFrom(src => src.LaptopId))
                .ForMember("Name", opt => opt.MapFrom(src => src.Name))
                .ForMember("Description", opt => opt.MapFrom(src => src.Description))
                .ForMember("PricePerDay", opt => opt.MapFrom(src => src.PricePerDay))
                .ForMember("Status", opt => opt.MapFrom(src => src.Status))
                .ForMember("BrandName", opt => opt.MapFrom(src => src.Brand.Name));

            CreateMap<Order, object>()
                .ForMember("Id", opt => opt.MapFrom(src => src.OrderId))
                .ForMember("Status", opt => opt.MapFrom(src => src.Status))
                .ForMember("TotalCharge", opt => opt.MapFrom(src => src.TotalCharge))
                .ForMember("StartDate", opt => opt.MapFrom(src => src.StartDate))
                .ForMember("EndDate", opt => opt.MapFrom(src => src.EndDate))
                .ForMember("LaptopName", opt => opt.MapFrom(src => src.Laptop.Name))
                .ForMember("RenterName", opt => opt.MapFrom(src => src.Renter.Name));
        }
    }
} 