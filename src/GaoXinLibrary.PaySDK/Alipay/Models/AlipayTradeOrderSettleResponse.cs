using GaoXinLibrary.PaySDK.Alipay.Core;

namespace GaoXinLibrary.PaySDK.Alipay.Models;

/// <summary>
/// 支付宝交易分账响应
/// </summary>
public sealed class AlipayTradeOrderSettleResponse : AlipayBaseResponse
{
    /// <summary>支付宝交易号</summary>
    public string? TradeNo { get; set; }

    /// <summary>商户订单号</summary>
    public string? OutTradeNo { get; set; }
}
