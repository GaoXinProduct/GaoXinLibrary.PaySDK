using System.Text.Json.Serialization;
using GaoXinLibrary.PaySDK.Wechat.Core;

namespace GaoXinLibrary.PaySDK.Wechat.Models;

/// <summary>
/// 商家转账到零钱响应
/// <para>https://pay.weixin.qq.com/doc/v3/merchant/4012716435</para>
/// </summary>
public class WechatTransferBillsResponse : WechatPayBaseResponse
{
    /// <summary>商户转账单号</summary>
    [JsonPropertyName("out_bill_no")]
    public string OutBillNo { get; set; } = string.Empty;

    /// <summary>微信转账单号</summary>
    [JsonPropertyName("transfer_bill_no")]
    public string TransferBillNo { get; set; } = string.Empty;

    /// <summary>创建时间（rfc3339）</summary>
    [JsonPropertyName("create_time")]
    public string CreateTime { get; set; } = string.Empty;

    /// <summary>转账状态：ACCEPTED / PROCESSING / WAIT_USER_CONFIRM / TRANSFERING / SUCCESS / FAIL / CANCELING / CANCELLED</summary>
    [JsonPropertyName("state")]
    public string State { get; set; } = string.Empty;

    /// <summary>跳转领取页面的 package 信息（仅在 WAIT_USER_CONFIRM 状态返回）</summary>
    [JsonPropertyName("package_info")]
    public string? PackageInfo { get; set; }
}
