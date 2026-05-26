using System.Text.Json.Serialization;
using GaoXinLibrary.PaySDK.Wechat.Core;

namespace GaoXinLibrary.PaySDK.Wechat.Models;

/// <summary>
/// 查询分账回退结果响应
/// <para>https://pay.weixin.qq.com/doc/v3/merchant/4012791912</para>
/// </summary>
public class WechatProfitSharingReturnQueryResponse : WechatPayBaseResponse
{
    /// <summary>商户回退单号</summary>
    [JsonPropertyName("out_return_no")]
    public string OutReturnNo { get; set; } = string.Empty;

    /// <summary>微信分账单号</summary>
    [JsonPropertyName("order_id")]
    public string OrderId { get; set; } = string.Empty;

    /// <summary>商户分账单号</summary>
    [JsonPropertyName("out_order_no")]
    public string OutOrderNo { get; set; } = string.Empty;

    /// <summary>微信回退单号</summary>
    [JsonPropertyName("return_id")]
    public string ReturnId { get; set; } = string.Empty;

    /// <summary>回退金额，单位分</summary>
    [JsonPropertyName("amount")]
    public int Amount { get; set; }

    /// <summary>回退结果：PROCESSING / SUCCESS / FAILED</summary>
    [JsonPropertyName("result")]
    public string Result { get; set; } = string.Empty;

    /// <summary>失败原因</summary>
    [JsonPropertyName("fail_reason")]
    public string? FailReason { get; set; }

    /// <summary>创建时间（rfc3339）</summary>
    [JsonPropertyName("create_time")]
    public string? CreateTime { get; set; }

    /// <summary>完成时间（rfc3339）</summary>
    [JsonPropertyName("finish_time")]
    public string? FinishTime { get; set; }
}
