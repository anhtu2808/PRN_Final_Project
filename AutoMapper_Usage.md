# AutoMapper Usage Guide

## Đã Setup AutoMapper với các mappings sau:

### 1. Entity to Entity Mappings
```csharp
// Copy entity với những field cần ignore
var newAccount = mapper.Map<Account>(existingAccount);
var newLaptop = mapper.Map<Laptop>(existingLaptop);
var newOrder = mapper.Map<Order>(existingOrder);
```

### 2. Entity to Anonymous Object (API Response)
```csharp
// Account to simple object
var accountResponse = mapper.Map<object>(account);
// Kết quả: { Id, Email, Name, Role }

// Laptop to simple object  
var laptopResponse = mapper.Map<object>(laptop);
// Kết quả: { Id, Name, Description, PricePerDay, Status, BrandName }

// Order to simple object
var orderResponse = mapper.Map<object>(order);
// Kết quả: { Id, Status, TotalCharge, StartDate, EndDate, LaptopName, RenterName }
```

## Cách sử dụng trong Controller:

### Inject IMapper
```csharp
public class AccountController : ControllerBase
{
    private readonly IMapper _mapper;
    
    public AccountController(IMapper mapper)
    {
        _mapper = mapper;
    }
    
    [HttpGet]
    public IActionResult GetAccounts()
    {
        var accounts = _context.Accounts.ToList();
        var response = _mapper.Map<List<object>>(accounts);
        return Ok(response);
    }
}
```

### Trong Service Layer
```csharp
public class AccountService
{
    private readonly IMapper _mapper;
    
    public AccountService(IMapper mapper)
    {
        _mapper = mapper;
    }
    
    public async Task<Account> CreateAccountAsync(Account model)
    {
        var newAccount = _mapper.Map<Account>(model);
        // newAccount sẽ có AccountId = 0, CreatedAt được ignore
        return await _repository.AddAsync(newAccount);
    }
}
```

## Các Entity được support:
- ✅ Account
- ✅ Laptop  
- ✅ Order
- ✅ Brand
- ✅ Category
- ✅ Review
- ✅ Slot
- ✅ Notification

## Note:
- Primary keys (Id fields) được ignore khi copy entity
- DateTime fields được auto-set cho update operations
- Object mappings trả về anonymous objects phù hợp cho API responses 