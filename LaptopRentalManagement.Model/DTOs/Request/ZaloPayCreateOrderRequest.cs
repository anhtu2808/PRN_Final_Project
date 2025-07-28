using System.Text.Json.Serialization;

namespace LaptopRentalManagement.Model.DTOs.Request;

public class ZaloPayCreateOrderRequest
{
    [JsonPropertyName("app_id")]
    public int AppId { get; set; }

    [JsonPropertyName("app_trans_id")]
    public string AppTransId { get; set; } // Mã giao dịch của riêng bạn, ví dụ: 240324_123456

    [JsonPropertyName("app_user")]
    public string AppUser { get; set; } = "user123";

    [JsonPropertyName("app_time")]
    public long AppTime { get; set; } // Thời gian tạo đơn hàng (miliseconds)

    [JsonPropertyName("amount")]
    public long Amount { get; set; }

    [JsonPropertyName("item")]
    public string Item { get; set; } = "[]"; // Để trống hoặc điền thông tin sản phẩm dạng JSON string

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("embed_data")]
    public string EmbedData { get; set; } = "{}"; // Dữ liệu riêng, ví dụ: {"redirecturl": "https://yourdomain.com/PaymentCallback"}

    [JsonPropertyName("bank_code")]
    public string BankCode { get; set; } = ""; // Để trống để chọn ở cổng ZaloPay

    [JsonPropertyName("callback_url")]
    public string CallbackUrl { get; set; } // URL ZaloPay server sẽ gọi để thông báo kết quả (khác với redirect)

    [JsonPropertyName("mac")]
    public string Mac { get; set; }

    public void MakeSignature(string key)
    {
        var data = $"{this.AppId}|{this.AppTransId}|{this.AppUser}|{this.Amount}|{this.AppTime}|{this.EmbedData}|{this.Item}";
        this.Mac = Helpers.HmacHelper.Compute(key, data);
    }
}