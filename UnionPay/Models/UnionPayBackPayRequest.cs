namespace GaoXinLibrary.PaySDK.UnionPay.Models;

/// <summary>
/// 银联后台消费请求参数（适用于无跳转支付、签约支付、云闪付等后台交易）
/// <para>
/// 无跳转支付 bizType=000301；签约支付通过 contractNo 扣款；云闪付通过 tokenPayData 扣款。
/// </para>
/// </summary>
public class UnionPayBackPayRequest
{
    /// <summary>交易类型，固定 01（消费）</summary>
    public string TxnType { get; set; } = "01";

    /// <summary>交易子类，01 = 自助消费</summary>
    public string TxnSubType { get; set; } = "01";

    /// <summary>
    /// 产品类型
    /// <para>000301 = 无跳转支付；000201 = 网关支付；000000 = 通用</para>
    /// </summary>
    public string BizType { get; set; } = "000301";

    /// <summary>渠道类型，07 = PC，08 = 手机</summary>
    public string ChannelType { get; set; } = "07";

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

    /// <summary>
    /// Token 支付数据（云闪付无感支付使用，JSON 格式）
    /// <para>包含 trId（签约协议号）等信息</para>
    /// </summary>
    public string? TokenPayData { get; set; }

    /// <summary>
    /// 签约协议号（签约支付使用）
    /// </summary>
    public string? ContractNo { get; set; }

    /// <summary>
    /// 卡号（无跳转支付使用，需加密）
    /// </summary>
    public string? AccNo { get; set; }

    /// <summary>
    /// 持卡人信息（无跳转支付使用，需加密）
    /// <para>包含手机号、CVN2、有效期等，JSON 格式</para>
    /// </summary>
    public string? CustomerInfo { get; set; }

    /// <summary>
    /// 短信验证码（无跳转支付使用）
    /// </summary>
    public string? SmsCode { get; set; }

    /// <summary>附加信息（原样返回）</summary>
    public string? ReqReserved { get; set; }
}
