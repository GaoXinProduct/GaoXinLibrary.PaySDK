using System.Text.Json.Serialization;

namespace GaoXinLibrary.PaySDK.Alipay.Models;

/// <summary>
/// 支付宝商品明细
/// </summary>
public class AlipayGoodsDetail
{
    /// <summary>商品的编号</summary>
    [JsonPropertyName("goods_id")]
    public string GoodsId { get; set; } = string.Empty;

    /// <summary>商品名称</summary>
    [JsonPropertyName("goods_name")]
    public string GoodsName { get; set; } = string.Empty;

    /// <summary>商品数量</summary>
    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }

    /// <summary>商品单价，单位为元，精确到小数点后两位</summary>
    [JsonPropertyName("price")]
    public string Price { get; set; } = string.Empty;

    /// <summary>商品类目</summary>
    [JsonPropertyName("goods_category")]
    public string? GoodsCategory { get; set; }

    /// <summary>商品描述信息</summary>
    [JsonPropertyName("body")]
    public string? Body { get; set; }
}
