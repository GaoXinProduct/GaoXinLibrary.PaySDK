namespace GaoXinLibrary.PaySDK.Alipay.Models;

/// <summary>
/// 支付宝分账明细
/// </summary>
public sealed class AlipayRoyaltyDetail
{
    /// <summary>分账方账户类型，默认 userId</summary>
    public string TransInType { get; set; } = "userId";

    /// <summary>分账方账号</summary>
    public string TransIn { get; set; } = string.Empty;

    /// <summary>分账金额（元）</summary>
    public string Amount { get; set; } = string.Empty;

    /// <summary>分账描述</summary>
    public string? Desc { get; set; }
}
