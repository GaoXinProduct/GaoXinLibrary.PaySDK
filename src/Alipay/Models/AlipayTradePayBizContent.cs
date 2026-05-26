using System.Text.Json.Serialization;
using GaoXinLibrary.PaySDK.Alipay.Core;

namespace GaoXinLibrary.PaySDK.Alipay.Models;

/// <summary>
/// 支付宝当面付（条码支付）业务请求
/// <para>https://opendocs.alipay.com/open/02ivbt</para>
/// </summary>
public class AlipayTradePayBizContent
{
    /// <summary>商户订单号，由商家自定义，64 个字符以内</summary>
    [JsonPropertyName("out_trade_no")]
    public string OutTradeNo { get; set; } = string.Empty;

    /// <summary>销售产品码，固定值 FACE_TO_FACE_PAYMENT</summary>
    [JsonPropertyName("product_code")]
    public string ProductCode { get; set; } = "FACE_TO_FACE_PAYMENT";

    /// <summary>支付场景：bar_code（条码支付）/ wave_code（声波支付）</summary>
    [JsonPropertyName("scene")]
    public string Scene { get; set; } = "bar_code";

    /// <summary>支付授权码（25~30 位数字，用户打开支付宝钱包扫描界面显示的码）</summary>
    [JsonPropertyName("auth_code")]
    public string AuthCode { get; set; } = string.Empty;

    /// <summary>订单标题</summary>
    [JsonPropertyName("subject")]
    public string Subject { get; set; } = string.Empty;

    /// <summary>买家的支付宝用户 id，如果为空，会从传入的码值信息中获取买家 ID</summary>
    [JsonPropertyName("buyer_id")]
    public string? BuyerId { get; set; }

    /// <summary>订单总金额，单位为元，精确到小数点后两位</summary>
    [JsonPropertyName("total_amount")]
    public string TotalAmount { get; set; } = string.Empty;

    /// <summary>订单描述</summary>
    [JsonPropertyName("body")]
    public string? Body { get; set; }

    /// <summary>商品明细列表</summary>
    [JsonPropertyName("goods_detail")]
    public List<AlipayGoodsDetail>? GoodsDetail { get; set; }

    /// <summary>业务扩展参数</summary>
    [JsonPropertyName("extend_params")]
    public Dictionary<string, string>? ExtendParams { get; set; }

    /// <summary>绝对超时时间，格式为 yyyy-MM-dd HH:mm:ss</summary>
    [JsonPropertyName("time_expire")]
    public string? TimeExpire { get; set; }

    /// <summary>商户操作员编号</summary>
    [JsonPropertyName("operator_id")]
    public string? OperatorId { get; set; }

    /// <summary>商户门店编号</summary>
    [JsonPropertyName("store_id")]
    public string? StoreId { get; set; }

    /// <summary>商户机具终端编号</summary>
    [JsonPropertyName("terminal_id")]
    public string? TerminalId { get; set; }
}
