using AutoMapper;
using LaptopRentalManagement.BLL.DTOs.Request;
using LaptopRentalManagement.BLL.DTOs.Response;
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

            // Account DTO mappings
            CreateMap<Account, AccountDetailResponse>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()); // Don't expose password hash

            CreateMap<AccountRegisterRequest, Account>()
                .ForMember(dest => dest.AccountId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Notifications, opt => opt.Ignore())
                .ForMember(dest => dest.OrderOwners, opt => opt.Ignore())
                .ForMember(dest => dest.OrderRenters, opt => opt.Ignore())
                .ForMember(dest => dest.Reviews, opt => opt.Ignore());

            CreateMap<AccountUpdateRequest, Account>()
                .ForMember(dest => dest.AccountId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Notifications, opt => opt.Ignore())
                .ForMember(dest => dest.OrderOwners, opt => opt.Ignore())
                .ForMember(dest => dest.OrderRenters, opt => opt.Ignore())
                .ForMember(dest => dest.Reviews, opt => opt.Ignore());

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
            
            // Category DTO mappings
            CreateMap<CreateCategoryRequest, Category>()
                .ForMember(dest => dest.CategoryId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Laptops, opt => opt.Ignore());

            CreateMap<UpdateCategoryRequest, Category>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Laptops, opt => opt.Ignore());

            CreateMap<Category, CategoryResponse>()
                .ForMember(dest => dest.LaptopCount, opt => opt.Ignore()); // Will be set manually in service

            CreateMap<Category, CategorySelectDto>();

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

            // Note: Removed invalid object mappings that were causing AutoMapper configuration errors
            // These mappings to System.Object with ForMember string-based configuration are not supported
            // If you need view models for API responses, create specific DTO classes instead
        }
    }
} 