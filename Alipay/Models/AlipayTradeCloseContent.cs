using System.Text.Json.Serialization;

namespace GaoXinLibrary.PaySDK.Alipay.Models;

/// <summary>
/// 支付宝关闭订单业务请求内容
/// <para>alipay.trade.close</para>
/// <para>https://opendocs.alipay.com/open/ce0b4954_alipay.trade.close</para>
/// </summary>
public class AlipayTradeCloseContent
{
    /// <summary>商户订单号（与 trade_no 二选一）</summary>
    [JsonPropertyName("out_trade_no")]
    public string? OutTradeNo { get; set; }

    /// <summary>支付宝交易号（与 out_trade_no 二选一）</summary>
    [JsonPropertyName("trade_no")]
    public string? TradeNo { get; set; }

    /// <summary>商户操作员编号（可选）</summary>
    [JsonPropertyName("operator_id")]
    public string? OperatorId { get; set; }
}
