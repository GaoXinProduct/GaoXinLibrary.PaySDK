using System.Text.Json.Serialization;

namespace GaoXinLibrary.PaySDK.Wechat.Models;

/// <summary>
/// 分账接收方信息
/// <para>https://pay.weixin.qq.com/doc/v3/merchant/4012791906</para>
/// </summary>
public class WechatProfitSharingReceiver
{
    /// <summary>分账接收方类型：MERCHANT_ID / PERSONAL_OPENID / PERSONAL_SUB_OPENID</summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    /// <summary>分账接收方账号</summary>
    [JsonPropertyName("account")]
    public string Account { get; set; } = string.Empty;

    /// <summary>分账金额，单位分</summary>
    [JsonPropertyName("amount")]
    public int Amount { get; set; }

    /// <summary>分账描述</summary>
    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    /// <summary>分账接收方姓名（个人 openid 类型可选）</summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}
