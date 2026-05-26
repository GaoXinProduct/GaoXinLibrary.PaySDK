using System.Text.Json.Serialization;

namespace GaoXinLibrary.PaySDK.Wechat.Models;

/// <summary>
/// 微信支付退款优惠信息
/// <para>https://pay.weixin.qq.com/doc/v3/merchant/4013070374</para>
/// </summary>
public class WechatRefundPromotion
{
    /// <summary>券 ID</summary>
    [JsonPropertyName("promotion_id")]
    public string PromotionId { get; set; } = string.Empty;

    /// <summary>优惠范围：GLOBAL（全单优惠）/ SINGLE（单品优惠）</summary>
    [JsonPropertyName("scope")]
    public string Scope { get; set; } = string.Empty;

    /// <summary>优惠类型：COUPON（代金券）/ DISCOUNT（优惠券）</summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    /// <summary>优惠券面额（分）</summary>
    [JsonPropertyName("amount")]
    public int Amount { get; set; }

    /// <summary>优惠退款金额（分）</summary>
    [JsonPropertyName("refund_amount")]
    public int RefundAmount { get; set; }

    /// <summary>商品列表</summary>
    [JsonPropertyName("goods_detail")]
    public List<WechatRefundGoodsDetail>? GoodsDetail { get; set; }
}
