using System.Text.Json.Serialization;

namespace GaoXinLibrary.PaySDK.Wechat.Models;

/// <summary>
/// 微信支付退款资金来源明细
/// <para>https://pay.weixin.qq.com/doc/v3/merchant/4013070374</para>
/// </summary>
public class WechatRefundFromItem
{
    /// <summary>出资账户类型：AVAILABLE / UNAVAILABLE</summary>
    [JsonPropertyName("account")]
    public string Account { get; set; } = string.Empty;

    /// <summary>出资金额（分）</summary>
    [JsonPropertyName("amount")]
    public int Amount { get; set; }
}
