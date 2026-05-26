using System.ComponentModel.DataAnnotations;
using GaoXinLibrary.PaySDK.UnionPay.OpenApi;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace GaoXinLibrary.PaySDK.Extensions;

/// <summary>
/// 银联 OpenAPI 独立模块 DI 注入扩展
/// </summary>
public static class UnionPayOpenApiServiceCollectionExtensions
{
    /// <summary>
    /// 注册银联 OpenAPI 服务（使用配置委托）
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <param name="configure">OpenAPI 配置委托</param>
    /// <returns>服务集合（支持链式调用）</returns>
    public static IServiceCollection AddUnionPayOpenApi(
        this IServiceCollection services,
        Action<UnionPayOpenApiOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(configure);
        var options = new UnionPayOpenApiOptions();
        configure(options);
        return services.AddUnionPayOpenApi(options);
    }

    /// <summary>
    /// 注册银联 OpenAPI 服务（直接传入配置对象）
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <param name="options">OpenAPI 配置对象（会进行 <see cref="System.ComponentModel.DataAnnotations.Validator"/> 校验）</param>
    /// <returns>服务集合（支持链式调用）</returns>
    public static IServiceCollection AddUnionPayOpenApi(
        this IServiceCollection services,
        UnionPayOpenApiOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        Validator.ValidateObject(options, new ValidationContext(options), validateAllProperties: true);

        services.TryAddSingleton(options);
        services.AddHttpClient("UnionPayOpenApi", client =>
        {
            client.BaseAddress = new Uri(options.BaseUrl);
            client.Timeout = options.HttpTimeout;
        });
        services.TryAddSingleton<IUnionPayOpenApiService>(sp =>
        {
            var factory = sp.GetRequiredService<IHttpClientFactory>();
            var client = factory.CreateClient("UnionPayOpenApi");
            return new UnionPayOpenApiService(client, options);
        });
        return services;
    }
}
