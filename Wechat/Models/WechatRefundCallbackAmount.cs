using System.Text.Json.Serialization;

namespace GaoXinLibrary.PaySDK.Wechat.Models;

/// <summary>
/// 微信支付退款回调金额信息
/// <para>https://pay.weixin.qq.com/doc/v3/merchant/4013071196</para>
/// </summary>
public class WechatRefundCallbackAmount
{
    /// <summary>原订单金额（分）</summary>
    [JsonPropertyName("total")]
    public int Total { get; set; }

    /// <summary>退款金额（分）</summary>
    [JsonPropertyName("refund")]
    public int Refund { get; set; }

    /// <summary>用户实际支付金额（分）</summary>
    [JsonPropertyName("payer_total")]
    public int PayerTotal { get; set; }

    /// <summary>用户退款金额（分）</summary>
    [JsonPropertyName("payer_refund")]
    public int PayerRefund { get; set; }
}
