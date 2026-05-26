namespace GaoXinLibrary.PaySDK.Core;

/// <summary>
/// 查询退款请求
/// </summary>
public class QueryRefundRequest
{
    /// <summary>支付渠道</summary>
    public PayChannel Channel { get; set; }

    /// <summary>商户订单号（与 TransactionId 二选一）</summary>
    public string? OutTradeNo { get; set; }

    /// <summary>平台交易号（与 OutTradeNo 二选一）</summary>
    public string? TransactionId { get; set; }

    /// <summary>商户退款单号</summary>
    public string OutRefundNo { get; set; } = string.Empty;

    /// <summary>扩展参数（如银联查询需要原始交易时间 TxnTime）</summary>
    public Dictionary<string, string>? Extra { get; set; }
}
