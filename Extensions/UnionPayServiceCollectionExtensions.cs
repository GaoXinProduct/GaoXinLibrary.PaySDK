using GaoXinLibrary.PaySDK.Core;
using GaoXinLibrary.PaySDK.UnionPay;
using GaoXinLibrary.PaySDK.UnionPay.Core;
using GaoXinLibrary.PaySDK.UnionPay.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace GaoXinLibrary.PaySDK.Extensions;

/// <summary>
/// 银联支付 DI 注入扩展
/// <para>
/// <code>
/// builder.Services.AddUnionPay(opt =>
/// {
///     opt.MerId      = "your_mer_id";
///     opt.CertId     = "your_cert_id";
///     opt.PrivateKey = "your_private_key_pem";
///     opt.UnionPayPublicKey = "unionpay_public_key_pem";
///     opt.FrontUrl   = "https://your-site.com/pay/unionpay/front";
///     opt.BackUrl    = "https://your-site.com/pay/unionpay/notify";
/// });
/// // 注入:
/// public class MyService(IUnionPayService pay) { ... }
/// </code>
/// </para>
/// </summary>
public static class UnionPayServiceCollectionExtensions
{
    /// <summary>
    /// 注册银联支付服务（使用委托配置选项）
    /// </summary>
    public static IServiceCollection AddUnionPay(
        this IServiceCollection services,
        Action<UnionPayOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(configure);
        var options = new UnionPayOptions();
        configure(options);
        return services.AddUnionPay(options);
    }

    /// <summary>
    /// 注册银联支付服务（使用已有配置对象）
    /// </summary>
    public static IServiceCollection AddUnionPay(
        this IServiceCollection services,
        UnionPayOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        services.TryAddSingleton(options);

        services.AddHttpClient(UnionPayConstants.HttpClientName, client =>
        {
            client.Timeout = options.HttpTimeout;
            client.DefaultRequestHeaders.UserAgent.ParseAdd(PayConstants.UserAgent);
        });

        services.TryAddSingleton<UnionPayClient>(sp =>
        {
            var factory = sp.GetRequiredService<IHttpClientFactory>();
            var httpClient = factory.CreateClient(UnionPayConstants.HttpClientName);
            return UnionPayClient.Create(options, httpClient);
        });

        services.TryAddSingleton<IUnionPayService>(sp =>
            sp.GetRequiredService<UnionPayClient>().Pay);

        services.TryAddSingleton<IUnionPayCustomsService>(sp =>
            sp.GetRequiredService<UnionPayClient>().Customs);

        return services;
    }
}
