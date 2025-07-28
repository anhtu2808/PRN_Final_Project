using AutoMapper;
using LaptopRentalManagement.BLL.DTOs.Request;
using LaptopRentalManagement.BLL.DTOs.Response;
using LaptopRentalManagement.BLL.DTOs.Request;
using LaptopRentalManagement.BLL.DTOs.Response;
using LaptopRentalManagement.DAL.Entities;
using LaptopRentalManagement.Model.DTOs.Request;
using LaptopRentalManagement.Model.DTOs.Response.Brand;

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
            // Account → AccountResponse
            CreateMap<Account, AccountResponse>();

            // Order mappings
            CreateMap<Order, Order>()
                .ForMember(dest => dest.OrderId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            // Brand mappings

            CreateMap<Brand, BrandResponse>();


            CreateMap<CreateBrandRequest, Brand>()
                .ForMember(dest => dest.BrandId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Laptops, opt => opt.Ignore());

            CreateMap<UpdateBrandRequest, Brand>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Laptops, opt => opt.Ignore());

            CreateMap<Brand, BrandSelectDto>();


            // Category mappings
            CreateMap<Category, CategoryResponse>();

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
            CreateMap<CreateReviewRequest, Review>();
            CreateMap<UpdateReviewRequest, Review>();

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

            // Order mappings
            CreateMap<CreateOrderRequest, Order>()
                .ForMember(dest => dest.StartDate, opt => opt.Ignore())
                .ForMember(dest => dest.EndDate, opt => opt.Ignore());
            CreateMap<Order, OrderResponse>()
                .ForMember(dest => dest.Owner, opt => opt.Ignore())
                .ForMember(dest => dest.Renter, opt => opt.Ignore())
                .ForMember(dest => dest.Laptop, opt => opt.Ignore());

            //Laptop mappings
            CreateMap<Laptop, LaptopResponse>()
                // map nested Brand → BrandResponse
                .ForMember(dest => dest.Brand,
                    opt => opt.MapFrom(src => src.Brand))
                // map nested Account → AccountResponse
                .ForMember(dest => dest.Owner,
                    opt => opt.MapFrom(src => src.Account))
                // map Categories collection → List<CategoryResponse>
                .ForMember(dest => dest.Categories,
                    opt => opt.MapFrom(src => src.Categories));
            CreateMap<EditLaptopRequest, Laptop>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<CreateLaptopRequest, Laptop>();
            CreateMap<Slot, SlotResponse>()
                .ForMember(dest => dest.Order, opt => opt.Ignore())
                .ForMember(dest => dest.Laptop, opt => opt.Ignore());

            CreateMap<CreateSlotRequest, Slot>()
                .ForMember(dest => dest.Order, opt => opt.Ignore())
                .ForMember(dest => dest.Laptop, opt => opt.Ignore());

            CreateMap<CreateOrderLogRequest, OrderLog>();
            CreateMap<OrderLog, OrderLogResponse>();
        }
    }
}