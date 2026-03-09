using GaoXinLibrary.PaySDK.Core;
using GaoXinLibrary.PaySDK.UnionPay.Core;
using GaoXinLibrary.PaySDK.UnionPay.Services;

namespace GaoXinLibrary.PaySDK.UnionPay;

/// <summary>
/// 银联支付 SDK 主客户端
/// <para>
/// 使用示例：
/// <code>
/// var client = UnionPayClient.Create(new UnionPayOptions
/// {
///     MerId      = "your_mer_id",
///     CertId     = "your_cert_id",
///     PrivateKey = "your_private_key_pem",
///     UnionPayPublicKey = "unionpay_public_key_pem",
///     FrontUrl   = "https://your-site.com/pay/unionpay/front",
///     BackUrl    = "https://your-site.com/pay/unionpay/notify"
/// });
/// var resp = client.Pay.CreateFrontPay(request);
/// </code>
/// </para>
/// </summary>
public sealed class UnionPayClient : IDisposable
{
    private readonly HttpClient _httpClient;
    private bool _disposed;

    /// <summary>银联支付服务（创建支付表单、查询、退款、回调解析）</summary>
    public IUnionPayService Pay { get; }

    /// <summary>银联跨境电商海关申报服务（非支付接口）</summary>
    public IUnionPayCustomsService Customs { get; }

    /// <summary>当前配置</summary>
    public UnionPayOptions Options { get; }

    private UnionPayClient(UnionPayOptions options, HttpClient httpClient)
    {
        Options = options;
        _httpClient = httpClient;

        var signer = new UnionPaySigner(options);
        var http = new UnionPayHttpClient(httpClient, options, signer);
        Pay = new UnionPayService(http, options);
        Customs = new UnionPayCustomsService(http, options);
    }

    /// <summary>
    /// 创建银联支付客户端（使用默认 HttpClient）
    /// </summary>
    public static UnionPayClient Create(UnionPayOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        var httpClient = new HttpClient { Timeout = options.HttpTimeout };
        httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(PayConstants.UserAgent);
        return new UnionPayClient(options, httpClient);
    }

    /// <summary>
    /// 创建银联支付客户端（使用外部 HttpClient，适合 DI）
    /// </summary>
    public static UnionPayClient Create(UnionPayOptions options, HttpClient httpClient)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(httpClient);
        return new UnionPayClient(options, httpClient);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        _httpClient.Dispose();
    }
}
