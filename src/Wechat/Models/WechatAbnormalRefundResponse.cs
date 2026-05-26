using System.Text.Json.Serialization;
using GaoXinLibrary.PaySDK.Wechat.Core;

namespace GaoXinLibrary.PaySDK.Wechat.Models;

/// <summary>
/// 微信支付异常退款响应
/// <para>https://pay.weixin.qq.com/doc/v3/merchant/4013071193</para>
/// </summary>
public class WechatAbnormalRefundResponse : WechatPayBaseResponse
{
    /// <summary>微信支付退款单号</summary>
    [JsonPropertyName("refund_id")]
    public string RefundId { get; set; } = string.Empty;

    /// <summary>商户退款单号</summary>
    [JsonPropertyName("out_refund_no")]
    public string OutRefundNo { get; set; } = string.Empty;

    /// <summary>微信支付订单号</summary>
    [JsonPropertyName("transaction_id")]
    public string TransactionId { get; set; } = string.Empty;

    /// <summary>商户订单号</summary>
    [JsonPropertyName("out_trade_no")]
    public string OutTradeNo { get; set; } = string.Empty;

    /// <summary>退款渠道：ORIGINAL / BALANCE / OTHER_BALANCE / OTHER_BANKCARD</summary>
    [JsonPropertyName("channel")]
    public string Channel { get; set; } = string.Empty;

    /// <summary>退款入账账户</summary>
    [JsonPropertyName("user_received_account")]
    public string UserReceivedAccount { get; set; } = string.Empty;

    /// <summary>退款成功时间（rfc3339），退款状态为 SUCCESS 时返回</summary>
    [JsonPropertyName("success_time")]
    public string? SuccessTime { get; set; }

    /// <summary>退款创建时间（rfc3339）</summary>
    [JsonPropertyName("create_time")]
    public string CreateTime { get; set; } = string.Empty;

    /// <summary>退款状态：SUCCESS / CLOSED / PROCESSING / ABNORMAL</summary>
    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    /// <summary>资金账户：UNSETTLED / AVAILABLE / UNAVAILABLE / OPERATION / BASIC / ECNY_BASIC</summary>
    [JsonPropertyName("funds_account")]
    public string FundsAccount { get; set; } = string.Empty;

    /// <summary>金额信息</summary>
    [JsonPropertyName("amount")]
    public WechatAbnormalRefundAmountResponse? Amount { get; set; }

    /// <summary>优惠退款详情</summary>
    [JsonPropertyName("promotion_detail")]
    public List<WechatRefundPromotion>? PromotionDetail { get; set; }
}
