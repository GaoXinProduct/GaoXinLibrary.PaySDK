using System.Text.Json.Serialization;
using GaoXinLibrary.PaySDK.Wechat.Core;

namespace GaoXinLibrary.PaySDK.Wechat.Models;

/// <summary>
/// 查询商家转账到零钱响应
/// <para>https://pay.weixin.qq.com/doc/v3/merchant/4012716436</para>
/// </summary>
public class WechatTransferBillQueryResponse : WechatPayBaseResponse
{
    /// <summary>商户转账单号</summary>
    [JsonPropertyName("out_bill_no")]
    public string OutBillNo { get; set; } = string.Empty;

    /// <summary>微信转账单号</summary>
    [JsonPropertyName("transfer_bill_no")]
    public string TransferBillNo { get; set; } = string.Empty;

    /// <summary>商户 appid</summary>
    [JsonPropertyName("appid")]
    public string AppId { get; set; } = string.Empty;

    /// <summary>收款用户 openid</summary>
    [JsonPropertyName("openid")]
    public string OpenId { get; set; } = string.Empty;

    /// <summary>创建时间（rfc3339）</summary>
    [JsonPropertyName("create_time")]
    public string CreateTime { get; set; } = string.Empty;

    /// <summary>转账状态：ACCEPTED / PROCESSING / WAIT_USER_CONFIRM / TRANSFERING / SUCCESS / FAIL / CANCELING / CANCELLED</summary>
    [JsonPropertyName("state")]
    public string State { get; set; } = string.Empty;

    /// <summary>转账金额，单位分</summary>
    [JsonPropertyName("transfer_amount")]
    public int TransferAmount { get; set; }

    /// <summary>转账备注</summary>
    [JsonPropertyName("transfer_remark")]
    public string TransferRemark { get; set; } = string.Empty;

    /// <summary>失败原因</summary>
    [JsonPropertyName("fail_reason")]
    public string? FailReason { get; set; }

    /// <summary>跳转领取页面的 package 信息</summary>
    [JsonPropertyName("package_info")]
    public string? PackageInfo { get; set; }

    /// <summary>收款用户姓名（加密）</summary>
    [JsonPropertyName("user_name")]
    public string? UserName { get; set; }
}
