namespace GaoXinLibrary.PaySDK.Wechat.Models;

/// <summary>
/// 微信支付回调通知 HTTP 请求头签名信息
/// <para>
/// 封装验签所需的四个请求头字段，适用于支付回调和退款回调。
/// </para>
/// </summary>
public class WechatPayCallbackHeaders
{
    /// <summary>HTTP 请求头名称：<c>Wechatpay-Timestamp</c></summary>
    public const string TimestampHeader = "Wechatpay-Timestamp";

    /// <summary>HTTP 请求头名称：<c>Wechatpay-Nonce</c></summary>
    public const string NonceHeader = "Wechatpay-Nonce";

    /// <summary>HTTP 请求头名称：<c>Wechatpay-Signature</c></summary>
    public const string SignatureHeader = "Wechatpay-Signature";

    /// <summary>HTTP 请求头名称：<c>Wechatpay-Serial</c></summary>
    public const string SerialHeader = "Wechatpay-Serial";

    /// <summary>
    /// 回调请求头 <c>Wechatpay-Timestamp</c>
    /// </summary>
    public string Timestamp { get; set; } = string.Empty;

    /// <summary>
    /// 回调请求头 <c>Wechatpay-Nonce</c>
    /// </summary>
    public string Nonce { get; set; } = string.Empty;

    /// <summary>
    /// 回调请求头 <c>Wechatpay-Signature</c>
    /// </summary>
    public string Signature { get; set; } = string.Empty;

    /// <summary>
    /// 回调请求头 <c>Wechatpay-Serial</c>（平台证书序列号或微信支付公钥 ID）
    /// </summary>
    public string? Serial { get; set; }

    /// <summary>
    /// 从 HTTP 请求头字典中解析微信支付回调签名信息
    /// </summary>
    /// <param name="headers">HTTP 请求头字典（键不区分大小写）</param>
    public static WechatPayCallbackHeaders FromHeaders(IDictionary<string, string> headers)
    {
        headers.TryGetValue(TimestampHeader, out var ts);
        headers.TryGetValue(NonceHeader,     out var nonce);
        headers.TryGetValue(SignatureHeader, out var sig);
        headers.TryGetValue(SerialHeader,    out var serial);
        return new WechatPayCallbackHeaders
        {
            Timestamp = ts        ?? string.Empty,
            Nonce     = nonce     ?? string.Empty,
            Signature = sig       ?? string.Empty,
            Serial    = serial
        };
    }
}
