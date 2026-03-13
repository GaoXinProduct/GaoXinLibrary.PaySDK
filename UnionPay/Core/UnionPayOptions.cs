using System.ComponentModel.DataAnnotations;
using GaoXinLibrary.PaySDK.Core;

namespace GaoXinLibrary.PaySDK.UnionPay.Core;

/// <summary>
/// 银联支付配置选项
/// <para>https://open.unionpay.com/tjweb/support/doc/online/3/125</para>
/// </summary>
public class UnionPayOptions
{
    /// <summary>商户号（merId）</summary>
    [Required(ErrorMessage = "银联 MerId 不能为空")]
    public string MerId { get; set; } = string.Empty;

    /// <summary>
    /// 商户 RSA 私钥（PEM 格式或 Base64 裸字符串）
    /// <para>用于请求签名（SHA256withRSA）</para>
    /// </summary>
    [Required(ErrorMessage = "银联 PrivateKey 不能为空")]
    public string PrivateKey { get; set; } = string.Empty;

    /// <summary>
    /// 商户证书序列号（certId）
    /// <para>即私钥对应的公钥证书的序列号，放入请求参数</para>
    /// </summary>
    [Required(ErrorMessage = "银联 CertId 不能为空")]
    public string CertId { get; set; } = string.Empty;

    /// <summary>
    /// 银联根证书公钥（PEM）
    /// <para>用于验证银联回调签名</para>
    /// </summary>
    [Required(ErrorMessage = "银联 UnionPayPublicKey 不能为空")]
    public string UnionPayPublicKey { get; set; } = string.Empty;

    /// <summary>
    /// 前台通知地址（frontUrl）
    /// <para>支付完成后浏览器同步跳转</para>
    /// </summary>
    public string FrontUrl { get; set; } = string.Empty;

    /// <summary>
    /// 后台通知地址（backUrl）
    /// <para>银联服务器异步通知</para>
    /// </summary>
    public string BackUrl { get; set; } = string.Empty;

    /// <summary>前台请求网关地址</summary>
    public string FrontGatewayUrl { get; set; } = "https://gateway.95516.com/gateway/api/frontTransReq.do";

    /// <summary>后台请求网关地址</summary>
    public string BackGatewayUrl { get; set; } = "https://gateway.95516.com/gateway/api/backTransReq.do";

    /// <summary>订单查询网关地址</summary>
    public string QueryGatewayUrl { get; set; } = "https://gateway.95516.com/gateway/api/queryTrans.do";

    /// <summary>App 请求网关地址（WAP/APP 前台交易）</summary>
    public string AppGatewayUrl { get; set; } = "https://gateway.95516.com/gateway/api/appTransReq.do";

    /// <summary>文件传输网关地址（对账文件下载）</summary>
    public string FileGatewayUrl { get; set; } = "https://filedownload.95516.com/";

    /// <summary>HTTP 请求超时时间，默认 30 秒</summary>
    public TimeSpan HttpTimeout { get; set; } = TimeSpan.FromSeconds(30);

    /// <summary>版本号，默认 5.1.0</summary>
    public string Version { get; set; } = "5.1.0";

    /// <summary>编码，默认 UTF-8</summary>
    public string Encoding { get; set; } = "UTF-8";

    /// <summary>签名方式，01 = RSA，11 = SM2，默认 01</summary>
    public string SignMethod { get; set; } = "01";

    /// <summary>
    /// 瞫态故障重试配置（网络抖动、超时、5xx）
    /// </summary>
    public PayRetryOptions RetryOptions { get; set; } = new();
}
