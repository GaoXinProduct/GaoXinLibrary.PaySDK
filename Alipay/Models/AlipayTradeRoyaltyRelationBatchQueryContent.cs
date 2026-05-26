using System.Text.Json.Serialization;

namespace GaoXinLibrary.PaySDK.Alipay.Models;

/// <summary>
/// 支付宝分账关系批量查询请求
/// <para>alipay.trade.royalty.relation.batchquery</para>
/// </summary>
public sealed class AlipayTradeRoyaltyRelationBatchQueryContent
{
    /// <summary>外部请求号（幂等）</summary>
    [JsonPropertyName("out_request_no")]
    public string? OutRequestNo { get; set; }

    /// <summary>页码，默认 1</summary>
    [JsonPropertyName("page_num")]
    public int PageNum { get; set; } = 1;

    /// <summary>每页条数，默认 20</summary>
    [JsonPropertyName("page_size")]
    public int PageSize { get; set; } = 20;
}
