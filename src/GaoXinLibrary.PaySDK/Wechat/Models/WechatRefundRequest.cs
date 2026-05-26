using System.Text.Json.Serialization;

namespace GaoXinLibrary.PaySDK.Wechat.Models;

/// <summary>
/// 微信支付退款申请请求
/// <para>https://pay.weixin.qq.com/doc/v3/merchant/4012086975</para>
/// </summary>
public class WechatRefundRequest
{
    /// <summary>微信支付订单号（与 out_trade_no 二选一）</summary>
    [JsonPropertyName("transaction_id")]
    public string? TransactionId { get; set; }

    /// <summary>商户订单号（与 transaction_id 二选一）</summary>
    [JsonPropertyName("out_trade_no")]
    public string? OutTradeNo { get; set; }

    /// <summary>商户退款单号</summary>
    [JsonPropertyName("out_refund_no")]
    public string OutRefundNo { get; set; } = string.Empty;

    /// <summary>退款原因</summary>
    [JsonPropertyName("reason")]
    public string? Reason { get; set; }

    /// <summary>退款结果回调 URL</summary>
    [JsonPropertyName("notify_url")]
    public string? NotifyUrl { get; set; }

    /// <summary>退款资金来源（仅对老资金流商户适用）</summary>
    [JsonPropertyName("funds_account")]
    public string? FundsAccount { get; set; }

    /// <summary>金额信息</summary>
    [JsonPropertyName("amount")]
    public WechatRefundAmount Amount { get; set; } = new();

    /// <summary>退款商品列表</summary>
    [JsonPropertyName("goods_detail")]
    public List<WechatRefundGoodsDetail>? GoodsDetail { get; set; }
}
