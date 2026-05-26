using System.Text.Json.Serialization;

namespace GaoXinLibrary.PaySDK.Wechat.Models;

/// <summary>
/// 微信支付结算信息
/// </summary>
public class WechatPaySettleInfo
{
    /// <summary>是否指定分账（true/false）</summary>
    [JsonPropertyName("profit_sharing")]
    public bool ProfitSharing { get; set; }
}
