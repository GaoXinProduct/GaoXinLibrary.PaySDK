using System.Text.Json.Serialization;

namespace GaoXinLibrary.PaySDK.Alipay.Models;

/// <summary>
/// 支付宝手机网站支付（H5）业务请求内容
/// <para>alipay.trade.wap.pay</para>
/// <para>https://opendocs.alipay.com/open/02ivbt</para>
/// </summary>
public class AlipayTradeWapPayContent
{
    /// <summary>商户订单号</summary>
    [JsonPropertyName("out_trade_no")]
    public string OutTradeNo { get; set; } = string.Empty;

    /// <summary>销售产品码，固定值 QUICK_WAP_WAY</summary>
    [JsonPropertyName("product_code")]
    public string ProductCode { get; set; } = "QUICK_WAP_WAY";

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

    /// <summary>支付完成后同步跳转的页面路径</summary>
    [JsonPropertyName("quit_url")]
    public string? QuitUrl { get; set; }
}
