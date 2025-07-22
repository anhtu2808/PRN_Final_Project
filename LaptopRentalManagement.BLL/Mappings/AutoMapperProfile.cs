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
            // Account → AccountResponse
            CreateMap<Account, AccountResponse>();

            // Order mappings
            CreateMap<Order, Order>()
                .ForMember(dest => dest.OrderId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            // Brand mappings
          
            CreateMap<Brand, BrandResponse>()
                 .ForMember(dest => dest.BrandId, opt => opt.Ignore());
         

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

            // CreateReviewRequest -> Review Entity
            CreateMap<CreateReviewRequest, Review>()
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => (byte)src.Rating))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            // Slot mappings
            CreateMap<Slot, Slot>()
                .ForMember(dest => dest.SlotId, opt => opt.Ignore());

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
            CreateMap<CreateSlotRequest, Slot>();

            //Laptop mappings
            CreateMap<Laptop, LaptopResponse>();
            CreateMap<EditLaptopRequest, Laptop>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<CreateLaptopRequest, Laptop>()
                .ForMember(dest => dest.LaptopId, opt => opt.Ignore()) // Khóa chính do DB sinh
                .ForMember(dest => dest.Categories,
                    opt => opt.Ignore()) // Danh sách categories sẽ gán thủ công trong service
                .ForMember(dest => dest.Brand,
                    opt => opt.Ignore()) // Nếu bạn gán Brand navigation, hoặc map BrandId tự động
                .ForMember(dest => dest.Account, opt => opt.Ignore()); // Owner sẽ set qua AccountId

            CreateMap<Account, AccountResponse>();
            CreateMap<Slot, SlotResponse>()
                 .ForMember(dest => dest.Order, opt => opt.Ignore())
                 .ForMember(dest => dest.Laptop, opt => opt.Ignore());

            CreateMap<CreateSlotRequest, Slot>()
                .ForMember(dest => dest.Order, opt => opt.Ignore())
                .ForMember(dest => dest.Laptop, opt => opt.Ignore());
        }
    }
}