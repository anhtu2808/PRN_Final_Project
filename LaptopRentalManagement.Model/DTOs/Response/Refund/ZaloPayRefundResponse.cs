using Newtonsoft.Json;

namespace LaptopRentalManagement.Model.DTOs.Response.Refund;

public class ZaloPayRefundResponse
{
    [JsonProperty("return_code")]
    public int ReturnCode { get; set; }

    [JsonProperty("return_message")]
    public string ReturnMessage { get; set; } = string.Empty;
}
