namespace GaoXinLibrary.PaySDK.Core;

/// <summary>
/// 关闭订单请求
/// </summary>
public class CloseOrderRequest
{
    /// <summary>支付渠道</summary>
    public PayChannel Channel { get; set; }

    /// <summary>商户订单号（与 TransactionId 二选一）</summary>
    public string? OutTradeNo { get; set; }

    /// <summary>平台交易号（与 OutTradeNo 二选一）</summary>
    public string? TransactionId { get; set; }
}
