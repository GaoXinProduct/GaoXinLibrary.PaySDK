using System.Text.Json.Serialization;

namespace GaoXinLibrary.PaySDK.Wechat.Models;

/// <summary>
/// 微信支付退款金额信息
/// </summary>
public class WechatRefundAmount
{
    /// <summary>退款金额（分），不能超过原订单支付金额</summary>
    [JsonPropertyName("refund")]
    public int Refund { get; set; }

    /// <summary>原订单金额（分）</summary>
    [JsonPropertyName("total")]
    public int Total { get; set; }

    /// <summary>退款货币类型，默认 CNY</summary>
    [JsonPropertyName("currency")]
    public string Currency { get; set; } = "CNY";
}
