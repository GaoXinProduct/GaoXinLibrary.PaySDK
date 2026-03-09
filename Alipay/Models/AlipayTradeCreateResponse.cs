using System.Text.Json.Serialization;
using GaoXinLibrary.PaySDK.Alipay.Core;

namespace GaoXinLibrary.PaySDK.Alipay.Models;

/// <summary>
/// 支付宝统一收单交易创建响应（JSAPI / 小程序支付）
/// <para>alipay.trade.create</para>
/// </summary>
public class AlipayTradeCreateResponse : AlipayBaseResponse
{
    /// <summary>支付宝交易号</summary>
    [JsonPropertyName("trade_no")]
    public string? TradeNo { get; set; }

    /// <summary>商户订单号</summary>
    [JsonPropertyName("out_trade_no")]
    public string? OutTradeNo { get; set; }
}
