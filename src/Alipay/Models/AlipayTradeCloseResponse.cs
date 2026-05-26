using System.Text.Json.Serialization;
using GaoXinLibrary.PaySDK.Alipay.Core;

namespace GaoXinLibrary.PaySDK.Alipay.Models;

/// <summary>
/// 支付宝关闭订单响应
/// <para>alipay.trade.close</para>
/// </summary>
public class AlipayTradeCloseResponse : AlipayBaseResponse
{
    /// <summary>支付宝交易号</summary>
    [JsonPropertyName("trade_no")]
    public string? TradeNo { get; set; }

    /// <summary>商户订单号</summary>
    [JsonPropertyName("out_trade_no")]
    public string? OutTradeNo { get; set; }
}
