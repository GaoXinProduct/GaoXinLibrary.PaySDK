namespace GaoXinLibrary.PaySDK.Core;

/// <summary>
/// 支付回调通知解析结果
/// </summary>
public class PayCallbackResult
{
    /// <summary>支付渠道</summary>
    public PayChannel Channel { get; set; }

    /// <summary>回调是否验签通过</summary>
    public bool IsValid { get; set; }

    /// <summary>商户订单号</summary>
    public string OutTradeNo { get; set; } = string.Empty;

    /// <summary>平台交易号</summary>
    public string? TransactionId { get; set; }

    /// <summary>交易状态（SUCCESS / FAIL 等）</summary>
    public string TradeStatus { get; set; } = string.Empty;

    /// <summary>支付金额（分）</summary>
    public int TotalFee { get; set; }

    /// <summary>买家账号 / openid</summary>
    public string? BuyerAccount { get; set; }

    /// <summary>支付成功时间</summary>
    public DateTimeOffset? SuccessTime { get; set; }

    /// <summary>原始回调体</summary>
    public string? RawBody { get; set; }

    /// <summary>验签失败原因</summary>
    public string? ErrorMessage { get; set; }
}
