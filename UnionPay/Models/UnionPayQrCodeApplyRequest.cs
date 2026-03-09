namespace GaoXinLibrary.PaySDK.UnionPay.Models;

/// <summary>
/// 银联申请二维码（主扫）请求参数
/// <para>https://open.unionpay.com/tjweb/api/dictionary?index=1&amp;catId=10&amp;apiSvcId=468</para>
/// </summary>
public class UnionPayQrCodeApplyRequest
{
    /// <summary>交易类型，固定 01（消费）</summary>
    public string TxnType { get; set; } = "01";

    /// <summary>交易子类，07 = 申请消费二维码（主扫）</summary>
    public string TxnSubType { get; set; } = "07";

    /// <summary>产品类型，000000 = 二维码支付</summary>
    public string BizType { get; set; } = "000000";

    /// <summary>商户订单号（唯一，8~32 位字母数字）</summary>
    public string OrderId { get; set; } = string.Empty;

    /// <summary>交易时间，格式 yyyyMMddHHmmss</summary>
    public string TxnTime { get; set; } = string.Empty;

    /// <summary>交易金额（分）</summary>
    public string TxnAmt { get; set; } = string.Empty;

    /// <summary>订单描述</summary>
    public string OrderDesc { get; set; } = string.Empty;

    /// <summary>后台通知地址</summary>
    public string BackUrl { get; set; } = string.Empty;

    /// <summary>超时时间（秒数）</summary>
    public string? PayTimeout { get; set; }

    /// <summary>附加信息（原样返回）</summary>
    public string? ReqReserved { get; set; }
}
