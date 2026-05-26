namespace GaoXinLibrary.PaySDK.Alipay.Models;

/// <summary>
/// 支付宝交易分账请求
/// <para>alipay.trade.order.settle</para>
/// </summary>
public sealed class AlipayTradeOrderSettleContent
{
    /// <summary>支付宝交易号</summary>
    public string? TradeNo { get; set; }

    /// <summary>商户订单号（TradeNo 与 OutTradeNo 二选一）</summary>
    public string? OutTradeNo { get; set; }

    /// <summary>结算请求号（幂等）</summary>
    public string OutRequestNo { get; set; } = string.Empty;

    /// <summary>分账明细</summary>
    public List<AlipayRoyaltyDetail> RoyaltyParameters { get; set; } = [];
}
