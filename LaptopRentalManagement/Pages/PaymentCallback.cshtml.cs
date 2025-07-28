using LaptopRentalManagement.BLL.Interfaces.LaptopRentalManagement.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LaptopRentalManagement.Pages;

public class PaymentCallbackModel : PageModel
{
    private readonly IZaloPayService _zaloPayService;
        
    public string Message { get; set; }
    public bool IsSuccess { get; set; }

    public PaymentCallbackModel(IZaloPayService zaloPayService)
    {
        _zaloPayService = zaloPayService;
    }

    public IActionResult OnGet()
    {
        // 1. Xác thực callback từ ZaloPay
        // Lưu ý: Tài liệu ZaloPay nói rằng việc kiểm tra mac ở redirect là không bắt buộc
        // nhưng nên làm để tăng bảo mật.
        var isValidSignature = _zaloPayService.VerifyCallback(Request.Query);

        if (!isValidSignature)
        {
            IsSuccess = false;
            Message = "Chữ ký không hợp lệ!";
            return Page();
        }
            
        // 2. Lấy trạng thái thanh toán
        var status = Request.Query["status"];

        if (status == "1") // 1: Thành công, 0: User hủy, -1: Giao dịchpending
        {
            // TODO: Cập nhật trạng thái đơn hàng trong database của bạn là "Đã thanh toán"
            // Dựa vào `apptransid` để tìm đúng đơn hàng
            var appTransId = Request.Query["apptransid"];
            // yourOrderRepository.UpdateStatus(appTransId, "Paid");

            IsSuccess = true;
            Message = "Thanh toán thành công!";
        }
        else
        {
            IsSuccess = false;
            Message = "Thanh toán thất bại hoặc đã bị hủy.";
        }

        return Page();
    }
}