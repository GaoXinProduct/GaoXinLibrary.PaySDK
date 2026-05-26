using System.Text.Json.Serialization;

namespace GaoXinLibrary.PaySDK.Wechat.Models;

/// <summary>
/// 微信支付订单金额信息
/// </summary>
public class WechatPayAmount
{
    /// <summary>订单总金额，单位分</summary>
    [JsonPropertyName("total")]
    public int Total { get; set; }

    /// <summary>货币类型，默认 CNY</summary>
    [JsonPropertyName("currency")]
    public string Currency { get; set; } = "CNY";
}
