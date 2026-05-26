using System.Text.Json.Serialization;
using GaoXinLibrary.PaySDK.Alipay.Core;

namespace GaoXinLibrary.PaySDK.Alipay.Models;

/// <summary>
/// 支付宝交易分账查询响应
/// </summary>
public sealed class AlipayTradeOrderSettleQueryResponse : AlipayBaseResponse
{
    /// <summary>支付宝交易号</summary>
    [JsonPropertyName("trade_no")]
    public string? TradeNo { get; set; }

    /// <summary>商户订单号</summary>
    [JsonPropertyName("out_trade_no")]
    public string? OutTradeNo { get; set; }

    /// <summary>分账金额（元）</summary>
    [JsonPropertyName("settle_amount")]
    public string? SettleAmount { get; set; }

    /// <summary>分账明细列表</summary>
    [JsonPropertyName("royalty_detail_list")]
    public List<AlipayRoyaltyDetail>? RoyaltyDetailList { get; set; }
}
