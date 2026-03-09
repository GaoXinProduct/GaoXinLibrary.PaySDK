using System.Text.Json.Serialization;
using GaoXinLibrary.PaySDK.Wechat.Core;

namespace GaoXinLibrary.PaySDK.Wechat.Models;

/// <summary>
/// 微信支付 Native 下单响应
/// </summary>
public class WechatNativeOrderResponse : WechatPayBaseResponse
{
    /// <summary>二维码链接（code_url），用于生成二维码</summary>
    [JsonPropertyName("code_url")]
    public string CodeUrl { get; set; } = string.Empty;
}
