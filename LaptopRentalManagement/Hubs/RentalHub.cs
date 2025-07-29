using Microsoft.AspNetCore.SignalR;

namespace LaptopRentalManagement.Hubs
{
	public class RentalHub : Hub
	{
		public async Task NotifySlotUpdate(int laptopId, List<int> slotIds, String status)
		{
			await Clients.Others.SendAsync("ReceiveSlotUpdate", laptopId, slotIds);
			await Clients.Others.SendAsync("ReceiveOrderStatusUpdate");
		}
	}
}
