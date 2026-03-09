using System.Text.Json.Serialization;
using GaoXinLibrary.PaySDK.Alipay.Core;

namespace GaoXinLibrary.PaySDK.Alipay.Models;

/// <summary>
/// 支付宝撤销订单响应
/// <para>alipay.trade.cancel</para>
/// <para>https://opendocs.alipay.com/open/13399511_alipay.trade.cancel</para>
/// </summary>
public class AlipayTradeCancelResponse : AlipayBaseResponse
{
    /// <summary>支付宝交易号</summary>
    [JsonPropertyName("trade_no")]
    public string? TradeNo { get; set; }

    /// <summary>商户订单号</summary>
    [JsonPropertyName("out_trade_no")]
    public string? OutTradeNo { get; set; }

    /// <summary>
    /// 是否需要重试
    /// <para>Y：需要重试（系统异常，需要调用方重新调用撤销接口）</para>
    /// <para>N：不需要重试</para>
    /// </summary>
    [JsonPropertyName("retry_flag")]
    public string? RetryFlag { get; set; }

    /// <summary>
    /// 本次撤销触发的交易动作
    /// <para>close：未付款交易，直接关闭</para>
    /// <para>refund：已付款交易，触发退款</para>
    /// <para>wait：等待支付方确认中，暂时无法撤销</para>
    /// </summary>
    [JsonPropertyName("action")]
    public string? Action { get; set; }
}
