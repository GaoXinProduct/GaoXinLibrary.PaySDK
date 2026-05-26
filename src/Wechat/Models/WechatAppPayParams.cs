using System.Text.Json.Serialization;

namespace GaoXinLibrary.PaySDK.Wechat.Models;

/// <summary>
/// 微信 APP 支付调起参数
/// <para>由服务端生成，返回给 APP 客户端用于调起微信支付</para>
/// </summary>
public class WechatAppPayParams
{
    /// <summary>应用 ID</summary>
    [JsonPropertyName("appid")]
    public string AppId { get; set; } = string.Empty;

    /// <summary>商户号</summary>
    [JsonPropertyName("partnerid")]
    public string PartnerId { get; set; } = string.Empty;

    /// <summary>预支付交易会话 ID</summary>
    [JsonPropertyName("prepayid")]
    public string PrepayId { get; set; } = string.Empty;

    /// <summary>扩展字段，固定为 Sign=WXPay</summary>
    [JsonPropertyName("package")]
    public string Package { get; set; } = "Sign=WXPay";

    /// <summary>随机字符串</summary>
    [JsonPropertyName("noncestr")]
    public string NonceStr { get; set; } = string.Empty;

    /// <summary>时间戳（秒）</summary>
    [JsonPropertyName("timestamp")]
    public string Timestamp { get; set; } = string.Empty;

    /// <summary>签名</summary>
    [JsonPropertyName("sign")]
    public string Sign { get; set; } = string.Empty;
}
