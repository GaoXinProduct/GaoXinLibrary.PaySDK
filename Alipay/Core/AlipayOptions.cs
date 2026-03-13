using System.ComponentModel.DataAnnotations;
using GaoXinLibrary.PaySDK.Core;

namespace GaoXinLibrary.PaySDK.Alipay.Core;

/// <summary>
/// 支付宝配置选项
/// </summary>
public class AlipayOptions
{
    /// <summary>支付宝分配给开发者的应用 ID</summary>
    [Required(ErrorMessage = "支付宝 AppId 不能为空")]
    public string AppId { get; set; } = string.Empty;

    /// <summary>
    /// 商户 RSA 私钥（PEM 格式或 Base64 裸字符串）
    /// <para>使用 RSA2（SHA256withRSA）签名</para>
    /// </summary>
    [Required(ErrorMessage = "支付宝 PrivateKey 不能为空")]
    public string PrivateKey { get; set; } = string.Empty;

    /// <summary>
    /// 支付宝 RSA2 公钥（PEM 格式或 Base64 裸字符串）
    /// <para>用于验证支付宝回调签名</para>
    /// </summary>
    [Required(ErrorMessage = "支付宝 AlipayPublicKey 不能为空")]
    public string AlipayPublicKey { get; set; } = string.Empty;

    /// <summary>签名类型，默认 RSA2</summary>
    public string SignType { get; set; } = "RSA2";

    /// <summary>字符集，默认 utf-8</summary>
    public string Charset { get; set; } = "utf-8";

    /// <summary>数据格式，默认 JSON</summary>
    public string Format { get; set; } = "JSON";

    /// <summary>API 版本，默认 1.0</summary>
    public string Version { get; set; } = "1.0";

    /// <summary>
    /// 支付宝异步通知回调地址（notify_url）
    /// <para>配置后所有支付请求自动携带，无需在每次下单时手动传入</para>
    /// </summary>
    public string NotifyUrl { get; set; } = string.Empty;

    /// <summary>
    /// 支付宝同步跳转地址（return_url）
    /// <para>配置后手机网站支付、电脑网站支付等自动携带，无需在每次下单时手动传入</para>
    /// </summary>
    public string ReturnUrl { get; set; } = string.Empty;

    /// <summary>支付宝网关地址，默认正式环境</summary>
    public string GatewayUrl { get; set; } = "https://openapi.alipay.com/gateway.do";

    /// <summary>HTTP 请求超时时间，默认 30 秒</summary>
    public TimeSpan HttpTimeout { get; set; } = TimeSpan.FromSeconds(30);

    /// <summary>
    /// 瞬态故障重试配置（网络抖动、超时、5xx）
    /// </summary>
    public PayRetryOptions RetryOptions { get; set; } = new();
}
