using System.Text.Json.Serialization;

namespace GaoXinLibrary.PaySDK.Alipay.Models;

/// <summary>
/// 支付宝退款查询业务请求内容
/// <para>alipay.trade.fastpay.refund.query</para>
/// <para>https://opendocs.alipay.com/open/c1cb8815_alipay.trade.fastpay.refund.query</para>
/// </summary>
public class AlipayTradeRefundQueryContent
{
    /// <summary>商户订单号（与 trade_no 二选一）</summary>
    [JsonPropertyName("out_trade_no")]
    public string? OutTradeNo { get; set; }

    /// <summary>支付宝交易号（与 out_trade_no 二选一）</summary>
    [JsonPropertyName("trade_no")]
    public string? TradeNo { get; set; }

    /// <summary>
    /// 商户退款请求号（必填）
    /// <para>即申请退款时传入的 out_request_no，用于精确定位一笔退款。</para>
    /// </summary>
    [JsonPropertyName("out_request_no")]
    public string OutRequestNo { get; set; } = string.Empty;
}
