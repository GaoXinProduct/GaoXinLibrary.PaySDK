using System.Text.Json.Serialization;
using GaoXinLibrary.PaySDK.Wechat.Core;

namespace GaoXinLibrary.PaySDK.Wechat.Models;

/// <summary>
/// 微信支付 H5 下单响应
/// </summary>
public class WechatH5OrderResponse : WechatPayBaseResponse
{
    /// <summary>支付跳转链接（h5_url）</summary>
    [JsonPropertyName("h5_url")]
    public string H5Url { get; set; } = string.Empty;
}
