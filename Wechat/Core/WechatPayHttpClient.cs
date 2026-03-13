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
    /// GET 请求（含瞫态故障自动重试）
    /// </summary>
    public async Task<T> GetAsync<T>(string path, CancellationToken ct = default) where T : WechatPayBaseResponse
    {
        return await ExecuteWithRetryAsync(async () =>
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
        }, ct);
    }

    /// <summary>
    /// POST 请求（JSON body）
    /// </summary>
    /// <param name="path">API 路径</param>
    /// <param name="body">请求体对象</param>
    /// <param name="idempotencyKey">幂等键（微信支付 v3 <c>Idempotency-Key</c> 请求头），传入后可防止因网络重试导致的重复扣款</param>
    /// <param name="ct">取消令牌</param>
    public async Task<T> PostAsync<T>(string path, object body, string? idempotencyKey = null, CancellationToken ct = default) where T : WechatPayBaseResponse
    {
        return await ExecuteWithRetryAsync(async () =>
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
            if (!string.IsNullOrEmpty(idempotencyKey))
                request.Headers.TryAddWithoutValidation("Idempotency-Key", idempotencyKey);
            request.Content = new StringContent(bodyJson, Encoding.UTF8, "application/json");

            using var response = await _httpClient.SendAsync(request, ct);
            var json = await response.Content.ReadAsStringAsync(ct);
            VerifyResponseSignature(response, json);
            return DeserializeAndValidate<T>(json, response.StatusCode);
        }, ct);
    }

    /// <summary>
    /// POST 请求（JSON body），无响应包体（204 No Content）
    /// </summary>
    /// <param name="path">API 路径</param>
    /// <param name="body">请求体对象</param>
    /// <param name="idempotencyKey">幂等键（微信支付 v3 <c>Idempotency-Key</c> 请求头），传入后可防止因网络重试导致的重复操作</param>
    /// <param name="ct">取消令牌</param>
    public async Task PostNoContentAsync(string path, object body, string? idempotencyKey = null, CancellationToken ct = default)
    {
        await ExecuteWithRetryAsync(async () =>
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
            if (!string.IsNullOrEmpty(idempotencyKey))
                request.Headers.TryAddWithoutValidation("Idempotency-Key", idempotencyKey);
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
            return true;
        }, ct);
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
    /// <param name="path">API 路径</param>
    /// <param name="body">请求体对象</param>
    /// <param name="idempotencyKey">幂等键（微信支付 v3 <c>Idempotency-Key</c> 请求头），传入后可防止因网络重试导致的重复操作</param>
    /// <param name="ct">取消令牌</param>
    public async Task<T> PostWithEncryptionAsync<T>(string path, object body, string? idempotencyKey = null, CancellationToken ct = default) where T : WechatPayBaseResponse
    {
        var serialNo = _signer.PlatformSerialNo
            ?? throw new PayException("SERVICE_NOT_CONFIGURED", "未配置微信支付公钥或平台证书，无法发送包含加密敏感字段的请求", null);

        return await ExecuteWithRetryAsync(async () =>
        {
            var uri = new Uri(new Uri(_options.BaseUrl), path);
            var bodyJson = JsonSerializer.Serialize(body, JsonOptions);
            var authorization = _signer.BuildAuthorization(_options.MchId, _options.CertSerialNo, "POST", uri.PathAndQuery, bodyJson);

            using var request = new HttpRequestMessage(HttpMethod.Post, uri);
            request.Headers.Authorization = AuthenticationHeaderValue.Parse(authorization);
            request.Headers.Accept.ParseAdd("application/json");
            request.Headers.UserAgent.ParseAdd(PayConstants.UserAgent);
            request.Headers.TryAddWithoutValidation(WechatPayCallbackHeaders.SerialHeader, serialNo);
            if (!string.IsNullOrEmpty(idempotencyKey))
                request.Headers.TryAddWithoutValidation("Idempotency-Key", idempotencyKey);
            request.Content = new StringContent(bodyJson, Encoding.UTF8, "application/json");

            using var response = await _httpClient.SendAsync(request, ct);
            var json = await response.Content.ReadAsStringAsync(ct);
            VerifyResponseSignature(response, json);
            return DeserializeAndValidate<T>(json, response.StatusCode);
        }, ct);
    }

    /// <summary>
    /// GET 请求，返回原始字节（账单下载使用，含瞫态故障自动重试）
    /// </summary>
    public async Task<byte[]> GetBytesAsync(string path, CancellationToken ct = default)
    {
        return await ExecuteWithRetryAsync(async () =>
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
        }, ct);
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

    // ─── 瞫态故障自动重试 ─────────────────────────────────────────────

    /// <summary>
    /// 执行请求，网络抖动、连接超时、5xx 等瞫态故障时按指数退避自动重试
    /// </summary>
    private async Task<T> ExecuteWithRetryAsync<T>(Func<Task<T>> action, CancellationToken ct)
    {
        var retryOptions = _options.RetryOptions;
        if (retryOptions is not { MaxRetries: > 0 })
            return await action();

        var maxRetries = retryOptions.MaxRetries;
        var delay = retryOptions.InitialDelay;
        var maxDelay = retryOptions.MaxDelay;

        for (var attempt = 0; ; attempt++)
        {
            try
            {
                return await action();
            }
            catch (Exception ex) when (attempt < maxRetries && IsTransientException(ex, ct))
            {
                await Task.Delay(delay, ct);
                delay = TimeSpan.FromTicks(Math.Min(delay.Ticks * 2, maxDelay.Ticks));
            }
        }
    }

    /// <summary>
    /// 判断异常是否为可重试的瞫态故障
    /// </summary>
    private static bool IsTransientException(Exception ex, CancellationToken ct)
    {
        if (ct.IsCancellationRequested)
            return false;

        return ex switch
        {
            HttpRequestException => true,
            TaskCanceledException when !ct.IsCancellationRequested => true,
            _ => false
        };
    }
}
