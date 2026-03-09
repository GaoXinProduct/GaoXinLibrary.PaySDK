using System.Text;
using System.Web;
using GaoXinLibrary.PaySDK.Core;

namespace GaoXinLibrary.PaySDK.UnionPay.Core;

/// <summary>
/// 银联支付 HTTP 客户端封装
/// <para>自动组装公共参数、签名、发起 Form POST，并解析响应</para>
/// </summary>
public sealed class UnionPayHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly UnionPayOptions _options;
    private readonly UnionPaySigner _signer;

    /// <summary>
    /// 初始化银联 HTTP 客户端
    /// </summary>
    public UnionPayHttpClient(HttpClient httpClient, UnionPayOptions options, UnionPaySigner signer)
    {
        _httpClient = httpClient;
        _options = options;
        _signer = signer;
    }

    /// <summary>
    /// 向银联后台网关发起 Form POST 请求并返回解析后的键值对响应
    /// </summary>
    public async Task<Dictionary<string, string>> PostBackAsync(
        Dictionary<string, string> parameters,
        string gatewayUrl,
        CancellationToken ct = default)
    {
        AddCommonParams(parameters);
        parameters["signature"] = _signer.Sign(parameters);

        var formContent = new FormUrlEncodedContent(parameters);

        using var request = new HttpRequestMessage(HttpMethod.Post, gatewayUrl);
        request.Headers.UserAgent.ParseAdd(PayConstants.UserAgent);
        request.Content = formContent;
        using var response = await _httpClient.SendAsync(request, ct);
        var body = await response.Content.ReadAsStringAsync(ct);
        return ParseFormResponse(body);
    }

    /// <summary>
    /// 构建前台跳转表单 URL（前台支付，将用户浏览器 POST 到银联网关）
    /// </summary>
    public string BuildFrontFormHtml(Dictionary<string, string> parameters)
    {
        return BuildFrontFormHtml(parameters, _options.FrontGatewayUrl);
    }

    /// <summary>
    /// 构建前台跳转表单 URL（指定网关地址）
    /// </summary>
    public string BuildFrontFormHtml(Dictionary<string, string> parameters, string gatewayUrl)
    {
        AddCommonParams(parameters);
        parameters["signature"] = _signer.Sign(parameters);

        var sb = new StringBuilder();
        sb.AppendLine($"<form id=\"unionpay_submit\" name=\"unionpay_submit\" action=\"{HttpUtility.HtmlEncode(gatewayUrl)}\" method=\"post\">");
        foreach (var kv in parameters)
        {
            sb.AppendLine($"  <input type=\"hidden\" name=\"{HttpUtility.HtmlEncode(kv.Key)}\" value=\"{HttpUtility.HtmlEncode(kv.Value)}\" />");
        }
        sb.AppendLine("</form>");
        sb.AppendLine("<script>document.forms['unionpay_submit'].submit();</script>");
        return sb.ToString();
    }

    /// <summary>
    /// 验证银联回调通知签名
    /// </summary>
    public bool VerifyCallback(Dictionary<string, string> parameters)
    {
        if (!parameters.TryGetValue("signature", out var signature))
            return false;

        var paramsCopy = new Dictionary<string, string>(parameters, StringComparer.Ordinal);
        paramsCopy.Remove("signature");
        paramsCopy.Remove("signPubKeyCert");
        return _signer.Verify(paramsCopy, signature);
    }

    /// <summary>
    /// 下载文件（对账文件下载）
    /// </summary>
    public async Task<byte[]> DownloadFileAsync(
        Dictionary<string, string> parameters,
        string gatewayUrl,
        CancellationToken ct = default)
    {
        AddCommonParams(parameters);
        parameters["signature"] = _signer.Sign(parameters);

        var formContent = new FormUrlEncodedContent(parameters);

        using var request = new HttpRequestMessage(HttpMethod.Post, gatewayUrl);
        request.Headers.UserAgent.ParseAdd(PayConstants.UserAgent);
        request.Content = formContent;
        using var response = await _httpClient.SendAsync(request, ct);
        return await response.Content.ReadAsByteArrayAsync(ct);
    }

    private void AddCommonParams(Dictionary<string, string> parameters)
    {
        parameters.TryAdd("version", _options.Version);
        parameters.TryAdd("encoding", _options.Encoding);
        parameters.TryAdd("signMethod", _options.SignMethod);
        parameters.TryAdd("merId", _options.MerId);
        parameters.TryAdd("certId", _options.CertId);
        parameters.TryAdd("channelType", "07");
        parameters.TryAdd("currencyCode", "156");
    }

    private static Dictionary<string, string> ParseFormResponse(string body)
    {
        var result = new Dictionary<string, string>(StringComparer.Ordinal);
        if (string.IsNullOrWhiteSpace(body)) return result;

        foreach (var pair in body.Split('&'))
        {
            var idx = pair.IndexOf('=');
            if (idx < 0) continue;
            var key = HttpUtility.UrlDecode(pair[..idx]);
            var val = HttpUtility.UrlDecode(pair[(idx + 1)..]);
            result[key] = val;
        }
        return result;
    }
}
