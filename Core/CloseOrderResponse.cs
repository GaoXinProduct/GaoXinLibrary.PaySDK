namespace GaoXinLibrary.PaySDK.Core;

/// <summary>
/// 关闭订单响应
/// </summary>
public class CloseOrderResponse
{
    /// <summary>支付渠道</summary>
    public PayChannel Channel { get; set; }

    /// <summary>商户订单号</summary>
    public string? OutTradeNo { get; set; }

    /// <summary>平台交易号</summary>
    public string? TransactionId { get; set; }

    /// <summary>是否成功</summary>
    public bool Success { get; set; }

    /// <summary>原始响应（JSON 字符串）</summary>
    public string? RawResponse { get; set; }
}
