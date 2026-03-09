using System.Text.Json.Serialization;

namespace GaoXinLibrary.PaySDK.Alipay.Models;

/// <summary>
/// 支付宝资金渠道明细
/// </summary>
public class AlipayFundBill
{
    /// <summary>交易使用的资金渠道（如 ALIPAYACCOUNT / COUPON / POINT / MDISCOUNT 等）</summary>
    [JsonPropertyName("fund_channel")]
    public string? FundChannel { get; set; }

    /// <summary>该支付工具类型所使用的金额</summary>
    [JsonPropertyName("amount")]
    public string? Amount { get; set; }

    /// <summary>渠道实际付款金额</summary>
    [JsonPropertyName("real_amount")]
    public string? RealAmount { get; set; }
}
