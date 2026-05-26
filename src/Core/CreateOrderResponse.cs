namespace GaoXinLibrary.PaySDK.Core;

/// <summary>
/// 创建支付订单响应（统一参数）
/// </summary>
public class CreateOrderResponse
{
    /// <summary>支付渠道</summary>
    public PayChannel Channel { get; set; }

    /// <summary>商户订单号</summary>
    public string OutTradeNo { get; set; } = string.Empty;

    /// <summary>
    /// 预支付 ID（微信 JSAPI/APP/小程序，调起支付时使用）
    /// </summary>
    public string? PrepayId { get; set; }

    /// <summary>
    /// 二维码链接（微信 Native / 支付宝 Precreate / 银联扫码）
    /// </summary>
    public string? CodeUrl { get; set; }

    /// <summary>
    /// 跳转 URL（微信 H5 / 支付宝 WAP/PAGE / 银联前台跳转）
    /// </summary>
    public string? PayUrl { get; set; }

    /// <summary>
    /// 支付宝 SDK 调起字符串（App 支付，客户端 SDK 直接使用）
    /// </summary>
    public string? SdkOrderString { get; set; }

    /// <summary>
    /// 微信 JSAPI/小程序 调起支付参数（JSON 序列化后直接传给前端）
    /// </summary>
    public WechatJsPayParams? JsPayParams { get; set; }
}
