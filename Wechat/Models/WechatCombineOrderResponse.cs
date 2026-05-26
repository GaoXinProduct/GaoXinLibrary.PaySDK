using System.Text.Json.Serialization;
using GaoXinLibrary.PaySDK.Wechat.Core;

namespace GaoXinLibrary.PaySDK.Wechat.Models;

/// <summary>
/// 合单支付下单响应
/// <para>https://pay.weixin.qq.com/doc/v3/merchant/4012791856</para>
/// </summary>
public class WechatCombineOrderResponse : WechatPayBaseResponse
{
    /// <summary>预支付交易会话标识（prepay_id，有效期为 2 小时）</summary>
    [JsonPropertyName("prepay_id")]
    public string PrepayId { get; set; } = string.Empty;
}
