using System.Text.Json.Serialization;

namespace GaoXinLibrary.PaySDK.Wechat.Models;

/// <summary>
/// 微信支付优惠功能
/// </summary>
public class WechatPayDetail
{
    /// <summary>订单原价（分）</summary>
    [JsonPropertyName("cost_price")]
    public int? CostPrice { get; set; }

    /// <summary>商品小票 ID</summary>
    [JsonPropertyName("invoice_id")]
    public string? InvoiceId { get; set; }

    /// <summary>单品列表</summary>
    [JsonPropertyName("goods_detail")]
    public List<WechatPayGoodsDetail>? GoodsDetail { get; set; }
}
