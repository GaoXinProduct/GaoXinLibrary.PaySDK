using System.Text.Json.Serialization;

namespace GaoXinLibrary.PaySDK.Wechat.Models;

/// <summary>
/// 微信支付退款单品信息
/// </summary>
public class WechatRefundGoodsDetail
{
    /// <summary>商户侧商品编码</summary>
    [JsonPropertyName("merchant_goods_id")]
    public string MerchantGoodsId { get; set; } = string.Empty;

    /// <summary>微信侧商品编码</summary>
    [JsonPropertyName("wechatpay_goods_id")]
    public string? WechatpayGoodsId { get; set; }

    /// <summary>商品名称</summary>
    [JsonPropertyName("goods_name")]
    public string? GoodsName { get; set; }

    /// <summary>商品单价（分）</summary>
    [JsonPropertyName("unit_price")]
    public int UnitPrice { get; set; }

    /// <summary>商品退款金额（分）</summary>
    [JsonPropertyName("refund_amount")]
    public int RefundAmount { get; set; }

    /// <summary>商品退货数量</summary>
    [JsonPropertyName("refund_quantity")]
    public int RefundQuantity { get; set; }
}
