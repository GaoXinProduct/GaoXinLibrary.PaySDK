using System.Text.Json.Serialization;

namespace GaoXinLibrary.PaySDK.Wechat.Core;

/// <summary>
/// 微信支付 v3 API 响应基类
/// </summary>
public class WechatPayBaseResponse
{
    /// <summary>错误码（成功时为 null）</summary>
    [JsonPropertyName("code")]
    public string? Code { get; set; }

    /// <summary>错误信息</summary>
    [JsonPropertyName("message")]
    public string? Message { get; set; }
}
