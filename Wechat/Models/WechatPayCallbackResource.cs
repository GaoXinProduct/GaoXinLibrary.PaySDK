using System.Text.Json.Serialization;

namespace GaoXinLibrary.PaySDK.Wechat.Models;

/// <summary>
/// 微信支付回调通知资源密文
/// </summary>
public class WechatPayCallbackResource
{
    /// <summary>加密算法，固定 AEAD_AES_256_GCM</summary>
    [JsonPropertyName("algorithm")]
    public string Algorithm { get; set; } = string.Empty;

    /// <summary>数据密文（Base64）</summary>
    [JsonPropertyName("ciphertext")]
    public string Ciphertext { get; set; } = string.Empty;

    /// <summary>附加数据（解密时使用）</summary>
    [JsonPropertyName("associated_data")]
    public string? AssociatedData { get; set; }

    /// <summary>原始回调类型</summary>
    [JsonPropertyName("original_type")]
    public string OriginalType { get; set; } = string.Empty;

    /// <summary>加密使用的随机串（12 字节）</summary>
    [JsonPropertyName("nonce")]
    public string Nonce { get; set; } = string.Empty;
}
