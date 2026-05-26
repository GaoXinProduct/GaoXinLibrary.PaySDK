using System.Text.Json.Serialization;

namespace GaoXinLibrary.PaySDK.Wechat.Models;

/// <summary>
/// 请求分账回退请求
/// <para>https://pay.weixin.qq.com/doc/v3/merchant/4012791911</para>
/// </summary>
public class WechatProfitSharingReturnRequest
{
    /// <summary>商户回退单号</summary>
    [JsonPropertyName("out_return_no")]
    public string OutReturnNo { get; set; } = string.Empty;

    /// <summary>回退商户号</summary>
    [JsonPropertyName("return_mchid")]
    public string ReturnMchId { get; set; } = string.Empty;

    /// <summary>回退金额，单位分</summary>
    [JsonPropertyName("amount")]
    public int Amount { get; set; }

    /// <summary>回退描述</summary>
    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;
}
