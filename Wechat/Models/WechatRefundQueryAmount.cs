using System.Text.Json.Serialization;

namespace GaoXinLibrary.PaySDK.Wechat.Models;

/// <summary>
/// 微信支付退款查询金额信息
/// <para>https://pay.weixin.qq.com/doc/v3/merchant/4013070374</para>
/// </summary>
public class WechatRefundQueryAmount
{
    /// <summary>订单金额（分）</summary>
    [JsonPropertyName("total")]
    public int Total { get; set; }

    /// <summary>退款金额（分）</summary>
    [JsonPropertyName("refund")]
    public int Refund { get; set; }

    /// <summary>退款出资账户及金额</summary>
    [JsonPropertyName("from")]
    public List<WechatRefundFromItem>? From { get; set; }

    /// <summary>用户支付金额（分）</summary>
    [JsonPropertyName("payer_total")]
    public int PayerTotal { get; set; }

    /// <summary>用户退款金额（分）</summary>
    [JsonPropertyName("payer_refund")]
    public int PayerRefund { get; set; }

    /// <summary>应结退款金额（分）</summary>
    [JsonPropertyName("settlement_refund")]
    public int SettlementRefund { get; set; }

    /// <summary>应结订单金额（分）</summary>
    [JsonPropertyName("settlement_total")]
    public int SettlementTotal { get; set; }

    /// <summary>优惠退款金额（分）</summary>
    [JsonPropertyName("discount_refund")]
    public int DiscountRefund { get; set; }

    /// <summary>退款币种（CNY）</summary>
    [JsonPropertyName("currency")]
    public string Currency { get; set; } = string.Empty;

    /// <summary>手续费退款金额（分）</summary>
    [JsonPropertyName("refund_fee")]
    public int? RefundFee { get; set; }
}
