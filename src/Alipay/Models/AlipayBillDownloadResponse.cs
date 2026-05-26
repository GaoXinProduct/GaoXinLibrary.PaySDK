using System.Text.Json.Serialization;
using GaoXinLibrary.PaySDK.Alipay.Core;

namespace GaoXinLibrary.PaySDK.Alipay.Models;

/// <summary>
/// 支付宝下载账单响应
/// </summary>
public class AlipayBillDownloadResponse : AlipayBaseResponse
{
    /// <summary>账单下载地址链接，有效时间为 30s</summary>
    [JsonPropertyName("bill_download_url")]
    public string? BillDownloadUrl { get; set; }
}
