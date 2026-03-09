namespace GaoXinLibrary.PaySDK.Core;

/// <summary>
/// 查询支付订单响应
/// </summary>
public class QueryOrderResponse
{
    /// <summary>支付渠道</summary>
    public PayChannel Channel { get; set; }

    /// <summary>商户订单号</summary>
    public string OutTradeNo { get; set; } = string.Empty;

    /// <summary>平台交易号</summary>
    public string? TransactionId { get; set; }

    /// <summary>
    /// 交易状态
    /// <para>SUCCESS / REFUND / NOTPAY / CLOSED / REVOKED / USERPAYING / PAYERROR</para>
    /// </summary>
    public string TradeStatus { get; set; } = string.Empty;

    /// <summary>订单金额（分）</summary>
    public int TotalFee { get; set; }

    /// <summary>实际支付金额（分）</summary>
    public int PayerFee { get; set; }

    /// <summary>支付完成时间</summary>
    public DateTimeOffset? SuccessTime { get; set; }

    /// <summary>买家账号 / openid</summary>
    public string? BuyerAccount { get; set; }

    /// <summary>原始响应（JSON 字符串）</summary>
    public string? RawResponse { get; set; }
}
