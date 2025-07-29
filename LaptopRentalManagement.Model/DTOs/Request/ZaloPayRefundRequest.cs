using System.Text.Json.Serialization;

namespace LaptopRentalManagement.Model.DTOs.Request;

public class ZaloPayRefundRequest
{
    [JsonPropertyName("app_id")]
    public int AppId { get; set; }

    [JsonPropertyName("zp_trans_id")]
    public string ZpTransId { get; set; } = string.Empty;

    [JsonPropertyName("amount")]
    public long Amount { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("timestamp")]
    public long Timestamp { get; set; }

    [JsonPropertyName("mac")]
    public string Mac { get; set; } = string.Empty;

    public void MakeSignature(string key)
    {
        var data = $"{AppId}|{ZpTransId}|{Amount}|{Description}|{Timestamp}";
        Mac = LaptopRentalManagement.Model.Helpers.HmacHelper.Compute(key, data);
    }
}
