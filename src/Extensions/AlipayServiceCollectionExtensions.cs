using System.ComponentModel.DataAnnotations;
using GaoXinLibrary.PaySDK.Alipay;
using GaoXinLibrary.PaySDK.Alipay.Core;
using GaoXinLibrary.PaySDK.Alipay.Services;
using GaoXinLibrary.PaySDK.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace GaoXinLibrary.PaySDK.Extensions;

/// <summary>
/// 支付宝 DI 注入扩展
/// <para>
/// <code>
/// builder.Services.AddAlipay(opt =>
/// {
///     opt.AppId           = "2021...";
///     opt.PrivateKey      = "your_private_key";
///     opt.AlipayPublicKey = "alipay_public_key";
/// });
/// // 注入:
/// public class MyService(IAlipayService pay) { ... }
/// </code>
/// </para>
/// </summary>
public static class AlipayServiceCollectionExtensions
{
    /// <summary>
    /// 注册支付宝服务（使用委托配置选项）
    /// </summary>
    public static IServiceCollection AddAlipay(
        this IServiceCollection services,
        Action<AlipayOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(configure);
        var options = new AlipayOptions();
        configure(options);
        return services.AddAlipay(options);
    }

    /// <summary>
    /// 注册支付宝服务（使用已有配置对象）
    /// </summary>
    public static IServiceCollection AddAlipay(
        this IServiceCollection services,
        AlipayOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        Validator.ValidateObject(options, new ValidationContext(options), validateAllProperties: true);

        services.TryAddSingleton(options);

        services.AddHttpClient(AlipayConstants.HttpClientName, client =>
        {
            client.Timeout = options.HttpTimeout;
            client.DefaultRequestHeaders.UserAgent.ParseAdd(PayConstants.UserAgent);
        });

        services.TryAddSingleton<AlipayClient>(sp =>
        {
            var factory = sp.GetRequiredService<IHttpClientFactory>();
            var httpClient = factory.CreateClient(AlipayConstants.HttpClientName);
            return AlipayClient.Create(options, httpClient);
        });

        services.TryAddSingleton<IAlipayService>(sp =>
            sp.GetRequiredService<AlipayClient>().Pay);

        return services;
    }
}
