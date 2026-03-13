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

    /// <summary>
    /// 是否为模拟成功（即平台本身不支持关闭订单 API，SDK 为保持接口一致性而返回成功）
    /// <para>当此值为 <c>true</c> 时，表示实际并未向支付平台发送关闭请求。
    /// 例如银联网关支付不提供独立的关闭订单接口，未支付订单会自动超时关闭。</para>
    /// </summary>
    public bool IsSimulated { get; set; }

    /// <summary>原始响应（JSON 字符串）</summary>
    public string? RawResponse { get; set; }
}
