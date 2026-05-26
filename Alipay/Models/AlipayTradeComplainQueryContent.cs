using System.Text.Json.Serialization;

namespace GaoXinLibrary.PaySDK.Alipay.Models;

/// <summary>
/// 支付宝交易投诉查询请求
/// <para>alipay.merchant.tradecomplain.batchquery</para>
/// </summary>
public sealed class AlipayTradeComplainQueryContent
{
    /// <summary>投诉状态（可选）：WAIT_FEEDBACK / FEEDBACKED / ALL（不传默认全部）</summary>
    [JsonPropertyName("status")]
    public string? Status { get; set; }

    /// <summary>页码，默认 1</summary>
    [JsonPropertyName("page_num")]
    public int PageNum { get; set; } = 1;

    /// <summary>每页条数，默认 20</summary>
    [JsonPropertyName("page_size")]
    public int PageSize { get; set; } = 20;

    /// <summary>查询开始时间（格式 yyyy-MM-dd HH:mm:ss）</summary>
    [JsonPropertyName("start_time")]
    public string? StartTime { get; set; }

    /// <summary>查询结束时间（格式 yyyy-MM-dd HH:mm:ss）</summary>
    [JsonPropertyName("end_time")]
    public string? EndTime { get; set; }
}
