namespace GaoXinLibrary.PaySDK.UnionPay.Models;

/// <summary>
/// 银联预授权撤销请求参数
/// <para>https://open.unionpay.com/tjweb/support/doc/online/3/125</para>
/// </summary>
public class UnionPayPreAuthUndoRequest
{
    /// <summary>交易类型，固定 32（预授权撤销）</summary>
    public string TxnType { get; set; } = "32";

    /// <summary>交易子类，固定 00</summary>
    public string TxnSubType { get; set; } = "00";

    /// <summary>产品类型，与原始交易保持一致</summary>
    public string BizType { get; set; } = "000301";

    /// <summary>商户订单号（唯一，8~32 位字母数字）</summary>
    public string OrderId { get; set; } = string.Empty;

    /// <summary>交易时间，格式 yyyyMMddHHmmss</summary>
    public string TxnTime { get; set; } = string.Empty;

    /// <summary>交易金额（分）</summary>
    public string TxnAmt { get; set; } = string.Empty;

    /// <summary>原始预授权流水号（queryId）</summary>
    public string OrigQueryId { get; set; } = string.Empty;

    /// <summary>后台通知地址</summary>
    public string BackUrl { get; set; } = string.Empty;

    /// <summary>附加信息</summary>
    public string? ReqReserved { get; set; }
}
