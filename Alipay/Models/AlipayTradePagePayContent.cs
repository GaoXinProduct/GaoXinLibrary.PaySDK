using System.Text.Json.Serialization;

namespace GaoXinLibrary.PaySDK.Alipay.Models;

/// <summary>
/// 支付宝电脑网站支付（PC）业务请求内容
/// <para>alipay.trade.page.pay</para>
/// <para>https://opendocs.alipay.com/open/028r8t</para>
/// </summary>
public class AlipayTradePagePayContent
{
    /// <summary>商户订单号</summary>
    [JsonPropertyName("out_trade_no")]
    public string OutTradeNo { get; set; } = string.Empty;

    /// <summary>
    /// 销售产品码，与支付宝签约的产品码名称，固定值 FAST_INSTANT_TRADE_PAY
    /// </summary>
    [JsonPropertyName("product_code")]
    public string ProductCode { get; set; } = "FAST_INSTANT_TRADE_PAY";

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

    /// <summary>开票信息</summary>
    [JsonPropertyName("invoice_info")]
    public Dictionary<string, string>? InvoiceInfo { get; set; }
}
