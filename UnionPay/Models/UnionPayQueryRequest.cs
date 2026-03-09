namespace GaoXinLibrary.PaySDK.UnionPay.Models;

/// <summary>
/// 银联订单查询请求参数
/// <para>https://open.unionpay.com/tjweb/support/doc/online/3/125</para>
/// </summary>
public class UnionPayQueryRequest
{
    /// <summary>交易类型，固定 00（查询）</summary>
    public string TxnType { get; set; } = "00";

    /// <summary>交易子类，固定 00</summary>
    public string TxnSubType { get; set; } = "00";

    /// <summary>产品类型，000000 = 全渠道通用查询</summary>
    public string BizType { get; set; } = "000000";

    /// <summary>商户订单号（原始交易的 orderId）</summary>
    public string OrderId { get; set; } = string.Empty;

    /// <summary>原始交易时间（格式 yyyyMMddHHmmss）</summary>
    public string TxnTime { get; set; } = string.Empty;
}
