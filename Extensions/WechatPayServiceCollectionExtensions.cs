using GaoXinLibrary.PaySDK.Core;
using GaoXinLibrary.PaySDK.Wechat;
using GaoXinLibrary.PaySDK.Wechat.Core;
using GaoXinLibrary.PaySDK.Wechat.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace GaoXinLibrary.PaySDK.Extensions;

/// <summary>
/// 微信支付 DI 注入扩展
/// <para>
/// <code>
/// builder.Services.AddWechatPay(opt =>
/// {
///     opt.AppId        = "wx...";
///     opt.MchId        = "1600...";
///     opt.ApiV3Key     = "your_api_v3_key";
///     opt.PrivateKey   = "your_private_key_pem";
///     opt.CertSerialNo = "your_cert_serial_no";
///     // 回调验签必须配置平台证书公钥（从商户平台下载或 /v3/certificates 接口获取）
///     opt.PlatformPublicKey = "-----BEGIN CERTIFICATE-----\n...\n-----END CERTIFICATE-----";
///     // 开发测试时可跳过验签（生产环境禁用）
///     // opt.SkipSignatureVerification = true;
/// });
/// // 注入:
/// public class MyService(IWechatPayService pay) { ... }
/// </code>
/// </para>
/// </summary>
public static class WechatPayServiceCollectionExtensions
{
    /// <summary>
    /// 注册微信支付服务（使用委托配置选项）
    /// </summary>
    public static IServiceCollection AddWechatPay(
        this IServiceCollection services,
        Action<WechatPayOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(configure);
        var options = new WechatPayOptions();
        configure(options);
        return services.AddWechatPay(options);
    }

    /// <summary>
    /// 注册微信支付服务（使用已有配置对象）
    /// </summary>
    public static IServiceCollection AddWechatPay(
        this IServiceCollection services,
        WechatPayOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        services.TryAddSingleton(options);

        services.AddHttpClient(WechatPayConstants.HttpClientName, client =>
        {
            client.Timeout = options.HttpTimeout;
            client.DefaultRequestHeaders.UserAgent.ParseAdd(PayConstants.UserAgent);
        });

        services.TryAddSingleton<WechatPayClient>(sp =>
        {
            var factory = sp.GetRequiredService<IHttpClientFactory>();
            var httpClient = factory.CreateClient(WechatPayConstants.HttpClientName);
            return WechatPayClient.Create(options, httpClient);
        });

        services.TryAddSingleton<IWechatPayService>(sp =>
            sp.GetRequiredService<WechatPayClient>().Pay);

        return services;
    }
}
