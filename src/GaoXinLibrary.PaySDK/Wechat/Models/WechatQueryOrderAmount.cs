using System.Text.Json.Serialization;

namespace GaoXinLibrary.PaySDK.Wechat.Models;

/// <summary>
/// 微信支付查询订单金额信息
/// </summary>
public class WechatQueryOrderAmount
{
    /// <summary>订单总金额（分）</summary>
    [JsonPropertyName("total")]
    public int Total { get; set; }

    /// <summary>用户支付金额（分）</summary>
    [JsonPropertyName("payer_total")]
    public int PayerTotal { get; set; }

    /// <summary>货币类型</summary>
    [JsonPropertyName("currency")]
    public string Currency { get; set; } = "CNY";

    /// <summary>用户支付货币类型</summary>
    [JsonPropertyName("payer_currency")]
    public string? PayerCurrency { get; set; }
}
