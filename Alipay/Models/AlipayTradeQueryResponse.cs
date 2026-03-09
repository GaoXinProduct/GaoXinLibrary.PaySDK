using System.Text.Json.Serialization;
using GaoXinLibrary.PaySDK.Alipay.Core;

namespace GaoXinLibrary.PaySDK.Alipay.Models;

/// <summary>
/// 支付宝查询订单响应
/// </summary>
public class AlipayTradeQueryResponse : AlipayBaseResponse
{
    /// <summary>支付宝交易号</summary>
    [JsonPropertyName("trade_no")]
    public string? TradeNo { get; set; }

    /// <summary>商户订单号</summary>
    [JsonPropertyName("out_trade_no")]
    public string? OutTradeNo { get; set; }

    /// <summary>买家支付宝账号</summary>
    [JsonPropertyName("buyer_logon_id")]
    public string? BuyerLogonId { get; set; }

    /// <summary>
    /// 交易状态：WAIT_BUYER_PAY / TRADE_CLOSED / TRADE_SUCCESS / TRADE_FINISHED
    /// </summary>
    [JsonPropertyName("trade_status")]
    public string? TradeStatus { get; set; }

    /// <summary>交易总金额</summary>
    [JsonPropertyName("total_amount")]
    public string? TotalAmount { get; set; }

    /// <summary>实付款金额</summary>
    [JsonPropertyName("buyer_pay_amount")]
    public string? BuyerPayAmount { get; set; }

    /// <summary>买家用户 id</summary>
    [JsonPropertyName("buyer_user_id")]
    public string? BuyerUserId { get; set; }

    /// <summary>交易付款时间</summary>
    [JsonPropertyName("send_pay_date")]
    public string? SendPayDate { get; set; }
}
