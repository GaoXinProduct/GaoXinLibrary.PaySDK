using System.Text.Json.Serialization;

namespace GaoXinLibrary.PaySDK.Wechat.Models;

/// <summary>
/// 微信支付平台证书密文信息
/// </summary>
public class WechatCertificateEncrypt
{
    /// <summary>加密算法，固定 AEAD_AES_256_GCM</summary>
    [JsonPropertyName("algorithm")]
    public string Algorithm { get; set; } = string.Empty;

    /// <summary>附加数据（固定 "certificate"）</summary>
    [JsonPropertyName("associated_data")]
    public string? AssociatedData { get; set; }

    /// <summary>加密使用的随机串（12 字节）</summary>
    [JsonPropertyName("nonce")]
    public string Nonce { get; set; } = string.Empty;

    /// <summary>证书密文（Base64）</summary>
    [JsonPropertyName("ciphertext")]
    public string Ciphertext { get; set; } = string.Empty;
}
