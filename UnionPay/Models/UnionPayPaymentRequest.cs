namespace GaoXinLibrary.PaySDK.UnionPay.Models;

/// <summary>
/// 银联代付请求参数
/// <para>https://open.unionpay.com/tjweb/support/doc/online/3/125</para>
/// </summary>
public class UnionPayPaymentRequest
{
    /// <summary>交易类型，固定 01（消费）</summary>
    public string TxnType { get; set; } = "01";

    /// <summary>交易子类，01 = 自助消费</summary>
    public string TxnSubType { get; set; } = "01";

    /// <summary>产品类型，代付业务编码</summary>
    public string BizType { get; set; } = "000802";

    /// <summary>渠道类型，07 = PC，08 = 手机</summary>
    public string ChannelType { get; set; } = "07";

    /// <summary>商户订单号（唯一，8~32 位字母数字）</summary>
    public string OrderId { get; set; } = string.Empty;

    /// <summary>交易时间，格式 yyyyMMddHHmmss</summary>
    public string TxnTime { get; set; } = string.Empty;

    /// <summary>交易金额（分）</summary>
    public string TxnAmt { get; set; } = string.Empty;

    /// <summary>卡号（需加密）</summary>
    public string? AccNo { get; set; }

    /// <summary>持卡人信息（需加密，JSON 格式）</summary>
    public string? CustomerInfo { get; set; }

    /// <summary>后台通知地址</summary>
    public string BackUrl { get; set; } = string.Empty;

    /// <summary>附加信息</summary>
    public string? ReqReserved { get; set; }
}
