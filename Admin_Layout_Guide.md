# Admin Layout Guide

## Tổng quan
Admin layout với sidebar đẹp được tạo cho laptop rental management system. Layout sử dụng Bootstrap 5, Font Awesome, Google Fonts (Inter) và custom CSS.

## Cấu trúc files
```
LaptopRentalManagement/
├── Pages/
│   ├── Shared/
│   │   └── _AdminLayout.cshtml       # Main admin layout
│   └── Manage/
│       ├── _ViewImports.cshtml       # ViewImports cho manage area
│       ├── Index.cshtml              # Dashboard page
│       └── Index.cshtml.cs           # Dashboard PageModel
├── wwwroot/
│   ├── css/
│   │   └── admin.css                 # Admin styling
│   └── js/
│       └── admin.js                  # Admin JavaScript
```

## Cách sử dụng

### 1. Tạo page mới với admin layout
```csharp
@page
@model YourPageModel
@{
    ViewData["Title"] = "Page Title";
    Layout = "~/Pages/Shared/_AdminLayout.cshtml";
}

<div class="row">
    <div class="col-12">
        <div class="card">
            <div class="card-header">
                <h5 class="card-title mb-0">Your Content</h5>
            </div>
            <div class="card-body">
                <!-- Your content here -->
            </div>
        </div>
    </div>
</div>
```

### 2. Thêm Scripts
```html
@section Scripts {
    <script>
        // Your custom scripts
    </script>
}
```

## Features

### Sidebar Navigation
- **Responsive design**: Collapse trên mobile, overlay menu
- **Desktop collapse**: Click button để thu nhỏ sidebar chỉ còn icons
- **Tooltips**: Hover vào icons khi collapsed để xem tên menu
- **Active states**: Tự động highlight page hiện tại
- **Icons**: Font Awesome icons cho mỗi menu item
- **User info**: Thông tin user và logout button
- **State persistence**: Trạng thái collapsed được lưu trong localStorage

### Header
- **Page title**: Dynamic title từ ViewData["Title"]
- **Notifications**: Dropdown với notifications
- **User menu**: Profile, settings, logout

### Utilities
JavaScript utilities có sẵn:
```javascript
// Loading states
AdminUtils.showLoading('#submitButton');
AdminUtils.hideLoading('#submitButton');

// Alerts
AdminUtils.showAlert('Success message', 'success');
AdminUtils.showAlert('Error message', 'danger');

// Format currency
AdminUtils.formatCurrency(150000); // ₫150,000

// Format date
AdminUtils.formatDate(new Date()); // DD/MM/YYYY HH:mm
```

### CSS Classes
Các utility classes có sẵn:
- `.card`, `.card-header`, `.card-body`
- `.btn-primary`, `.btn-outline-primary`
- `.text-primary`, `.text-success`, `.text-warning`, `.text-danger`
- `.badge-danger`

## Customization

### Thay đổi colors
Chỉnh sửa CSS variables trong `admin.css`:
```css
:root {
  --primary-color: #4f46e5;     /* Main brand color */
  --sidebar-width: 280px;       /* Sidebar width */
  --header-height: 70px;        /* Header height */
}
```

### Thêm menu items
Chỉnh sửa sidebar navigation trong `_AdminLayout.cshtml`:
```html
<li class="nav-item">
    <a class="nav-link" href="/manage/your-page">
        <i class="fas fa-your-icon"></i>
        <span>Your Menu</span>
    </a>
</li>
```

## Routes
Tất cả admin pages nên bắt đầu với `/manage`:
- `/manage` - Dashboard
- `/manage/orders` - Orders management
- `/manage/laptops` - Laptops management
- `/manage/customers` - Customers management
- etc.

## Responsive Design
- **Desktop (≥992px)**: Full sidebar visible, có thể collapse thành icon-only
- **Tablet (768px-991px)**: Collapsible sidebar
- **Mobile (<768px)**: Overlay sidebar với backdrop

## Sidebar Collapse Feature
- **Desktop collapse button**: Nút collapse (chevron) ở góc phải sidebar header
- **Icon-only mode**: Khi collapsed, sidebar width = 80px, chỉ hiện icons
- **Tooltips**: Hover vào icons sẽ hiện tooltip với tên menu
- **Main content adjust**: Content area tự động điều chỉnh margin
- **Persistent state**: Trạng thái collapse được lưu và khôi phục khi reload page
- **Smooth transitions**: Animation 0.3s cho tất cả transitions

## Browser Support
- Chrome 80+
- Firefox 75+
- Safari 13+
- Edge 80+

## Dependencies
- Bootstrap 5.3+
- Font Awesome 6.4+
- jQuery 3.6+ (optional, for DataTables)
- Chart.js (optional, for charts) 