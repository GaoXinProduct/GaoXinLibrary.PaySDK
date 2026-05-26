using System.Text.Json.Serialization;

namespace GaoXinLibrary.PaySDK.Alipay.Models;

/// <summary>
/// 分账关系信息
/// <para>alipay.trade.royalty.relation.batchquery 响应中的 royalty_relation_list 数组元素</para>
/// </summary>
public sealed class AlipayRoyaltyRelationInfo
{
    /// <summary>分账关系类型</summary>
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    /// <summary>分出方账号</summary>
    [JsonPropertyName("trans_out")]
    public string? TransOut { get; set; }

    /// <summary>分入方账号</summary>
    [JsonPropertyName("trans_in")]
    public string? TransIn { get; set; }

    /// <summary>分入方账户类型</summary>
    [JsonPropertyName("trans_in_type")]
    public string? TransInType { get; set; }

    /// <summary>分账比例（百分比，如 "20.0"）</summary>
    [JsonPropertyName("trans_in_ratio")]
    public string? TransInRatio { get; set; }

    /// <summary>分账关系状态</summary>
    [JsonPropertyName("status")]
    public string? Status { get; set; }
}
