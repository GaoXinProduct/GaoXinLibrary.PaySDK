using System.Text.Json.Serialization;
using GaoXinLibrary.PaySDK.Alipay.Core;

namespace GaoXinLibrary.PaySDK.Alipay.Models;

/// <summary>
/// 支付宝退款响应
/// </summary>
public class AlipayTradeRefundResponse : AlipayBaseResponse
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
    /// 本次退款是否发生了资金变化，Y 表示是，N 表示否
    /// </summary>
    [JsonPropertyName("fund_change")]
    public string? FundChange { get; set; }

    /// <summary>退款总金额</summary>
    [JsonPropertyName("refund_fee")]
    public string? RefundFee { get; set; }

    /// <summary>退款时间</summary>
    [JsonPropertyName("gmt_refund_pay")]
    public string? GmtRefundPay { get; set; }

    /// <summary>退款使用的资金渠道</summary>
    [JsonPropertyName("refund_detail_item_list")]
    public List<AlipayFundBill>? RefundDetailItemList { get; set; }
}
