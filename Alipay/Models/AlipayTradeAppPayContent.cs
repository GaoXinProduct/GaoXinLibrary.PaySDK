using System.Text.Json.Serialization;

namespace GaoXinLibrary.PaySDK.Alipay.Models;

/// <summary>
/// 支付宝 App 支付业务请求内容
/// <para>alipay.trade.app.pay — 服务端生成签名字符串，客户端 SDK 使用</para>
/// <para>https://opendocs.alipay.com/open/02e7gq</para>
/// </summary>
public class AlipayTradeAppPayContent
{
    /// <summary>商户订单号</summary>
    [JsonPropertyName("out_trade_no")]
    public string OutTradeNo { get; set; } = string.Empty;

    /// <summary>销售产品码，固定值 QUICK_MSECURITY_PAY</summary>
    [JsonPropertyName("product_code")]
    public string ProductCode { get; set; } = "QUICK_MSECURITY_PAY";

    /// <summary>订单总金额，单位为元，精确到小数点后两位</summary>
    [JsonPropertyName("total_amount")]
    public string TotalAmount { get; set; } = string.Empty;

    /// <summary>订单标题</summary>
    [JsonPropertyName("subject")]
    public string Subject { get; set; } = string.Empty;

    /// <summary>订单描述</summary>
    [JsonPropertyName("body")]
    public string? Body { get; set; }

    /// <summary>支付超时</summary>
    [JsonPropertyName("timeout_express")]
    public string? TimeoutExpress { get; set; }

    /// <summary>绝对超时时间</summary>
    [JsonPropertyName("time_expire")]
    public string? TimeExpire { get; set; }

    /// <summary>商品明细列表</summary>
    [JsonPropertyName("goods_detail")]
    public List<AlipayGoodsDetail>? GoodsDetail { get; set; }

    /// <summary>商户传入业务信息</summary>
    [JsonPropertyName("business_params")]
    public Dictionary<string, string>? BusinessParams { get; set; }
}
