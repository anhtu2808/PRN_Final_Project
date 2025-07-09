# SignalR Infrastructure Usage Guide

## 🚀 Setup hoàn tất với các components:

### 📁 Hubs Available:
- **BaseHub** (`/baseHub`) - Hub chung, auto connection management (trong BLL.Hubs)
- **NotificationHub** (`/notificationHub`) - Cho notifications
- **ChatHub** (`/chatHub`) - Cho messaging features  
- **OrderHub** (`/orderHub`) - Cho real-time order tracking

### 🔧 Project Structure:
- **BaseHub** - Nằm trong `LaptopRentalManagement.BLL.Hubs`
- **HubService** - Nằm trong `LaptopRentalManagement.BLL.Services`
- **Specialized Hubs** - Nằm trong `LaptopRentalManagement.Hubs`, inherit từ BLL.BaseHub

### 🔧 Services:
- **IHubService** - Service chung để gửi messages từ business logic
- **INotificationService** - Service specific cho notifications

## 💡 Cách sử dụng trong Business Services:

### 1. Inject IHubService vào service của bạn:
```csharp
public class OrderService
{
    private readonly IHubService _hubService;
    
    public OrderService(IHubService hubService)
    {
        _hubService = hubService;
    }
    
    public async Task CreateOrderAsync(Order order)
    {
        // Business logic...
        await _repository.AddAsync(order);
        
        // Send real-time notification
        await _hubService.SendToUserAsync(
            order.RenterId.ToString(), 
            "OrderCreated", 
            new { orderId = order.OrderId, status = "Pending" }
        );
        
        // Notify admins
        await _hubService.SendToGroupAsync(
            "Admins", 
            "NewOrderReceived", 
            new { orderId = order.OrderId, customer = order.Renter.Name }
        );
    }
}
```

### 2. Sử dụng NotificationService:
```csharp
public class AccountService
{
    private readonly INotificationService _notificationService;
    
    public AccountService(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }
    
    public async Task ApproveOrderAsync(int orderId)
    {
        // Approve logic...
        
        // Send notification
        await _notificationService.SendOrderApprovedNotificationAsync(
            userId: "123", 
            orderId: orderId, 
            laptopModel: "MacBook Pro"
        );
    }
}
```

### 3. Tạo service mới sử dụng SignalR:
```csharp
public class YourService
{
    private readonly IHubService _hubService;
    
    public YourService(IHubService hubService)
    {
        _hubService = hubService;
    }
    
    public async Task DoSomethingAsync()
    {
        // Send to specific user
        await _hubService.SendToUserAsync("userId", "MethodName", data);
        
        // Send to group
        await _hubService.SendToGroupAsync("GroupName", "MethodName", data);
        
        // Send to all
        await _hubService.SendToAllAsync("MethodName", data);
        
        // Check if user online
        bool isOnline = await _hubService.IsUserOnlineAsync("userId");
    }
}
```

## 🎯 Frontend Connection Examples:

### JavaScript/TypeScript:
```javascript
// Connect to specific hub
const notificationConnection = new signalR.HubConnectionBuilder()
    .withUrl("/notificationHub", {
        accessTokenFactory: () => localStorage.getItem("token")
    })
    .build();

// Listen for notifications
notificationConnection.on("ReceiveNotification", (data) => {
    console.log("Notification:", data);
    // Show notification in UI
});

// Listen for order updates
notificationConnection.on("OrderStatusUpdated", (data) => {
    console.log("Order updated:", data);
    // Update order status in UI
});

// Start connection
await notificationConnection.start();

// Connect to chat hub
const chatConnection = new signalR.HubConnectionBuilder()
    .withUrl("/chatHub", {
        accessTokenFactory: () => localStorage.getItem("token")
    })
    .build();

// Listen for messages
chatConnection.on("ReceiveMessage", (data) => {
    console.log("New message:", data);
});

// Send message
await chatConnection.invoke("SendMessage", "receiverId", "Hello!");
```

## 📋 Available Methods by Hub:

### NotificationHub:
- `SendNotification(userId, message, type)`
- `SendOrderUpdate(userId, orderData)`
- `SendSystemAlert(message, type)`

### ChatHub:
- `SendMessage(receiverId, message)`
- `JoinChatRoom(roomId)`
- `SendRoomMessage(roomId, message)`
- `StartTyping(receiverId)`
- `StopTyping(receiverId)`

### OrderHub:
- `TrackOrder(orderId)`
- `UpdateOrderStatus(orderId, status, notes)`
- `SendOrderMessage(orderId, message)`
- `RequestOrderExtension(orderId, newEndDate, reason)`
- `ApproveOrderExtension(orderId, newEndDate, adminNotes)`

### BaseHub (available in all hubs):
- `JoinGroup(groupName)`
- `LeaveGroup(groupName)`
- `JoinUserRoom(userId)`
- `LeaveUserRoom(userId)`

## 🔑 Groups tự động:
- `User_{userId}` - Mỗi user có group riêng
- `Admins` - Tất cả admin users
- `Customers` - Tất cả customer users
- `Order_{orderId}` - Tracking specific order
- `ChatRoom_{roomId}` - Chat rooms

## 🎨 Frontend Events để listen:

### Notifications:
- `ReceiveNotification`
- `OrderStatusUpdated`
- `OrderApproved`
- `OrderRejected`
- `OrderCancelled`
- `NewOrderReceived` (for admins)
- `SystemAlert` (for admins)

### Chat:
- `ReceiveMessage`
- `MessageSent`
- `ReceiveRoomMessage`
- `UserJoined`
- `UserLeft`
- `UserTyping`
- `UserStoppedTyping`

### Orders:
- `OrderTrackingStarted`
- `OrderStatusUpdated`
- `OrderMessage`
- `OrderExtensionRequested`
- `OrderExtensionApproved`
- `OrderExtensionRejected`

## ⚙️ Configuration:
- JWT authentication đã được setup
- CORS configured
- Multiple hubs mapped to different endpoints
- Automatic group management
- Connection tracking

Bạn chỉ cần inject `IHubService` hoặc `INotificationService` vào services và sử dụng! 