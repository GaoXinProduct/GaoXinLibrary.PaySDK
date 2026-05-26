using System.Text.Json.Serialization;

namespace GaoXinLibrary.PaySDK.Wechat.Models;

/// <summary>
/// 微信支付平台证书列表中的单条证书信息
/// </summary>
public class WechatCertificateItem
{
    /// <summary>证书序列号</summary>
    [JsonPropertyName("serial_no")]
    public string SerialNo { get; set; } = string.Empty;

    /// <summary>证书启用时间（ISO 8601）</summary>
    [JsonPropertyName("effective_time")]
    public string? EffectiveTime { get; set; }

    /// <summary>证书过期时间（ISO 8601）</summary>
    [JsonPropertyName("expire_time")]
    public string? ExpireTime { get; set; }

    /// <summary>证书密文信息</summary>
    [JsonPropertyName("encrypt_certificate")]
    public WechatCertificateEncrypt EncryptCertificate { get; set; } = new();
}
