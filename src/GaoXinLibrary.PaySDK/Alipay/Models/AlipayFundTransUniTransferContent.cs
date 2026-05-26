namespace GaoXinLibrary.PaySDK.Alipay.Models;

/// <summary>
/// 支付宝商家转账请求
/// <para>alipay.fund.trans.uni.transfer</para>
/// </summary>
public sealed class AlipayFundTransUniTransferContent
{
    /// <summary>商户侧唯一业务单号</summary>
    public string OutBizNo { get; set; } = string.Empty;

    /// <summary>转账金额（元）</summary>
    public string TransAmount { get; set; } = string.Empty;

    /// <summary>收款方账户标识</summary>
    public string PayeeInfoIdentity { get; set; } = string.Empty;

    /// <summary>收款方账户标识类型，默认 ALIPAY_USER_ID</summary>
    public string PayeeInfoIdentityType { get; set; } = "ALIPAY_USER_ID";

    /// <summary>转账业务场景，默认 DIRECT_TRANSFER</summary>
    public string BizScene { get; set; } = "DIRECT_TRANSFER";

    /// <summary>转账标题</summary>
    public string? ProductCode { get; set; } = "TRANS_ACCOUNT_NO_PWD";

    /// <summary>备注</summary>
    public string? Remark { get; set; }
}
