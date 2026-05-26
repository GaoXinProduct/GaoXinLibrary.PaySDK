using System.Text.Json.Serialization;
using GaoXinLibrary.PaySDK.Alipay.Core;

namespace GaoXinLibrary.PaySDK.Alipay.Models;

/// <summary>
/// 支付宝分账关系批量查询响应
/// </summary>
public sealed class AlipayTradeRoyaltyRelationBatchQueryResponse : AlipayBaseResponse
{
    /// <summary>当前页码</summary>
    [JsonPropertyName("current_page")]
    public long CurrentPage { get; set; }

    /// <summary>每页条数</summary>
    [JsonPropertyName("page_size")]
    public long PageSize { get; set; }

    /// <summary>总记录数</summary>
    [JsonPropertyName("total_count")]
    public long TotalCount { get; set; }

    /// <summary>总页数</summary>
    [JsonPropertyName("total_page")]
    public long TotalPage { get; set; }

    /// <summary>分账关系列表</summary>
    [JsonPropertyName("royalty_relation_list")]
    public List<AlipayRoyaltyRelationInfo>? RoyaltyRelationList { get; set; }
}
