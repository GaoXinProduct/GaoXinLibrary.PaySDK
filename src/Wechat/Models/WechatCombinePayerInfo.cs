using System.Text.Json.Serialization;

namespace GaoXinLibrary.PaySDK.Wechat.Models;

/// <summary>
/// 合单支付付款人信息
/// <para>https://pay.weixin.qq.com/doc/v3/merchant/4012791856</para>
/// </summary>
public class WechatCombinePayerInfo
{
    /// <summary>用户标识（openid），JSAPI 必填</summary>
    [JsonPropertyName("openid")]
    public string OpenId { get; set; } = string.Empty;
}
