using System.Text.Json.Serialization;

namespace GaoXinLibrary.PaySDK.Wechat.Models;

/// <summary>
/// 微信支付付款人信息
/// </summary>
public class WechatPayPayer
{
    /// <summary>用户在商户 appid 下的唯一标识（openid）</summary>
    [JsonPropertyName("openid")]
    public string OpenId { get; set; } = string.Empty;
}
