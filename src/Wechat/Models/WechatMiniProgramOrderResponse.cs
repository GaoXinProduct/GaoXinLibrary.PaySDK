using System.Text.Json.Serialization;
using GaoXinLibrary.PaySDK.Wechat.Core;

namespace GaoXinLibrary.PaySDK.Wechat.Models;

/// <summary>
/// 微信支付小程序下单响应
/// </summary>
public class WechatMiniProgramOrderResponse : WechatPayBaseResponse
{
    /// <summary>预支付交易会话标识（prepay_id）</summary>
    [JsonPropertyName("prepay_id")]
    public string PrepayId { get; set; } = string.Empty;
}
