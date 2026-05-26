using System.Text.Json.Serialization;

namespace GaoXinLibrary.PaySDK.Wechat.Models;

/// <summary>
/// 微信支付单品信息
/// </summary>
public class WechatPayGoodsDetail
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

    /// <summary>商品数量</summary>
    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }

    /// <summary>商品单价（分）</summary>
    [JsonPropertyName("unit_price")]
    public int UnitPrice { get; set; }
}
