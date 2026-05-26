using System.Text.Json.Serialization;

namespace GaoXinLibrary.PaySDK.Alipay.Models;

/// <summary>
/// 支付宝订单码（预下单）业务请求
/// <para>alipay.trade.precreate — 生成二维码，用户扫码付款</para>
/// <para>https://opendocs.alipay.com/open/8ad49e4a_alipay.trade.precreate</para>
/// <para>⚠️ notify_url 是外部公共参数，请通过 <see cref="Services.IAlipayService.PrecreateAsync"/> 的 notifyUrl 参数传入，不要在此模型中设置。</para>
/// </summary>
public class AlipayTradePrecreateContent
{
    /// <summary>商户订单号</summary>
    [JsonPropertyName("out_trade_no")]
    public string OutTradeNo { get; set; } = string.Empty;

    /// <summary>销售产品码，固定值 FACE_TO_FACE_PAYMENT</summary>
    [JsonPropertyName("product_code")]
    public string ProductCode { get; set; } = "FACE_TO_FACE_PAYMENT";

    /// <summary>订单标题</summary>
    [JsonPropertyName("subject")]
    public string Subject { get; set; } = string.Empty;

    /// <summary>订单总金额，单位为元，精确到小数点后两位</summary>
    [JsonPropertyName("total_amount")]
    public string TotalAmount { get; set; } = string.Empty;

    /// <summary>订单描述</summary>
    [JsonPropertyName("body")]
    public string? Body { get; set; }

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
}
