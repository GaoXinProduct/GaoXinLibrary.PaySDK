namespace GaoXinLibrary.PaySDK.Wechat.Core;

/// <summary>
/// 微信支付配置选项
/// </summary>
public class WechatPayOptions
{
    /// <summary>应用 ID（公众号/小程序/APP AppID）</summary>
    public string AppId { get; set; } = string.Empty;

    /// <summary>商户号</summary>
    public string MchId { get; set; } = string.Empty;

    /// <summary>
    /// API v3 密钥（32 字节）
    /// <para>用于回调通知解密（AEAD_AES_256_GCM）</para>
    /// </summary>
    public string ApiV3Key { get; set; } = string.Empty;

    /// <summary>
    /// 商户私钥（PEM 格式，去掉 header/footer 的 Base64）
    /// <para>用于请求签名（SHA256withRSA）</para>
    /// </summary>
    public string PrivateKey { get; set; } = string.Empty;

    /// <summary>商户证书序列号</summary>
    public string CertSerialNo { get; set; } = string.Empty;

    /// <summary>
    /// 支付结果异步通知回调地址（notify_url）
    /// <para>配置后所有下单请求自动携带，无需在每次下单时手动传入</para>
    /// </summary>
    public string NotifyUrl { get; set; } = string.Empty;

    /// <summary>
    /// 退款结果异步通知回调地址（notify_url）
    /// <para>配置后退款请求自动携带，无需在每次退款时手动传入；也可在退款请求中覆盖</para>
    /// </summary>
    public string RefundNotifyUrl { get; set; } = string.Empty;

    /// <summary>
    /// H5 支付完成后跳转的页面 URL（redirect_url）
    /// <para>拼接在 h5_url 后面，支付完成后返回指定页面。域名必须与微信商户平台「H5 支付」中配置的域名一致。</para>
    /// <para>配置后 H5 下单自动拼接，也可在 <see cref="WechatH5OrderRequest.RedirectUrl"/> 中按请求覆盖。</para>
    /// </summary>
    public string H5RedirectUrl { get; set; } = string.Empty;

    /// <summary>API 基础地址，默认 https://api.mch.weixin.qq.com</summary>
    public string BaseUrl { get; set; } = "https://api.mch.weixin.qq.com";

    /// <summary>HTTP 请求超时时间，默认 30 秒</summary>
    public TimeSpan HttpTimeout { get; set; } = TimeSpan.FromSeconds(30);

    /// <summary>
    /// 微信支付平台证书公钥（PEM 格式，带 header/footer 或仅 Base64 内容均可）
    /// <para>用于验证微信支付回调通知的签名（SHA256withRSA），即 <c>Wechatpay-Signature</c> 请求头。</para>
    /// <para>可从微信支付商户平台下载平台证书，或通过 GET /v3/certificates 接口获取。</para>
    /// <para>
    /// 新版微信支付公钥模式下，此处填写微信支付公钥内容；
    /// 旧版平台证书模式下，通过 <c>DownloadCertificatesAsync()</c> 自动管理，此处可留空。
    /// </para>
    /// </summary>
    public string PlatformPublicKey { get; set; } = string.Empty;

    /// <summary>
    /// 微信支付公钥 ID（新版公钥模式专用，格式为 <c>PUB_KEY_ID_xxxx</c>）
    /// <para>
    /// 在商户平台启用「微信支付公钥」后，回调 <c>Wechatpay-Serial</c> 的值即为此 ID。
    /// 配置后，验签时序列号与此 ID 完全匹配则使用 <see cref="PlatformPublicKey"/>；
    /// 不匹配则从证书缓存中查找（旧版平台证书模式）。
    /// </para>
    /// <para>若使用旧版平台证书模式，保持默认空值即可。</para>
    /// </summary>
    public string PlatformPublicKeyId { get; set; } = string.Empty;
}
