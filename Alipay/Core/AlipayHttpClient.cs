using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;
using GaoXinLibrary.PaySDK.Core;

namespace GaoXinLibrary.PaySDK.Alipay.Core;

/// <summary>
/// 支付宝网关 HTTP 客户端封装
/// <para>自动组装公共参数、排序签名、发起 Form POST，并解析响应</para>
/// </summary>
public sealed class AlipayHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly AlipayOptions _options;
    private readonly AlipaySigner _signer;

    internal static readonly JsonSerializerOptions JsonOptions = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    /// <summary>
    /// 初始化支付宝 HTTP 客户端
    /// </summary>
    public AlipayHttpClient(HttpClient httpClient, AlipayOptions options, AlipaySigner signer)
    {
        _httpClient = httpClient;
        _options = options;
        _signer = signer;
    }

    /// <summary>
    /// 执行支付宝网关 API 请求（Form POST），返回反序列化后的响应
    /// </summary>
    /// <param name="extra">需要合并到公共参数的额外外层参数（如 notify_url）</param>
    public async Task<T> ExecuteAsync<T>(string method, object bizContent, CancellationToken ct = default, Dictionary<string, string>? extra = null)
        where T : AlipayBaseResponse
    {
        var bizContentJson = JsonSerializer.Serialize(bizContent, JsonOptions);
        var parameters = BuildCommonParameters(method, bizContentJson, extra);
        var signContent = BuildSignContent(parameters);
        parameters["sign"] = _signer.Sign(signContent);

        var formContent = new FormUrlEncodedContent(parameters);
        formContent.Headers.ContentType!.CharSet = "utf-8";

        using var request = new HttpRequestMessage(HttpMethod.Post, _options.GatewayUrl);
        request.Headers.UserAgent.ParseAdd(PayConstants.UserAgent);
        request.Content = formContent;
        using var response = await _httpClient.SendAsync(request, ct);
        var responseBytes = await response.Content.ReadAsByteArrayAsync(ct);
        var charSet = response.Content.Headers.ContentType?.CharSet?.Trim('"') ?? "utf-8";
        Encoding encoding;
        try { encoding = Encoding.GetEncoding(charSet); }
        catch { encoding = Encoding.UTF8; }
        var responseJson = encoding.GetString(responseBytes);

        return ParseResponse<T>(method, responseJson);
    }

    /// <summary>
    /// 构建页面跳转表单（APP / H5 / PC 网站支付，返回给前端签名字符串）
    /// </summary>
    public string BuildPageForm(string method, object bizContent)
        => BuildPageFormWithExtra(method, bizContent, null);

    /// <summary>
    /// 构建 SDK 签名字符串（App 支付使用）
    /// </summary>
    public string BuildSdkString(string method, object bizContent)
        => BuildSdkStringWithExtra(method, bizContent, null);

    /// <summary>
    /// 构建 SDK 签名字符串（携带额外公共参数，如 notify_url）
    /// </summary>
    public string BuildSdkStringWithExtra(string method, object bizContent, Dictionary<string, string>? extra)
    {
        var bizContentJson = JsonSerializer.Serialize(bizContent, JsonOptions);
        var parameters = BuildCommonParameters(method, bizContentJson);
        if (extra != null)
        {
            foreach (var kv in extra)
                parameters[kv.Key] = kv.Value;
        }
        var signContent = BuildSignContent(parameters);
        parameters["sign"] = _signer.Sign(signContent);

        var sb = new StringBuilder();
        foreach (var kv in parameters)
        {
            sb.Append(HttpUtility.UrlEncode(kv.Key, Encoding.UTF8))
              .Append('=')
              .Append(HttpUtility.UrlEncode(kv.Value, Encoding.UTF8))
              .Append('&');
        }
        if (sb.Length > 0 && sb[^1] == '&')
            sb.Length--;
        return sb.ToString();
    }

    /// <summary>
    /// 构建页面跳转 URL（携带额外公共参数，如 notify_url / return_url）
    /// </summary>
    public string BuildPageFormWithExtra(string method, object bizContent, Dictionary<string, string>? extra)
    {
        var bizContentJson = JsonSerializer.Serialize(bizContent, JsonOptions);
        var parameters = BuildCommonParameters(method, bizContentJson);
        if (extra != null)
        {
            foreach (var kv in extra)
                parameters[kv.Key] = kv.Value;
        }
        var signContent = BuildSignContent(parameters);
        parameters["sign"] = _signer.Sign(signContent);

        var sb = new StringBuilder();
        sb.Append(_options.GatewayUrl).Append('?');
        foreach (var kv in parameters)
        {
            sb.Append(HttpUtility.UrlEncode(kv.Key, Encoding.UTF8))
              .Append('=')
              .Append(HttpUtility.UrlEncode(kv.Value, Encoding.UTF8))
              .Append('&');
        }
        if (sb[^1] == '&')
            sb.Length--;
        return sb.ToString();
    }

    private Dictionary<string, string> BuildCommonParameters(string method, string bizContentJson, Dictionary<string, string>? extra = null)
    {
        var parameters = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["app_id"] = _options.AppId,
            ["method"] = method,
            ["format"] = _options.Format,
            ["charset"] = _options.Charset,
            ["sign_type"] = _options.SignType,
            ["timestamp"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            ["version"] = _options.Version,
            ["biz_content"] = bizContentJson
        };
        if (extra != null)
            foreach (var kv in extra)
                parameters[kv.Key] = kv.Value;
        return parameters;
    }

    private static string BuildSignContent(Dictionary<string, string> parameters)
    {
        var sb = new StringBuilder();
        foreach (var kv in parameters.OrderBy(x => x.Key, StringComparer.Ordinal))
        {
            if (string.IsNullOrEmpty(kv.Value) || kv.Key == "sign") continue;
            if (sb.Length > 0) sb.Append('&');
            sb.Append(kv.Key).Append('=').Append(kv.Value);
        }
        return sb.ToString();
    }

    /// <summary>
    /// 下载外部资源字节（账单下载使用，复用内部 HttpClient）
    /// </summary>
    public async Task<byte[]> DownloadBytesAsync(string url, CancellationToken ct = default)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.UserAgent.ParseAdd(PayConstants.UserAgent);
        using var response = await _httpClient.SendAsync(request, ct);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsByteArrayAsync(ct);
    }

    private static T ParseResponse<T>(string method, string responseJson) where T : AlipayBaseResponse
    {
        using var doc = JsonDocument.Parse(responseJson);
        var root = doc.RootElement;

        var responseKey = method.Replace('.', '_') + "_response";

        // 网关级错误（签名错误、参数缺失等）：支付宝使用 error_response 作为响应 key
        if (!root.TryGetProperty(responseKey, out var responseElement))
        {
            if (root.TryGetProperty("error_response", out var errorElement))
            {
                var errResult = errorElement.Deserialize<AlipayBaseResponse>(JsonOptions);
                var errCode = errResult?.SubCode ?? errResult?.Code ?? "GATEWAY_ERROR";
                var errMsg  = errResult?.SubMsg  ?? errResult?.Msg  ?? "支付宝网关拒绝请求";
                throw new AlipayException(errCode, errMsg);
            }
            throw new AlipayException("INVALID_RESPONSE", $"支付宝响应中未找到 {responseKey}，原始响应：{responseJson}");
        }

        var result = responseElement.Deserialize<T>(JsonOptions)
            ?? throw new AlipayException("DESERIALIZE_FAILED", "反序列化支付宝响应失败");

        if (result.Code != "10000")
        {
            // 优先使用业务级 sub_code，回退到网关级 code
            var errCode = result.SubCode ?? result.Code ?? "UNKNOWN";
            var errMsg  = result.SubMsg  ?? result.Msg  ?? "未知错误";
            throw new AlipayException(errCode, errMsg);
        }

        return result;
    }
}
