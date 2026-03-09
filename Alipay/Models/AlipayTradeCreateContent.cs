using System.Text.Json.Serialization;

namespace GaoXinLibrary.PaySDK.Alipay.Models;

/// <summary>
/// 支付宝统一收单交易创建业务请求内容（JSAPI / 小程序支付）
/// <para>alipay.trade.create — 用于生活号、小程序内唤起支付</para>
/// </summary>
public class AlipayTradeCreateContent
{
    /// <summary>商户订单号</summary>
    [JsonPropertyName("out_trade_no")]
    public string OutTradeNo { get; set; } = string.Empty;

    /// <summary>
    /// 销售产品码
    /// <para>小程序场景固定值 JSAPI_PAY；生活号场景可不传</para>
    /// </summary>
    [JsonPropertyName("product_code")]
    public string? ProductCode { get; set; }

    /// <summary>订单总金额，单位为元，精确到小数点后两位</summary>
    [JsonPropertyName("total_amount")]
    public string TotalAmount { get; set; } = string.Empty;

    /// <summary>订单标题</summary>
    [JsonPropertyName("subject")]
    public string Subject { get; set; } = string.Empty;

    /// <summary>订单描述</summary>
    [JsonPropertyName("body")]
    public string? Body { get; set; }

    /// <summary>
    /// 买家支付宝用户 ID（buyer_id 与 buyer_open_id 二选一，推荐使用 buyer_open_id）
    /// </summary>
    [JsonPropertyName("buyer_id")]
    public string? BuyerId { get; set; }

    /// <summary>
    /// 买家支付宝用户唯一标识 open_id（buyer_id 与 buyer_open_id 二选一）
    /// </summary>
    [JsonPropertyName("buyer_open_id")]
    public string? BuyerOpenId { get; set; }

    /// <summary>
    /// 商户小程序应用 appid（小程序支付场景必填）
    /// </summary>
    [JsonPropertyName("op_app_id")]
    public string? OpAppId { get; set; }

    /// <summary>商品明细列表</summary>
    [JsonPropertyName("goods_detail")]
    public List<AlipayGoodsDetail>? GoodsDetail { get; set; }

    /// <summary>支付超时，格式如 90m / 1h / 1d</summary>
    [JsonPropertyName("timeout_express")]
    public string? TimeoutExpress { get; set; }

    /// <summary>绝对超时时间，格式为 yyyy-MM-dd HH:mm:ss</summary>
    [JsonPropertyName("time_expire")]
    public string? TimeExpire { get; set; }

    /// <summary>商户门店编号</summary>
    [JsonPropertyName("store_id")]
    public string? StoreId { get; set; }

    /// <summary>业务扩展参数</summary>
    [JsonPropertyName("extend_params")]
    public Dictionary<string, string>? ExtendParams { get; set; }
}
