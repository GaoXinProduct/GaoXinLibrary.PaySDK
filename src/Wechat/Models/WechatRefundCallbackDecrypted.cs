using System.Text.Json.Serialization;

namespace GaoXinLibrary.PaySDK.Wechat.Models;

/// <summary>
/// 微信支付退款回调通知解密后的退款信息
/// <para>https://pay.weixin.qq.com/doc/v3/merchant/4013071196</para>
/// </summary>
public class WechatRefundCallbackDecrypted
{
    /// <summary>商户号</summary>
    [JsonPropertyName("mchid")]
    public string MchId { get; set; } = string.Empty;

    /// <summary>商户订单号</summary>
    [JsonPropertyName("out_trade_no")]
    public string OutTradeNo { get; set; } = string.Empty;

    /// <summary>微信支付订单号</summary>
    [JsonPropertyName("transaction_id")]
    public string TransactionId { get; set; } = string.Empty;

    /// <summary>商户退款单号</summary>
    [JsonPropertyName("out_refund_no")]
    public string OutRefundNo { get; set; } = string.Empty;

    /// <summary>微信支付退款单号</summary>
    [JsonPropertyName("refund_id")]
    public string RefundId { get; set; } = string.Empty;

    /// <summary>退款状态：SUCCESS / CLOSED / PROCESSING / ABNORMAL</summary>
    [JsonPropertyName("refund_status")]
    public string RefundStatus { get; set; } = string.Empty;

    /// <summary>退款成功时间（rfc3339），仅退款成功时返回</summary>
    [JsonPropertyName("success_time")]
    public string? SuccessTime { get; set; }

    /// <summary>退款入账账户</summary>
    [JsonPropertyName("user_received_account")]
    public string UserReceivedAccount { get; set; } = string.Empty;

    /// <summary>金额信息</summary>
    [JsonPropertyName("amount")]
    public WechatRefundCallbackAmount? Amount { get; set; }
}
