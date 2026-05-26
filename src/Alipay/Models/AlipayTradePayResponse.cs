using System.Text.Json.Serialization;
using GaoXinLibrary.PaySDK.Alipay.Core;

namespace GaoXinLibrary.PaySDK.Alipay.Models;

/// <summary>
/// 支付宝当面付（条码支付）响应
/// </summary>
public class AlipayTradePayResponse : AlipayBaseResponse
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

    /// <summary>交易支付时间</summary>
    [JsonPropertyName("gmt_payment")]
    public string? GmtPayment { get; set; }

    /// <summary>实付款金额，单位为元，两位小数</summary>
    [JsonPropertyName("buyer_pay_amount")]
    public string? BuyerPayAmount { get; set; }

    /// <summary>积分支付金额</summary>
    [JsonPropertyName("point_amount")]
    public string? PointAmount { get; set; }

    /// <summary>优惠券支付金额</summary>
    [JsonPropertyName("invoice_amount")]
    public string? InvoiceAmount { get; set; }

    /// <summary>总金额</summary>
    [JsonPropertyName("total_amount")]
    public string? TotalAmount { get; set; }
}
