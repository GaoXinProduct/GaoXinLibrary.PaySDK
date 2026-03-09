using System.Text.Json.Serialization;

namespace GaoXinLibrary.PaySDK.Wechat.Models;

/// <summary>
/// 微信支付 JSAPI 下单请求
/// <para>https://pay.weixin.qq.com/doc/v3/merchant/4012062524</para>
/// </summary>
public class WechatJsapiOrderRequest : WechatCreateOrderRequestBase
{
    /// <summary>支付者信息（JSAPI 必填）</summary>
    [JsonPropertyName("payer")]
    public WechatPayPayer Payer { get; set; } = new();
}
