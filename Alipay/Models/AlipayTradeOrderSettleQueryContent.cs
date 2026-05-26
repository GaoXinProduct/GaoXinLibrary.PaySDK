using System.Text.Json.Serialization;

namespace GaoXinLibrary.PaySDK.Alipay.Models;

/// <summary>
/// 支付宝交易分账查询请求
/// <para>alipay.trade.order.settle.query</para>
/// </summary>
public sealed class AlipayTradeOrderSettleQueryContent
{
    /// <summary>分账请求号（与 trade_no 二选一）</summary>
    [JsonPropertyName("out_request_no")]
    public string? OutRequestNo { get; set; }

    /// <summary>支付宝交易号（与 out_request_no 二选一）</summary>
    [JsonPropertyName("trade_no")]
    public string? TradeNo { get; set; }

    /// <summary>扩展参数</summary>
    [JsonPropertyName("extend_params")]
    public string? ExtendParams { get; set; }
}
