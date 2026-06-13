namespace GaoXinLibrary.PaySDK.UnionPay.OpenApi;

/// <summary>
/// 银联 OpenAPI 认证模式
/// </summary>
public enum UnionPayOpenApiAuthMode : byte
{
    /// <summary>OAuth2 模式（X-OPEN-TOKEN / X-OPEN-SIGN / X-OPEN-TS）</summary>
    OAuth2 = 0,

    /// <summary>非对称验签模式（RSA2/SM2）</summary>
    Asymmetric = 1
}
