using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace GaoXinLibrary.PaySDK.Core;

/// <summary>
/// 支付 SDK 健康检查，报告各渠道配置状态
/// </summary>
public class PayHealthCheck : IHealthCheck
{
    private readonly IPayService? _payService;

    public PayHealthCheck(IPayService? payService = null)
    {
        _payService = payService;
    }

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken ct = default)
    {
        var data = new Dictionary<string, object>();
        var configured = _payService != null;
        data["channels.wechat"] = configured ? "configured" : "not_configured";
        data["channels.alipay"] = configured ? "configured" : "not_configured";
        data["channels.unionpay"] = configured ? "configured" : "not_configured";
        data["sdk.version"] = "1.0.0";

        return Task.FromResult(HealthCheckResult.Healthy("PaySDK is operational", data));
    }
}
