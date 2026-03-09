using System.Text.Json.Serialization;
using GaoXinLibrary.PaySDK.Wechat.Core;

namespace GaoXinLibrary.PaySDK.Wechat.Models;

/// <summary>
/// 微信支付账单下载地址响应
/// <para>https://pay.weixin.qq.com/doc/v3/merchant/4012791860</para>
/// </summary>
public class WechatBillDownloadUrlResponse : WechatPayBaseResponse
{
    /// <summary>哈希类型，固定 SHA1</summary>
    [JsonPropertyName("hash_type")]
    public string? HashType { get; set; }

    /// <summary>哈希值，用于文件校验</summary>
    [JsonPropertyName("hash_value")]
    public string? HashValue { get; set; }

    /// <summary>账单下载地址</summary>
    [JsonPropertyName("download_url")]
    public string DownloadUrl { get; set; } = string.Empty;
}
