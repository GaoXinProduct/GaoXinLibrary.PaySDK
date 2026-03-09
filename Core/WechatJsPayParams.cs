namespace GaoXinLibrary.PaySDK.Core;

/// <summary>
/// 微信 JS-SDK / 小程序 调起支付所需参数
/// </summary>
public class WechatJsPayParams
{
    /// <summary>公众号 ID / 小程序 AppID</summary>
    public string AppId { get; set; } = string.Empty;

    /// <summary>时间戳（秒）</summary>
    public string TimeStamp { get; set; } = string.Empty;

    /// <summary>随机字符串</summary>
    public string NonceStr { get; set; } = string.Empty;

    /// <summary>订单详情扩展字符串（prepay_id=xxx）</summary>
    public string Package { get; set; } = string.Empty;

    /// <summary>签名方式，默认 RSA</summary>
    public string SignType { get; set; } = "RSA";

    /// <summary>签名</summary>
    public string PaySign { get; set; } = string.Empty;
}
