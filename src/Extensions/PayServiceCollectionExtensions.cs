using GaoXinLibrary.PaySDK.Alipay.Core;
using GaoXinLibrary.PaySDK.Alipay.Services;
using GaoXinLibrary.PaySDK.Core;
using GaoXinLibrary.PaySDK.UnionPay.Core;
using GaoXinLibrary.PaySDK.UnionPay.Services;
using GaoXinLibrary.PaySDK.Wechat.Core;
using GaoXinLibrary.PaySDK.Wechat.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace GaoXinLibrary.PaySDK.Extensions;

/// <summary>
/// 统一支付 SDK DI 注入扩展
/// <para>
/// 用法一：单独注册各渠道
/// <code>
/// builder.Services
///     .AddWechatPay(opt => { ... })
///     .AddAlipay(opt => { ... })
///     .AddUnionPay(opt => { ... });
/// // 单独注入
/// public class MyService(IWechatPayService wechat, IAlipayService alipay) { ... }
/// </code>
/// </para>
/// <para>
/// 用法二：通过统一入口注册，注入 IPayService 统一接口
/// <code>
/// builder.Services.AddPaySDK(sdk =>
/// {
///     sdk.AddWechatPay(opt => { opt.AppId = "wx..."; ... });
///     sdk.AddAlipay(opt => { opt.AppId = "2021..."; ... });
///     sdk.AddUnionPay(opt => { opt.MerId = "your_mer_id"; ... });
/// });
/// // 统一注入
/// public class MyService(IPayService pay) { ... }
/// </code>
/// </para>
/// </summary>
public static class PayServiceCollectionExtensions
{
    /// <summary>
    /// 注册统一支付 SDK（通过回调依次配置各渠道，并自动注册 <see cref="IPayService"/>）
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <param name="configure">配置委托，在其中调用 AddWechatPay / AddAlipay / AddUnionPay</param>
    public static IServiceCollection AddPaySDK(
        this IServiceCollection services,
        Action<IServiceCollection> configure)
    {
        ArgumentNullException.ThrowIfNull(configure);
        configure(services);
        services.AddPayService();
        return services;
    }

    /// <summary>
    /// 仅注册 <see cref="IPayService"/> 统一路由实现（要求已先注册所需的渠道服务）
    /// <para>适合已单独调用 AddWechatPay / AddAlipay / AddUnionPay 后补充统一接口的场景</para>
    /// </summary>
    public static IServiceCollection AddPayService(this IServiceCollection services)
    {
        services.TryAddSingleton<IPayService>(sp =>
        {
            var wechat  = sp.GetService<IWechatPayService>();
            var alipay  = sp.GetService<IAlipayService>();
            var unionPay = sp.GetService<IUnionPayService>();
            return new PayService(wechat, alipay, unionPay);
        });

        return services;
    }
}
