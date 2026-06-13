using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace GaoXinLibrary.PaySDK.UnionPay.OpenApi;

/// <summary>
/// 银联 OpenAPI 通用服务
/// <para>用于承载 OpenAPI（OAuth2/非对称）能力，避免与现有收单交易接口混用。</para>
/// </summary>
public sealed class UnionPayOpenApiService : IUnionPayOpenApiService
{
    private readonly HttpClient _httpClient;
    private readonly UnionPayOpenApiOptions _options;
    private readonly RSA? _privateRsa;

    /// <summary>
    /// 初始化银联 OpenAPI 服务
    /// </summary>
    /// <param name="httpClient">HTTP 客户端实例</param>
    /// <param name="options">OpenAPI 配置选项</param>
    public UnionPayOpenApiService(HttpClient httpClient, UnionPayOpenApiOptions options)
    {
        _httpClient = httpClient;
        _options = options;
        _privateRsa = BuildPrivateRsaIfNeeded(options);
    }

    /// <summary>
    /// 向银联 OpenAPI 发送 POST 请求
    /// <para>根据 <see cref="UnionPayOpenApiOptions.AuthMode"/> 自动附加 OAuth2 X-OPEN 头或非对称签名头。</para>
    /// </summary>
    /// <param name="bizMethod">业务方法名</param>
    /// <param name="payload">请求体（将序列化为 JSON）</param>
    /// <param name="ct">取消令牌</param>
    /// <returns>API 响应原始字符串</returns>
    public async Task<string> PostAsync(string bizMethod, object payload, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(bizMethod);
        var reqId = Guid.NewGuid().ToString("N");
        var body = JsonSerializer.Serialize(payload);
        var uri = new Uri(new Uri(_options.BaseUrl), "/openapi/gateway");

        using var request = new HttpRequestMessage(HttpMethod.Post, uri)
        {
            Content = new StringContent(body, Encoding.UTF8, "application/json")
        };

        request.Headers.TryAddWithoutValidation("x-up-version", "1.0");
        request.Headers.TryAddWithoutValidation("x-up-appid", _options.AppId);
        request.Headers.TryAddWithoutValidation("x-up-biz-method", bizMethod);
        request.Headers.TryAddWithoutValidation("x-up-req-id", reqId);

        if (_options.AuthMode == UnionPayOpenApiAuthMode.OAuth2)
        {
            if (string.IsNullOrWhiteSpace(_options.OAuthToken))
                throw new InvalidOperationException("银联 OpenAPI OAuth2 模式下必须配置 OAuthToken");
            if (string.IsNullOrWhiteSpace(_options.OAuthSignatureKey))
                throw new InvalidOperationException("银联 OpenAPI OAuth2 模式下必须配置 OAuthSignatureKey");

            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
            var signature = BuildOAuthSignature(body, timestamp, _options.OAuthSignatureKey);
            request.Headers.TryAddWithoutValidation("X-OPEN-TOKEN", _options.OAuthToken);
            request.Headers.TryAddWithoutValidation("X-OPEN-SIGN", signature);
            request.Headers.TryAddWithoutValidation("X-OPEN-TS", timestamp);
        }
        else
        {
            var signingText = $"version=1.0&appId={_options.AppId}&bizMethod={bizMethod}&reqId={reqId}&body={body}";
            var signature = Sign(signingText);
            request.Headers.TryAddWithoutValidation("x-up-sign", signature);
            request.Headers.TryAddWithoutValidation("x-up-sign-type", "RSA2");
        }

        using var response = await _httpClient.SendAsync(request, ct);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync(ct);
    }

    private static RSA? BuildPrivateRsaIfNeeded(UnionPayOpenApiOptions options)
    {
        if (options.AuthMode != UnionPayOpenApiAuthMode.Asymmetric)
            return null;
        if (string.IsNullOrWhiteSpace(options.PrivateKey))
            throw new InvalidOperationException("银联 OpenAPI 非对称模式下必须配置 PrivateKey");

        var key = options.PrivateKey.Trim();
        var rsa = RSA.Create();
        if (key.StartsWith("-----", StringComparison.Ordinal))
            rsa.ImportFromPem(key);
        else
            rsa.ImportPkcs8PrivateKey(Convert.FromBase64String(key), out _);
        return rsa;
    }

    private string Sign(string content)
    {
        if (_privateRsa is null)
            throw new InvalidOperationException("银联 OpenAPI 签名器未初始化");
        var bytes = Encoding.UTF8.GetBytes(content);
        var signed = _privateRsa.SignData(bytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        return Convert.ToBase64String(signed);
    }

    private static string BuildOAuthSignature(string body, string timestamp, string signatureKey)
    {
        var bytes = Encoding.UTF8.GetBytes(signatureKey + body + timestamp);
        return Convert.ToHexString(SHA256.HashData(bytes)).ToLowerInvariant();
    }
}
