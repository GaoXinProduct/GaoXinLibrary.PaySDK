namespace GaoXinLibrary.PaySDK.Alipay.Models;

/// <summary>
/// 支付宝分账关系绑定请求
/// <para>alipay.trade.royalty.relation.bind</para>
/// </summary>
public sealed class AlipayTradeRoyaltyRelationBindContent
{
    /// <summary>外部请求号（幂等）</summary>
    public string OutRequestNo { get; set; } = string.Empty;

    /// <summary>分出方账号（通常为商户 PID）</summary>
    public string TransOut { get; set; } = string.Empty;

    /// <summary>分入方账号</summary>
    public string TransIn { get; set; } = string.Empty;

    /// <summary>分入方账户类型，默认 userId</summary>
    public string TransInType { get; set; } = "userId";

    /// <summary>关系类型，默认 transfer</summary>
    public string Type { get; set; } = "transfer";

    /// <summary>分账描述</summary>
    public string? Desc { get; set; }
}
