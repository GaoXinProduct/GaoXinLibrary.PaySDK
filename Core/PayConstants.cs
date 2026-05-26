namespace GaoXinLibrary.PaySDK.Core;

/// <summary>
/// 支付 SDK 全局常量
/// </summary>
internal static class PayConstants
{
    /// <summary>HTTP 请求 User-Agent 头，格式：GaoXinLibrary.PaySDK/{version}</summary>
    public static readonly string UserAgent = $"GaoXinLibrary.PaySDK/1.0.0";

    /// <summary>OpenTelemetry ActivitySource 名称</summary>
    public const string ActivitySourceName = "GaoXinLibrary.PaySDK";

    /// <summary>支付宝沙箱网关地址</summary>
    public const string AlipaySandboxGatewayUrl = "https://openapi-sandbox.dl.alipaydev.com/gateway.do";

    /// <summary>支付宝生产环境网关地址</summary>
    public const string AlipayProductionGatewayUrl = "https://openapi.alipay.com/gateway.do";

    // 微信支付没有公开沙箱环境，开发测试可使用商户平台的测试商户号进行联调。
    // 银联测试环境需向银联申请测试商户号，地址：https://open.unionpay.com/tjweb/support/doc/online/3/125
}

/// <summary>
/// 支付环境模式
/// </summary>
public enum PayEnvironment
{
    /// <summary>生产环境</summary>
    Production,

    /// <summary>沙箱/测试环境</summary>
    Sandbox
}
