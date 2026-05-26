using System.Text.Json.Serialization;
using GaoXinLibrary.PaySDK.Alipay.Core;

namespace GaoXinLibrary.PaySDK.Alipay.Models;

/// <summary>
/// 支付宝交易投诉查询响应
/// </summary>
public sealed class AlipayTradeComplainQueryResponse : AlipayBaseResponse
{
    /// <summary>当前页码</summary>
    [JsonPropertyName("page_num")]
    public int PageNum { get; set; }

    /// <summary>每页条数</summary>
    [JsonPropertyName("page_size")]
    public int PageSize { get; set; }

    /// <summary>总条数</summary>
    [JsonPropertyName("total")]
    public int Total { get; set; }

    /// <summary>总页数</summary>
    [JsonPropertyName("total_page")]
    public int TotalPage { get; set; }

    /// <summary>投诉信息列表</summary>
    [JsonPropertyName("trade_complain_infos")]
    public List<AlipayTradeComplainInfo>? TradeComplainInfos { get; set; }
}
