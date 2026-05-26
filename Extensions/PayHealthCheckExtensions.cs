using GaoXinLibrary.PaySDK.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace GaoXinLibrary.PaySDK.Extensions;

/// <summary>
/// 支付 SDK 健康检查 DI 注入扩展
/// </summary>
public static class PayHealthCheckExtensions
{
    /// <summary>
    /// 添加 PaySDK 健康检查，报告各渠道配置状态。
    /// <para>需先注册 <see cref="IPayService"/>（通过 <c>AddPaySDK</c> 或 <c>AddPayService</c>）。</para>
    /// </summary>
    public static IHealthChecksBuilder AddPayHealthChecks(this IHealthChecksBuilder builder)
    {
        builder.Services.AddSingleton<PayHealthCheck>();
        builder.AddCheck<PayHealthCheck>("pay_sdk", tags: ["payment"]);
        return builder;
    }
}
