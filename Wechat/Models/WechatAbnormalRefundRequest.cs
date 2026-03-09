using System.Text.Json.Serialization;

namespace GaoXinLibrary.PaySDK.Wechat.Models;

/// <summary>
/// 微信支付发起异常退款请求
/// <para>POST /v3/refund/domestic/refunds/{refund_id}/apply-abnormal-refund</para>
/// <para>https://pay.weixin.qq.com/doc/v3/merchant/4013071193</para>
/// </summary>
public class WechatAbnormalRefundRequest
{
    /// <summary>
    /// 【微信支付退款单号】申请退款受理成功时，该笔退款单在微信支付侧生成的唯一标识（路径参数，不参与 JSON 序列化）
    /// </summary>
    [JsonIgnore]
    public string RefundId { get; set; } = string.Empty;

    /// <summary>
    /// 【商户退款单号】商户申请退款时传入的商户系统内部退款单号（必填）
    /// </summary>
    [JsonPropertyName("out_refund_no")]
    public string OutRefundNo { get; set; } = string.Empty;

    /// <summary>
    /// 【异常退款处理方式】USER_BANK_CARD: 退款到用户银行卡；MERCHANT_BANK_CARD: 退款至交易商户银行账户（必填）
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// 【开户银行】银行类型，采用字符串类型的银行标识。若退款至用户此字段必填。
    /// <para>仅支持招行、交通银行、农行、建行、工商、中行、平安、浦发、中信、光大、民生、兴业、广发、邮储、宁波银行的借记卡。</para>
    /// </summary>
    [JsonPropertyName("bank_type")]
    public string? BankType { get; set; }

    /// <summary>
    /// 【收款银行卡号】用户的银行卡账号，传入明文即可，SDK 会自动使用微信支付公钥进行 RSAES-OAEP 加密。若退款至用户此字段必填。
    /// </summary>
    [JsonPropertyName("bank_account")]
    public string? BankAccount { get; set; }

    /// <summary>
    /// 【收款用户姓名】收款用户姓名，传入明文即可，SDK 会自动使用微信支付公钥进行 RSAES-OAEP 加密。若退款至用户此字段必填。
    /// </summary>
    [JsonPropertyName("real_name")]
    public string? RealName { get; set; }
}
