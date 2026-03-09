using System.Text.Json.Serialization;

namespace GaoXinLibrary.PaySDK.Alipay.Models;

/// <summary>
/// 退分账明细
/// <para>alipay.trade.fastpay.refund.query 响应中的 refund_royaltys 数组元素</para>
/// </summary>
public class AlipayRefundRoyalty
{
    /// <summary>退分账金额（元）</summary>
    [JsonPropertyName("refund_amount")]
    public string? RefundAmount { get; set; }

    /// <summary>退分账结果码</summary>
    [JsonPropertyName("result_code")]
    public string? ResultCode { get; set; }

    /// <summary>转出人支付宝账号（UID）</summary>
    [JsonPropertyName("trans_out")]
    public string? TransOut { get; set; }

    /// <summary>转出人支付宝账号（邮箱）</summary>
    [JsonPropertyName("trans_out_email")]
    public string? TransOutEmail { get; set; }

    /// <summary>转入人支付宝账号（UID）</summary>
    [JsonPropertyName("trans_in")]
    public string? TransIn { get; set; }

    /// <summary>转入人支付宝账号（邮箱）</summary>
    [JsonPropertyName("trans_in_email")]
    public string? TransInEmail { get; set; }
}
