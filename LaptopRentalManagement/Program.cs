using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using LaptopRentalManagement.BLL.Mappings;
using LaptopRentalManagement.BLL.Interfaces;
using LaptopRentalManagement.BLL.Services;
using LaptopRentalManagement.DAL.Context;
using LaptopRentalManagement.DAL.Interfaces;
using LaptopRentalManagement.DAL.Repositories;
using LaptopRentalManagement.Hubs;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();



var awsRegion = builder.Configuration["AWS:Region"]!;

var bucketName = Environment.GetEnvironmentVariable("BUCKET_NAME")!;
var accessKey  = Environment.GetEnvironmentVariable("ACCESS_KEY")!;
var secretKey  = Environment.GetEnvironmentVariable("SECRET_KEY")!;


var credentials = new BasicAWSCredentials(accessKey, secretKey);
builder.Services.AddSingleton<IAmazonS3>(sp =>
    new AmazonS3Client(
        credentials,
        RegionEndpoint.GetBySystemName(awsRegion)
    )
);

// Add Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
        options.SlidingExpiration = true;
    });

// Add Authorization with role-based policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("StaffOrAdmin", policy => policy.RequireRole("Admin", "Staff"));
    options.AddPolicy("CustomerOnly", policy => policy.RequireRole("Customer"));
    options.AddPolicy("AuthenticatedUser", policy => policy.RequireAuthenticatedUser());
});

// Add DbContext
builder.Services.AddDbContext<LaptopRentalDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile), typeof(AutoMapperProfile));


// Add SignalR
builder.Services.AddSignalR();


// Register Repositories
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ILaptopRepository, LaptopRepository>();
builder.Services.AddScoped<IBrandRepository, BrandRepository>();

// Register Business Services
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ILaptopService, LaptopService>();
builder.Services.AddScoped<IBrandService, BrandService>();


// Register repositories
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ILaptopRepository, LaptopRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ISlotRespository, SlotRepository>();
builder.Services.AddScoped<IOrderLogRepository, OrderLogRepository>();
builder.Services.AddScoped<IOrderLogImgRepository, OrderLogImgRepository>();
builder.Services.AddScoped<ISlotRespository, SlotRepository>();
builder.Services.AddScoped<ITicketRepository, TicketRepository>();

// Register services
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ILaptopService, LaptopService>();
builder.Services.AddScoped<IFeedbackService, FeedbackService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IFileUploadService, FileUploadService>();
builder.Services.AddScoped<ITicketService, TicketService>();

// Register Chat services
builder.Services.AddScoped<IChatRoomRepository, ChatRoomRepository>();
builder.Services.AddScoped<IChatMessageRepository, ChatMessageRepository>();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<ITicketChatMessageRepository, TicketChatMessageRepository>();
builder.Services.AddScoped<ITicketChatService, TicketChatService>();

// Register ZaloPay services
builder.Services.AddScoped<IZaloPayService, ZaloPayService>();
builder.Services.AddHttpClient();

builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ISlotService, SlotService>();
builder.Services.AddScoped<IOrderLogService, OrderLogService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Add Authentication and Authorization middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

// Map new ChatHub
app.MapHub<ChatHub>("/chathub");
app.MapHub<RentalHub>("/rentalHub");
app.MapHub<TicketChatHub>("/ticketChatHub");

app.Run();