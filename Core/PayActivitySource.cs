using System.Diagnostics;

namespace GaoXinLibrary.PaySDK.Core;

/// <summary>
/// OpenTelemetry 兼容的 ActivitySource，用于所有支付渠道的分布式追踪。
/// </summary>
public static class PayActivitySource
{
    /// <summary>ActivitySource 名称和版本</summary>
    public static readonly ActivitySource Source = new("GaoXinLibrary.PaySDK", "1.0.0");
}
