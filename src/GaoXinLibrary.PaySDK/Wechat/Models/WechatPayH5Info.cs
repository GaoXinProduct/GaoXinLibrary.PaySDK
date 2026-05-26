using System.Text.Json.Serialization;

namespace GaoXinLibrary.PaySDK.Wechat.Models;

/// <summary>
/// 微信支付 H5 场景信息
/// </summary>
public class WechatPayH5Info
{
    /// <summary>场景类型：iOS / Android / Wap</summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "Wap";

    /// <summary>应用名称</summary>
    [JsonPropertyName("app_name")]
    public string? AppName { get; set; }

    /// <summary>网站 URL</summary>
    [JsonPropertyName("app_url")]
    public string? AppUrl { get; set; }

    /// <summary>iOS 平台 BundleID</summary>
    [JsonPropertyName("bundle_id")]
    public string? BundleId { get; set; }

    /// <summary>Android 平台 PackageName</summary>
    [JsonPropertyName("package_name")]
    public string? PackageName { get; set; }
}
