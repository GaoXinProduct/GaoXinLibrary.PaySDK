using System.Text.Json.Serialization;
using GaoXinLibrary.PaySDK.Wechat.Core;

namespace GaoXinLibrary.PaySDK.Wechat.Models;

/// <summary>
/// 查询分账结果响应
/// <para>https://pay.weixin.qq.com/doc/v3/merchant/4012791908</para>
/// </summary>
public class WechatProfitSharingQueryResponse : WechatPayBaseResponse
{
    /// <summary>微信支付订单号</summary>
    [JsonPropertyName("transaction_id")]
    public string TransactionId { get; set; } = string.Empty;

    /// <summary>商户分账单号</summary>
    [JsonPropertyName("out_order_no")]
    public string OutOrderNo { get; set; } = string.Empty;

    /// <summary>微信分账单号</summary>
    [JsonPropertyName("order_id")]
    public string OrderId { get; set; } = string.Empty;

    /// <summary>分账单状态：PROCESSING / FINISHED</summary>
    [JsonPropertyName("state")]
    public string State { get; set; } = string.Empty;

    /// <summary>分账接收方列表</summary>
    [JsonPropertyName("receivers")]
    public List<WechatProfitSharingReceiver>? Receivers { get; set; }
}
