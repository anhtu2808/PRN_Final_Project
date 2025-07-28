using Microsoft.AspNetCore.SignalR;

namespace LaptopRentalManagement.Hubs
{
	public class RentalHub : Hub
	{
		public async Task NotifySlotUpdate(int laptopId, List<int> slotIds)
		{
			await Clients.Others.SendAsync("ReceiveSlotUpdate", laptopId, slotIds);
		}
	}
}
