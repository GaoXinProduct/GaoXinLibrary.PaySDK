using GaoXinLibrary.PaySDK.Alipay.Core;
using GaoXinLibrary.PaySDK.Alipay.Services;
using GaoXinLibrary.PaySDK.Core;

namespace GaoXinLibrary.PaySDK.Alipay;

/// <summary>
/// 支付宝支付 SDK 主客户端
/// <para>
/// 使用示例：
/// <code>
/// var client = AlipayClient.Create(new AlipayOptions
/// {
///     AppId           = "2021...",
///     PrivateKey      = "your_private_key",
///     AlipayPublicKey = "alipay_public_key"
/// });
/// var resp = await client.Pay.PrecreateAsync(content);
/// </code>
/// </para>
/// </summary>
public sealed class AlipayClient : IDisposable
{
    private readonly HttpClient _httpClient;
    private bool _disposed;

    /// <summary>支付宝支付服务（当面付、订单码、App、H5、PC、查询、退款、账单）</summary>
    public IAlipayService Pay { get; }

    /// <summary>当前配置</summary>
    public AlipayOptions Options { get; }

    private AlipayClient(AlipayOptions options, HttpClient httpClient)
    {
        Options = options;
        _httpClient = httpClient;

        var signer = new AlipaySigner(options);
        var http = new AlipayHttpClient(httpClient, options, signer);
        Pay = new AlipayService(http, options, signer);
    }

    /// <summary>
    /// 创建支付宝客户端（使用默认 HttpClient）
    /// </summary>
    public static AlipayClient Create(AlipayOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        var httpClient = new HttpClient { Timeout = options.HttpTimeout };
        httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(PayConstants.UserAgent);
        return new AlipayClient(options, httpClient);
    }

    /// <summary>
    /// 创建支付宝客户端（使用外部 HttpClient，适合 DI）
    /// </summary>
    public static AlipayClient Create(AlipayOptions options, HttpClient httpClient)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(httpClient);
        return new AlipayClient(options, httpClient);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        _httpClient.Dispose();
    }
}
