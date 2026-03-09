using System.Text.Json.Serialization;

namespace GaoXinLibrary.PaySDK.Wechat.Models;

/// <summary>
/// 微信支付场景信息
/// </summary>
public class WechatPaySceneInfo
{
    /// <summary>用户终端 IP（H5 必填）</summary>
    [JsonPropertyName("payer_client_ip")]
    public string PayerClientIp { get; set; } = string.Empty;

    /// <summary>商户端设备号</summary>
    [JsonPropertyName("device_id")]
    public string? DeviceId { get; set; }

    /// <summary>H5 场景信息</summary>
    [JsonPropertyName("h5_info")]
    public WechatPayH5Info? H5Info { get; set; }
}
