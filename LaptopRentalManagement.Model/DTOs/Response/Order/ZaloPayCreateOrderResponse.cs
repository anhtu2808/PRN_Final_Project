using Newtonsoft.Json;

namespace LaptopRentalManagement.Model.DTOs.Response.Order;

public class ZaloPayCreateOrderResponse
{
    [JsonProperty("return_code")]
    public int ReturnCode { get; set; }

    [JsonProperty("return_message")]
    public string ReturnMessage { get; set; } = string.Empty;

    [JsonProperty("sub_return_code")]
    public int SubReturnCode { get; set; }

    [JsonProperty("sub_return_message")]
    public string SubReturnMessage { get; set; } = string.Empty;

    [JsonProperty("order_url")]
    public string OrderUrl { get; set; } = string.Empty;

    [JsonProperty("zp_trans_token")]
    public string ZpTransToken { get; set; } = string.Empty;

    [JsonProperty("order_token")]
    public string OrderToken { get; set; } = string.Empty;
}