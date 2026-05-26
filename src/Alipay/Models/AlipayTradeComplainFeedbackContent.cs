using System.Text.Json.Serialization;

namespace GaoXinLibrary.PaySDK.Alipay.Models;

/// <summary>
/// 支付宝交易投诉反馈请求
/// <para>alipay.merchant.tradecomplain.feedback</para>
/// </summary>
public sealed class AlipayTradeComplainFeedbackContent
{
    /// <summary>支付宝侧投诉单号</summary>
    [JsonPropertyName("complain_event_id")]
    public string ComplainEventId { get; set; } = string.Empty;

    /// <summary>商家反馈内容</summary>
    [JsonPropertyName("content")]
    public string Content { get; set; } = string.Empty;

    /// <summary>商家处理投诉时反馈凭证的图片 ID，多个用逗号分隔</summary>
    [JsonPropertyName("images")]
    public string? Images { get; set; }
}
