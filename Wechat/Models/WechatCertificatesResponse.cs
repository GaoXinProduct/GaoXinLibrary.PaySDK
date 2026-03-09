using System.Text.Json.Serialization;
using GaoXinLibrary.PaySDK.Wechat.Core;

namespace GaoXinLibrary.PaySDK.Wechat.Models;

/// <summary>
/// 微信支付平台证书下载接口响应（GET /v3/certificates）
/// </summary>
public class WechatCertificatesResponse : WechatPayBaseResponse
{
    /// <summary>证书列表</summary>
    [JsonPropertyName("data")]
    public List<WechatCertificateItem> Data { get; set; } = [];
}
