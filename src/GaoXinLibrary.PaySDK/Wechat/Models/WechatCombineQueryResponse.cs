using System.Text.Json.Serialization;
using GaoXinLibrary.PaySDK.Wechat.Core;

namespace GaoXinLibrary.PaySDK.Wechat.Models;

/// <summary>
/// 合单支付查询响应
/// <para>https://pay.weixin.qq.com/doc/v3/merchant/4012791857</para>
/// </summary>
public class WechatCombineQueryResponse : WechatPayBaseResponse
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

    /// <summary>子订单信息列表</summary>
    [JsonPropertyName("sub_orders")]
    public List<WechatCombineSubOrderResponse>? SubOrders { get; set; }

    /// <summary>支付者信息</summary>
    [JsonPropertyName("combine_payer_info")]
    public WechatCombinePayerInfo? CombinePayerInfo { get; set; }

    /// <summary>微信支付合单订单号</summary>
    [JsonPropertyName("transaction_id")]
    public string? TransactionId { get; set; }

    /// <summary>场景信息</summary>
    [JsonPropertyName("scene_info")]
    public WechatPaySceneInfo? SceneInfo { get; set; }
}
