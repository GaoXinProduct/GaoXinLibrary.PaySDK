using System.Text.Json.Serialization;

namespace GaoXinLibrary.PaySDK.Wechat.Models;

/// <summary>
/// 合单支付下单请求基类
/// <para>适用于 JSAPI / APP / H5 / Native 四种支付方式</para>
/// <para>https://pay.weixin.qq.com/doc/v3/merchant/4012791856</para>
/// </summary>
public class WechatCombineOrderRequest
{
    /// <summary>合单商户 appid</summary>
    [JsonPropertyName("combine_appid")]
    public string CombineAppId { get; set; } = string.Empty;

    /// <summary>合单商户号</summary>
    [JsonPropertyName("combine_mchid")]
    public string CombineMchId { get; set; } = string.Empty;

    /// <summary>合单商户订单号</summary>
    [JsonPropertyName("combine_out_trade_no")]
    public string CombineOutTradeNo { get; set; } = string.Empty;

    /// <summary>子订单信息列表（最多 10 笔）</summary>
    [JsonPropertyName("sub_orders")]
    public List<WechatCombineSubOrder> SubOrders { get; set; } = new();

    /// <summary>支付者信息（JSAPI 必填）</summary>
    [JsonPropertyName("combine_payer_info")]
    public WechatCombinePayerInfo? CombinePayerInfo { get; set; }

    /// <summary>通知 URL</summary>
    [JsonPropertyName("notify_url")]
    public string NotifyUrl { get; set; } = string.Empty;

    /// <summary>场景信息（H5 场景必填）</summary>
    [JsonPropertyName("scene_info")]
    public WechatPaySceneInfo? SceneInfo { get; set; }

    /// <summary>交易结束时间（rfc3339）</summary>
    [JsonPropertyName("time_expire")]
    public string? TimeExpire { get; set; }
}
