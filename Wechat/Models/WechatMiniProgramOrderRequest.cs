using System.Text.Json.Serialization;

namespace GaoXinLibrary.PaySDK.Wechat.Models;

/// <summary>
/// 微信支付小程序下单请求
/// <para>https://pay.weixin.qq.com/doc/v3/merchant/4012062524</para>
/// </summary>
public class WechatMiniProgramOrderRequest : WechatCreateOrderRequestBase
{
    /// <summary>支付者信息（小程序必填）</summary>
    [JsonPropertyName("payer")]
    public WechatPayPayer Payer { get; set; } = new();
}
