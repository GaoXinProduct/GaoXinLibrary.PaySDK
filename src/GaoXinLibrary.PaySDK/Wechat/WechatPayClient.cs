using GaoXinLibrary.PaySDK.Core;
using GaoXinLibrary.PaySDK.Wechat.Core;
using GaoXinLibrary.PaySDK.Wechat.Services;

namespace GaoXinLibrary.PaySDK.Wechat;

/// <summary>
/// 微信支付 SDK 主客户端
/// <para>
/// 使用示例：
/// <code>
/// var client = WechatPayClient.Create(new WechatPayOptions
/// {
///     AppId       = "wx...",
///     MchId       = "1600...",
///     ApiV3Key    = "your_api_v3_key",
///     PrivateKey  = "your_private_key_pem",
///     CertSerialNo = "your_cert_serial_no"
/// });
/// var resp = await client.Pay.CreateJsapiOrderAsync(request);
/// </code>
/// </para>
/// </summary>
public sealed class WechatPayClient : IDisposable
{
    private readonly HttpClient _httpClient;
    private bool _disposed;

    /// <summary>微信支付服务（下单、查询、退款、账单、回调解析）</summary>
    public IWechatPayService Pay { get; }

    /// <summary>当前配置</summary>
    public WechatPayOptions Options { get; }

    private WechatPayClient(WechatPayOptions options, HttpClient httpClient)
    {
        Options = options;
        _httpClient = httpClient;

        var signer = new WechatPaySigner(options);
        var http = new WechatPayHttpClient(httpClient, options, signer);
        Pay = new WechatPayService(http, options, signer);
    }

    /// <summary>
    /// 创建微信支付客户端（使用默认 HttpClient）
    /// </summary>
    public static WechatPayClient Create(WechatPayOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        var httpClient = new HttpClient { Timeout = options.HttpTimeout };
        httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(PayConstants.UserAgent);
        return new WechatPayClient(options, httpClient);
    }

    /// <summary>
    /// 创建微信支付客户端（使用外部 HttpClient，适合 DI）
    /// </summary>
    public static WechatPayClient Create(WechatPayOptions options, HttpClient httpClient)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(httpClient);
        return new WechatPayClient(options, httpClient);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        _httpClient.Dispose();
    }
}
