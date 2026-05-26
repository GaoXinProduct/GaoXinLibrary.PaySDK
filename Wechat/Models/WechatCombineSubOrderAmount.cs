using System.Text.Json.Serialization;

namespace GaoXinLibrary.PaySDK.Wechat.Models;

/// <summary>
/// 合单支付子订单金额信息
/// <para>https://pay.weixin.qq.com/doc/v3/merchant/4012791856</para>
/// </summary>
public class WechatCombineSubOrderAmount
{
    /// <summary>子订单总金额，单位分</summary>
    [JsonPropertyName("total_amount")]
    public int TotalAmount { get; set; }

    /// <summary>货币类型，默认 CNY</summary>
    [JsonPropertyName("currency")]
    public string Currency { get; set; } = "CNY";
}
