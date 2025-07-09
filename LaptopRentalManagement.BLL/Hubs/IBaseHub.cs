using Microsoft.AspNetCore.SignalR;

namespace LaptopRentalManagement.BLL.Hubs
{
    public interface IBaseHub
    {
        Task JoinGroup(string groupName);
        Task LeaveGroup(string groupName);
        Task JoinUserRoom(string userId);
        Task LeaveUserRoom(string userId);
    }
} 