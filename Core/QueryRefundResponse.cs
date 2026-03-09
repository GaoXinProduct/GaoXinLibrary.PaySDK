namespace GaoXinLibrary.PaySDK.Core;

/// <summary>
/// 查询退款响应
/// </summary>
public class QueryRefundResponse
{
    /// <summary>支付渠道</summary>
    public PayChannel Channel { get; set; }

    /// <summary>商户订单号</summary>
    public string? OutTradeNo { get; set; }

    /// <summary>平台交易号</summary>
    public string? TransactionId { get; set; }

    /// <summary>商户退款单号</summary>
    public string OutRefundNo { get; set; } = string.Empty;

    /// <summary>
    /// 退款状态
    /// <para>SUCCESS / CLOSED / PROCESSING / ABNORMAL</para>
    /// </summary>
    public string RefundStatus { get; set; } = string.Empty;

    /// <summary>退款金额（分）</summary>
    public int RefundFee { get; set; }

    /// <summary>订单总金额（分）</summary>
    public int TotalFee { get; set; }

    /// <summary>原始响应（JSON 字符串）</summary>
    public string? RawResponse { get; set; }
}
