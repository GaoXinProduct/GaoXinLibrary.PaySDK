namespace GaoXinLibrary.PaySDK.UnionPay.Models;

/// <summary>
/// 银联实名认证请求参数
/// <para>
/// 实名认证交易主要是为了验证银行卡验证信息及身份信息
/// 如证件类型、证件号码、姓名、密码、CVN2、有效期、手机号等与银行卡号的一致性。
/// </para>
/// </summary>
public class UnionPayRealNameAuthRequest
{
    /// <summary>交易类型，固定 72（实名认证）</summary>
    public string TxnType { get; set; } = "72";

    /// <summary>交易子类，固定 00</summary>
    public string TxnSubType { get; set; } = "00";

    /// <summary>产品类型，固定 000000</summary>
    public string BizType { get; set; } = "000000";

    /// <summary>商户订单号</summary>
    public string OrderId { get; set; } = string.Empty;

    /// <summary>交易时间，格式 yyyyMMddHHmmss</summary>
    public string TxnTime { get; set; } = string.Empty;

    /// <summary>账号（银行卡号），需加密上送</summary>
    public string AccNo { get; set; } = string.Empty;

    /// <summary>
    /// 客户信息域（customerInfo）
    /// <para>
    /// 包含证件类型、证件号码、姓名、手机号等信息的 Base64 编码字符串。
    /// 如：certifTp=01&amp;certifId=xxx&amp;customerNm=xxx&amp;phoneNo=xxx
    /// </para>
    /// </summary>
    public string? CustomerInfo { get; set; }

    /// <summary>
    /// 交易金额（分），实名认证可填 0
    /// </summary>
    public string TxnAmt { get; set; } = "0";

    /// <summary>
    /// 请求方保留域（透传字段）
    /// </summary>
    public string? ReqReserved { get; set; }
}
