using System.ComponentModel.DataAnnotations;

namespace GaoXinLibrary.PaySDK.UnionPay.OpenApi;

/// <summary>
/// 银联 OpenAPI 模块配置
/// </summary>
public sealed class UnionPayOpenApiOptions
{
    /// <summary>OpenAPI 基础地址</summary>
    [Required(ErrorMessage = "银联 OpenAPI BaseUrl 不能为空")]
    public string BaseUrl { get; set; } = "https://openapi.unionpay.com";

    /// <summary>应用标识</summary>
    [Required(ErrorMessage = "银联 OpenAPI AppId 不能为空")]
    public string AppId { get; set; } = string.Empty;

    /// <summary>认证模式</summary>
    public UnionPayOpenApiAuthMode AuthMode { get; set; } = UnionPayOpenApiAuthMode.Asymmetric;

    /// <summary>OAuth2 Token（OAuth2 模式必填）</summary>
    public string? OAuthToken { get; set; }

    /// <summary>商户签名私钥（非对称模式必填，PEM/Base64）</summary>
    public string? PrivateKey { get; set; }

    /// <summary>HTTP 超时时间</summary>
    public TimeSpan HttpTimeout { get; set; } = TimeSpan.FromSeconds(30);
}
