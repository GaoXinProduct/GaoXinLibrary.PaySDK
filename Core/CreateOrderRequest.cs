namespace GaoXinLibrary.PaySDK.Core;

/// <summary>
/// 创建支付订单请求（统一参数）
/// </summary>
public class CreateOrderRequest
{
    /// <summary>支付渠道（精确到子渠道，如 <see cref="PayChannel.WechatH5"/>、<see cref="PayChannel.AlipayApp"/>）</summary>
    public PayChannel Channel { get; set; }

    /// <summary>商户订单号（唯一）</summary>
    public string OutTradeNo { get; set; } = string.Empty;

    /// <summary>商品描述</summary>
    public string Subject { get; set; } = string.Empty;

    /// <summary>订单金额（分，人民币）</summary>
    public int TotalFee { get; set; }

    /// <summary>货币类型，默认 CNY</summary>
    public string Currency { get; set; } = "CNY";

    /// <summary>支付成功通知 URL</summary>
    public string NotifyUrl { get; set; } = string.Empty;

    /// <summary>支付成功回跳 URL（支付宝 H5/PC 有效）</summary>
    public string? ReturnUrl { get; set; }

    /// <summary>用户标识（微信 JSAPI/小程序 必填 openid；支付宝当面付必填 buyer_id）</summary>
    public string? OpenId { get; set; }

    /// <summary>用户付款码（支付宝当面付扫码支付必填）</summary>
    public string? AuthCode { get; set; }

    /// <summary>订单过期时间（UTC）</summary>
    public DateTimeOffset? ExpireTime { get; set; }

    /// <summary>附加数据（原样回传）</summary>
    public string? Attach { get; set; }

    /// <summary>客户端 IP（银联、支付宝 H5 必填）</summary>
    public string? ClientIp { get; set; }

    /// <summary>场景信息（微信 H5 必填 h5_type）</summary>
    public string? SceneType { get; set; }

    /// <summary>扩展参数（各渠道特有字段透传）</summary>
    public Dictionary<string, string>? Extra { get; set; }
}
