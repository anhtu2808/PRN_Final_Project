using LaptopRentalManagement.BLL.Mappings;
using LaptopRentalManagement.BLL.Interfaces;
using LaptopRentalManagement.BLL.Services;
using LaptopRentalManagement.BLL.Hubs;
using LaptopRentalManagement.DAL.Context;
using LaptopRentalManagement.DAL.Interfaces;
using LaptopRentalManagement.DAL.Repositories;
using LaptopRentalManagement.Hubs;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Add DbContext
builder.Services.AddDbContext<LaptopRentalDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Add AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile), typeof(AutoMapperProfile));
        

// Add SignalR
builder.Services.AddSignalR();


// Register repositories
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();

// Register services
builder.Services.AddScoped<IFeedbackService, FeedbackService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();


// Register Hub Service
builder.Services.AddScoped<IHubService, HubService>();

// Register other services using SignalR
builder.Services.AddScoped<INotificationService, NotificationService>();

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

app.UseAuthorization();

app.MapRazorPages();

// Map SignalR Hubs
app.MapHub<BaseHub>("/baseHub");
app.MapHub<NotificationHub>("/notificationHub");
app.MapHub<ChatHub>("/chatHub");
app.MapHub<OrderHub>("/orderHub");

app.Run();
