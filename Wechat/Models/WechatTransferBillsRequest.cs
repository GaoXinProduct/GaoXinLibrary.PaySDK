using System.Text.Json.Serialization;

namespace GaoXinLibrary.PaySDK.Wechat.Models;

/// <summary>
/// 商家转账到零钱请求
/// <para>https://pay.weixin.qq.com/doc/v3/merchant/4012716434</para>
/// </summary>
public class WechatTransferBillsRequest
{
    /// <summary>商户 appid</summary>
    [JsonPropertyName("appid")]
    public string AppId { get; set; } = string.Empty;

    /// <summary>商户转账单号</summary>
    [JsonPropertyName("out_bill_no")]
    public string OutBillNo { get; set; } = string.Empty;

    /// <summary>转账场景 ID</summary>
    [JsonPropertyName("transfer_scene_id")]
    public string TransferSceneId { get; set; } = string.Empty;

    /// <summary>收款用户 openid</summary>
    [JsonPropertyName("openid")]
    public string OpenId { get; set; } = string.Empty;

    /// <summary>收款用户姓名（可选，加密传输）</summary>
    [JsonPropertyName("user_name")]
    public string? UserName { get; set; }

    /// <summary>转账金额，单位分</summary>
    [JsonPropertyName("transfer_amount")]
    public int TransferAmount { get; set; }

    /// <summary>转账备注</summary>
    [JsonPropertyName("transfer_remark")]
    public string TransferRemark { get; set; } = string.Empty;

    /// <summary>异步通知地址</summary>
    [JsonPropertyName("notify_url")]
    public string? NotifyUrl { get; set; }
}
