using System.Net.Http.Headers;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using GaoXinLibrary.PaySDK.Core;
using GaoXinLibrary.PaySDK.Wechat.Models;

namespace GaoXinLibrary.PaySDK.Wechat.Core;

/// <summary>
/// 微信支付 v3 HTTP 客户端封装
/// <para>自动签名、自动反序列化、自动错误检测</para>
/// </summary>
public sealed class WechatPayHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly WechatPayOptions _options;
    private readonly WechatPaySigner _signer;

    public static readonly JsonSerializerOptions JsonOptions = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    /// <summary>
    /// 初始化 HTTP 客户端
    /// </summary>
    public WechatPayHttpClient(HttpClient httpClient, WechatPayOptions options, WechatPaySigner signer)
    {
        _httpClient = httpClient;
        _options = options;
        _signer = signer;
    }

    /// <summary>
    /// GET 请求
    /// </summary>
    public async Task<T> GetAsync<T>(string path, CancellationToken ct = default) where T : WechatPayBaseResponse
    {
        var uri = new Uri(new Uri(_options.BaseUrl), path);
        var authorization = _signer.BuildAuthorization(_options.MchId, _options.CertSerialNo, "GET", uri.PathAndQuery, string.Empty);

        using var request = new HttpRequestMessage(HttpMethod.Get, uri);
        request.Headers.Authorization = AuthenticationHeaderValue.Parse(authorization);
        request.Headers.Accept.ParseAdd("application/json");
        request.Headers.UserAgent.ParseAdd(PayConstants.UserAgent);
        if (_signer.IsPlatformPublicKeyMode && !string.IsNullOrEmpty(_options.PlatformPublicKeyId))
            request.Headers.TryAddWithoutValidation(WechatPayCallbackHeaders.SerialHeader, _options.PlatformPublicKeyId);

        using var response = await _httpClient.SendAsync(request, ct);
        var json = await response.Content.ReadAsStringAsync(ct);
        VerifyResponseSignature(response, json);
        return DeserializeAndValidate<T>(json, response.StatusCode);
    }

    /// <summary>
    /// POST 请求（JSON body）
    /// </summary>
    public async Task<T> PostAsync<T>(string path, object body, CancellationToken ct = default) where T : WechatPayBaseResponse
    {
        var uri = new Uri(new Uri(_options.BaseUrl), path);
        var bodyJson = JsonSerializer.Serialize(body, JsonOptions);
        var authorization = _signer.BuildAuthorization(_options.MchId, _options.CertSerialNo, "POST", uri.PathAndQuery, bodyJson);

        using var request = new HttpRequestMessage(HttpMethod.Post, uri);
        request.Headers.Authorization = AuthenticationHeaderValue.Parse(authorization);
        request.Headers.Accept.ParseAdd("application/json");
        request.Headers.UserAgent.ParseAdd(PayConstants.UserAgent);
        if (_signer.IsPlatformPublicKeyMode && !string.IsNullOrEmpty(_options.PlatformPublicKeyId))
            request.Headers.TryAddWithoutValidation(WechatPayCallbackHeaders.SerialHeader, _options.PlatformPublicKeyId);
        request.Content = new StringContent(bodyJson, Encoding.UTF8, "application/json");

        using var response = await _httpClient.SendAsync(request, ct);
        var json = await response.Content.ReadAsStringAsync(ct);
        VerifyResponseSignature(response, json);
        return DeserializeAndValidate<T>(json, response.StatusCode);
    }

    /// <summary>
    /// POST 请求（JSON body），无响应包体（204 No Content）
    /// </summary>
    public async Task PostNoContentAsync(string path, object body, CancellationToken ct = default)
    {
        var uri = new Uri(new Uri(_options.BaseUrl), path);
        var bodyJson = JsonSerializer.Serialize(body, JsonOptions);
        var authorization = _signer.BuildAuthorization(_options.MchId, _options.CertSerialNo, "POST", uri.PathAndQuery, bodyJson);

        using var request = new HttpRequestMessage(HttpMethod.Post, uri);
        request.Headers.Authorization = AuthenticationHeaderValue.Parse(authorization);
        request.Headers.Accept.ParseAdd("application/json");
        request.Headers.UserAgent.ParseAdd(PayConstants.UserAgent);
        if (_signer.IsPlatformPublicKeyMode && !string.IsNullOrEmpty(_options.PlatformPublicKeyId))
            request.Headers.TryAddWithoutValidation(WechatPayCallbackHeaders.SerialHeader, _options.PlatformPublicKeyId);
        request.Content = new StringContent(bodyJson, Encoding.UTF8, "application/json");

        using var response = await _httpClient.SendAsync(request, ct);
        if (!response.IsSuccessStatusCode)
        {
            var errorJson = await response.Content.ReadAsStringAsync(ct);
            if (!string.IsNullOrWhiteSpace(errorJson))
            {
                var err = JsonSerializer.Deserialize<WechatPayBaseResponse>(errorJson, JsonOptions);
                if (err != null && !string.IsNullOrEmpty(err.Code))
                    throw new PayException(err.Code, err.Message ?? string.Empty, null);
            }
            throw new PayException("API_ERROR", $"微信支付 API 返回错误，HTTP {(int)response.StatusCode}", null);
        }
    }

    /// <summary>
    /// POST 请求（JSON body），包含敏感信息加密字段时使用
    /// <para>
    /// 与 <see cref="PostAsync{T}"/> 的区别：无论公钥模式还是平台证书模式，
    /// 均强制携带 <c>Wechatpay-Serial</c> 请求头，值为 <see cref="WechatPaySigner.PlatformSerialNo"/>。
    /// </para>
    /// <para>
    /// • 微信支付公钥模式：<c>Wechatpay-Serial</c> = <c>PUB_KEY_ID_xxx</c><br/>
    /// • 平台证书模式：<c>Wechatpay-Serial</c> = 平台证书序列号
    /// </para>
    /// </summary>
    public async Task<T> PostWithEncryptionAsync<T>(string path, object body, CancellationToken ct = default) where T : WechatPayBaseResponse
    {
        var serialNo = _signer.PlatformSerialNo
            ?? throw new PayException("SERVICE_NOT_CONFIGURED", "未配置微信支付公钥或平台证书，无法发送包含加密敏感字段的请求", null);

        var uri = new Uri(new Uri(_options.BaseUrl), path);
        var bodyJson = JsonSerializer.Serialize(body, JsonOptions);
        var authorization = _signer.BuildAuthorization(_options.MchId, _options.CertSerialNo, "POST", uri.PathAndQuery, bodyJson);

        using var request = new HttpRequestMessage(HttpMethod.Post, uri);
        request.Headers.Authorization = AuthenticationHeaderValue.Parse(authorization);
        request.Headers.Accept.ParseAdd("application/json");
        request.Headers.UserAgent.ParseAdd(PayConstants.UserAgent);
        request.Headers.TryAddWithoutValidation(WechatPayCallbackHeaders.SerialHeader, serialNo);
        request.Content = new StringContent(bodyJson, Encoding.UTF8, "application/json");

        using var response = await _httpClient.SendAsync(request, ct);
        var json = await response.Content.ReadAsStringAsync(ct);
        VerifyResponseSignature(response, json);
        return DeserializeAndValidate<T>(json, response.StatusCode);
    }

    /// <summary>
    /// GET 请求，返回原始字节（账单下载使用）
    /// </summary>
    public async Task<byte[]> GetBytesAsync(string path, CancellationToken ct = default)
    {
        var uri = new Uri(new Uri(_options.BaseUrl), path);
        var authorization = _signer.BuildAuthorization(_options.MchId, _options.CertSerialNo, "GET", uri.PathAndQuery, string.Empty);

        using var request = new HttpRequestMessage(HttpMethod.Get, uri);
        request.Headers.Authorization = AuthenticationHeaderValue.Parse(authorization);
        request.Headers.UserAgent.ParseAdd(PayConstants.UserAgent);
        if (_signer.IsPlatformPublicKeyMode && !string.IsNullOrEmpty(_options.PlatformPublicKeyId))
            request.Headers.TryAddWithoutValidation(WechatPayCallbackHeaders.SerialHeader, _options.PlatformPublicKeyId);

        using var response = await _httpClient.SendAsync(request, ct);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsByteArrayAsync(ct);
    }

    /// <summary>
    /// 验证微信支付 API 响应签名
    /// </summary>
    private void VerifyResponseSignature(HttpResponseMessage response, string body)
    {
        // 错误响应（如 4xx/5xx）不携带签名头，跳过验签交由业务层处理
        if (!response.IsSuccessStatusCode)
            return;

        var responseHeaders = WechatPayCallbackHeaders.FromHeaders(
            response.Headers.ToDictionary(h => h.Key, h => h.Value.FirstOrDefault() ?? string.Empty));

        if (string.IsNullOrEmpty(responseHeaders.Timestamp) || string.IsNullOrEmpty(responseHeaders.Nonce) || string.IsNullOrEmpty(responseHeaders.Signature))
            throw new PayException("VERIFY_FAILED", "微信支付 API 响应缺少签名头（Wechatpay-Timestamp/Nonce/Signature）", null);

        var valid = _signer.VerifySignature(body, responseHeaders);
        if (!valid)
            throw new PayException("VERIFY_FAILED", "微信支付 API 响应签名验证失败", null);
    }

    private static T DeserializeAndValidate<T>(string json, System.Net.HttpStatusCode statusCode) where T : WechatPayBaseResponse
    {
        if (string.IsNullOrWhiteSpace(json))
            throw new PayException("EMPTY_RESPONSE", $"微信支付 API 返回空响应，HTTP {(int)statusCode}", null);

        T result;
        try
        {
            result = JsonSerializer.Deserialize<T>(json, JsonOptions)
                     ?? throw new PayException("DESERIALIZE_FAILED", "反序列化微信支付响应失败", null);
        }
        catch (JsonException ex)
        {
            throw new PayException($"反序列化失败：{ex.Message}，原始响应：{json}", ex);
        }

        if (!string.IsNullOrEmpty(result.Code))
        {
            throw new PayException(result.Code, result.Message ?? string.Empty, null);
        }

        return result;
    }
}
