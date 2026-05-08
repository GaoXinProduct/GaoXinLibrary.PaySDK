using GaoXinLibrary.PaySDK.Alipay.Core;

namespace GaoXinLibrary.PaySDK.Alipay.Models;

/// <summary>
/// 支付宝商家转账响应
/// </summary>
public sealed class AlipayFundTransUniTransferResponse : AlipayBaseResponse
{
    /// <summary>支付宝转账单据号</summary>
    public string? OrderId { get; set; }

    /// <summary>商户转账单号</summary>
    public string? OutBizNo { get; set; }

    /// <summary>订单状态</summary>
    public string? Status { get; set; }
}
