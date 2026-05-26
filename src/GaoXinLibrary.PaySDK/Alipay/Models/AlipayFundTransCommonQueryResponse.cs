using System.Text.Json.Serialization;
using GaoXinLibrary.PaySDK.Alipay.Core;

namespace GaoXinLibrary.PaySDK.Alipay.Models;

/// <summary>
/// 支付宝资金转账查询响应
/// </summary>
public sealed class AlipayFundTransCommonQueryResponse : AlipayBaseResponse
{
    /// <summary>支付宝转账单据号</summary>
    [JsonPropertyName("order_id")]
    public string? OrderId { get; set; }

    /// <summary>商户转账单号</summary>
    [JsonPropertyName("out_biz_no")]
    public string? OutBizNo { get; set; }

    /// <summary>转账状态</summary>
    [JsonPropertyName("status")]
    public string? Status { get; set; }

    /// <summary>转账金额（元）</summary>
    [JsonPropertyName("trans_amount")]
    public string? TransAmount { get; set; }

    /// <summary>支付时间</summary>
    [JsonPropertyName("pay_date")]
    public string? PayDate { get; set; }

    /// <summary>付款方账号</summary>
    [JsonPropertyName("payer_id")]
    public string? PayerId { get; set; }

    /// <summary>收款方账号</summary>
    [JsonPropertyName("payee_id")]
    public string? PayeeId { get; set; }
}
