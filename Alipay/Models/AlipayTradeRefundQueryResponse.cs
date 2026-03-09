using System.Text.Json.Serialization;
using GaoXinLibrary.PaySDK.Alipay.Core;

namespace GaoXinLibrary.PaySDK.Alipay.Models;

/// <summary>
/// 支付宝退款查询响应
/// <para>alipay.trade.fastpay.refund.query</para>
/// </summary>
public class AlipayTradeRefundQueryResponse : AlipayBaseResponse
{
    /// <summary>商户订单号</summary>
    [JsonPropertyName("out_trade_no")]
    public string? OutTradeNo { get; set; }

    /// <summary>支付宝交易号</summary>
    [JsonPropertyName("trade_no")]
    public string? TradeNo { get; set; }

    /// <summary>商户退款请求号</summary>
    [JsonPropertyName("out_request_no")]
    public string? OutRequestNo { get; set; }

    /// <summary>退款原因</summary>
    [JsonPropertyName("refund_reason")]
    public string? RefundReason { get; set; }

    /// <summary>订单总金额（元）</summary>
    [JsonPropertyName("total_amount")]
    public string? TotalAmount { get; set; }

    /// <summary>本次退款的金额（元）</summary>
    [JsonPropertyName("refund_amount")]
    public string? RefundAmount { get; set; }

    /// <summary>退款时间（yyyy-MM-dd HH:mm:ss）</summary>
    [JsonPropertyName("gmt_refund_pay")]
    public string? GmtRefundPay { get; set; }

    /// <summary>退款使用的资金渠道列表</summary>
    [JsonPropertyName("refund_detail_item_list")]
    public List<AlipayFundBill>? RefundDetailItemList { get; set; }

    /// <summary>退分账明细列表</summary>
    [JsonPropertyName("refund_royaltys")]
    public List<AlipayRefundRoyalty>? RefundRoyaltys { get; set; }

    /// <summary>本次商户实际退回金额（元）</summary>
    [JsonPropertyName("send_back_fee")]
    public string? SendBackFee { get; set; }

    /// <summary>退款清算编号，用于清算对账</summary>
    [JsonPropertyName("refund_settlement_id")]
    public string? RefundSettlementId { get; set; }

    /// <summary>本次退款中买家实际退款金额（元）</summary>
    [JsonPropertyName("present_refund_buyer_amount")]
    public string? PresentRefundBuyerAmount { get; set; }

    /// <summary>本次退款中买家优惠券退款金额（元）</summary>
    [JsonPropertyName("present_refund_discount_amount")]
    public string? PresentRefundDiscountAmount { get; set; }

    /// <summary>本次退款中商家优惠退款金额（元）</summary>
    [JsonPropertyName("present_refund_mdiscount_amount")]
    public string? PresentRefundMdiscountAmount { get; set; }
}
