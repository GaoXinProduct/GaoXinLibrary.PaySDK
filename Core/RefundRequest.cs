namespace GaoXinLibrary.PaySDK.Core;

/// <summary>
/// 退款申请请求
/// </summary>
public class RefundRequest
{
    /// <summary>支付渠道</summary>
    public PayChannel Channel { get; set; }

    /// <summary>商户订单号（与 TransactionId 二选一）</summary>
    public string? OutTradeNo { get; set; }

    /// <summary>平台交易号（与 OutTradeNo 二选一）</summary>
    public string? TransactionId { get; set; }

    /// <summary>商户退款单号（唯一）</summary>
    public string OutRefundNo { get; set; } = string.Empty;

    /// <summary>退款金额（分）</summary>
    public int RefundFee { get; set; }

    /// <summary>订单总金额（分，银联退款必填）</summary>
    public int TotalFee { get; set; }

    /// <summary>退款原因</summary>
    public string? Reason { get; set; }

    /// <summary>退款结果通知 URL（部分渠道支持）</summary>
    public string? NotifyUrl { get; set; }
}
