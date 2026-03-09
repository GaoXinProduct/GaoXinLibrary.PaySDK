using System.Text.Json.Serialization;

namespace GaoXinLibrary.PaySDK.Wechat.Models;

/// <summary>
/// 微信支付回调通知请求体
/// <para>https://pay.weixin.qq.com/doc/v3/merchant/4012791860</para>
/// </summary>
public class WechatPayCallbackBody
{
    /// <summary>通知 ID</summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>创建时间（rfc3339）</summary>
    [JsonPropertyName("create_time")]
    public string CreateTime { get; set; } = string.Empty;

    /// <summary>通知类型，如 TRANSACTION.SUCCESS</summary>
    [JsonPropertyName("event_type")]
    public string EventType { get; set; } = string.Empty;

    /// <summary>通知数据类型，如 encrypt-resource</summary>
    [JsonPropertyName("resource_type")]
    public string ResourceType { get; set; } = string.Empty;

    /// <summary>通知资源数据</summary>
    [JsonPropertyName("resource")]
    public WechatPayCallbackResource Resource { get; set; } = new();

    /// <summary>摘要</summary>
    [JsonPropertyName("summary")]
    public string? Summary { get; set; }
}
