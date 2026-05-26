using System.Text.Json.Serialization;

namespace GaoXinLibrary.PaySDK.Alipay.Models;

/// <summary>
/// 交易投诉信息
/// </summary>
public sealed class AlipayTradeComplainInfo
{
    /// <summary>支付宝侧投诉单号</summary>
    [JsonPropertyName("complain_event_id")]
    public string? ComplainEventId { get; set; }

    /// <summary>投诉状态</summary>
    [JsonPropertyName("status")]
    public string? Status { get; set; }

    /// <summary>投诉时间</summary>
    [JsonPropertyName("gmt_create")]
    public string? GmtCreate { get; set; }

    /// <summary>商户订单号</summary>
    [JsonPropertyName("out_trade_no")]
    public string? OutTradeNo { get; set; }

    /// <summary>支付宝交易号</summary>
    [JsonPropertyName("trade_no")]
    public string? TradeNo { get; set; }

    /// <summary>投诉金额（元）</summary>
    [JsonPropertyName("amount")]
    public string? Amount { get; set; }

    /// <summary>投诉人联系方式</summary>
    [JsonPropertyName("contact")]
    public string? Contact { get; set; }

    /// <summary>投诉内容</summary>
    [JsonPropertyName("content")]
    public string? Content { get; set; }

    /// <summary>商户回复内容</summary>
    [JsonPropertyName("reply")]
    public string? Reply { get; set; }

    /// <summary>投诉人提供的图片地址列表</summary>
    [JsonPropertyName("images")]
    public List<string>? Images { get; set; }
}
