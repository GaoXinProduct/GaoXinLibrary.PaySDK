using System.Text.Json.Serialization;

namespace GaoXinLibrary.PaySDK.Alipay.Models;

/// <summary>
/// 支付宝退款业务请求内容
/// <para>alipay.trade.refund</para>
/// <para>https://opendocs.alipay.com/open/02ekfj</para>
/// </summary>
public class AlipayTradeRefundContent
{
    /// <summary>商户订单号（与 trade_no 二选一）</summary>
    [JsonPropertyName("out_trade_no")]
    public string? OutTradeNo { get; set; }

    /// <summary>支付宝交易号（与 out_trade_no 二选一）</summary>
    [JsonPropertyName("trade_no")]
    public string? TradeNo { get; set; }

    /// <summary>退款金额，单位为元，精确到小数点后两位</summary>
    [JsonPropertyName("refund_amount")]
    public string RefundAmount { get; set; } = string.Empty;

    /// <summary>退款原因说明</summary>
    [JsonPropertyName("refund_reason")]
    public string? RefundReason { get; set; }

    /// <summary>
    /// 标识一次退款请求，同一笔交易多次退款需保证唯一，如需部分退款传入此参数
    /// </summary>
    [JsonPropertyName("out_request_no")]
    public string? OutRequestNo { get; set; }

    /// <summary>退款包含的商品列表</summary>
    [JsonPropertyName("goods_detail")]
    public List<AlipayGoodsDetail>? GoodsDetail { get; set; }
}
