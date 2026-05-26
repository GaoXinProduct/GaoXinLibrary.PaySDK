using System.Text.Json.Serialization;

namespace GaoXinLibrary.PaySDK.Wechat.Models;

/// <summary>
/// 请求分账请求
/// <para>https://pay.weixin.qq.com/doc/v3/merchant/4012791906</para>
/// </summary>
public class WechatProfitSharingRequest
{
    /// <summary>微信分配的公众账号 ID / 服务商 appid</summary>
    [JsonPropertyName("appid")]
    public string AppId { get; set; } = string.Empty;

    /// <summary>微信支付订单号</summary>
    [JsonPropertyName("transaction_id")]
    public string TransactionId { get; set; } = string.Empty;

    /// <summary>商户分账单号</summary>
    [JsonPropertyName("out_order_no")]
    public string OutOrderNo { get; set; } = string.Empty;

    /// <summary>分账接收方列表</summary>
    [JsonPropertyName("receivers")]
    public List<WechatProfitSharingReceiver>? Receivers { get; set; }

    /// <summary>是否解冻剩余未分账资金</summary>
    [JsonPropertyName("unfreeze_unsplit")]
    public bool UnfreezeUnsplit { get; set; }
}
