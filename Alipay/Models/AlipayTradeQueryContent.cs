using System.Text.Json.Serialization;

namespace GaoXinLibrary.PaySDK.Alipay.Models;

/// <summary>
/// 支付宝查询订单业务请求内容
/// <para>alipay.trade.query</para>
/// </summary>
public class AlipayTradeQueryContent
{
    /// <summary>商户订单号（与 trade_no 二选一）</summary>
    [JsonPropertyName("out_trade_no")]
    public string? OutTradeNo { get; set; }

    /// <summary>支付宝交易号（与 out_trade_no 二选一）</summary>
    [JsonPropertyName("trade_no")]
    public string? TradeNo { get; set; }
}
