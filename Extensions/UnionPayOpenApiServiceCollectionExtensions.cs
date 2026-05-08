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
    public static IServiceCollection AddUnionPayOpenApi(
        this IServiceCollection services,
        Action<UnionPayOpenApiOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(configure);
        var options = new UnionPayOpenApiOptions();
        configure(options);
        return services.AddUnionPayOpenApi(options);
    }

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
