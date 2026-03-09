using System.Text.Json.Serialization;

namespace GaoXinLibrary.PaySDK.Wechat.Models;

/// <summary>
/// 微信支付下单请求基类
/// <para>https://pay.weixin.qq.com/doc/v3/merchant/4012062524</para>
/// </summary>
public abstract class WechatCreateOrderRequestBase
{
    /// <summary>公众号 ID / 小程序 AppID / APP AppID</summary>
    [JsonPropertyName("appid")]
    public string AppId { get; set; } = string.Empty;

    /// <summary>商户号</summary>
    [JsonPropertyName("mchid")]
    public string MchId { get; set; } = string.Empty;

    /// <summary>商品描述</summary>
    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    /// <summary>商户订单号，只能是数字、大小写字母_-*，且在同一个商户号下唯一</summary>
    [JsonPropertyName("out_trade_no")]
    public string OutTradeNo { get; set; } = string.Empty;

    /// <summary>交易结束时间，遵循 rfc3339 标准格式：yyyy-MM-DDTHH:mm:ss+TIMEZONE</summary>
    [JsonPropertyName("time_expire")]
    public string? TimeExpire { get; set; }

    /// <summary>附加数据，在查询 API 和支付通知中原样返回</summary>
    [JsonPropertyName("attach")]
    public string? Attach { get; set; }

    /// <summary>通知 URL（必须为外网可访问的 url，不能携带参数）</summary>
    [JsonPropertyName("notify_url")]
    public string NotifyUrl { get; set; } = string.Empty;

    /// <summary>订单优惠标记</summary>
    [JsonPropertyName("goods_tag")]
    public string? GoodsTag { get; set; }

    /// <summary>电子发票入口开放标识</summary>
    [JsonPropertyName("support_fapiao")]
    public bool? SupportFapiao { get; set; }

    /// <summary>订单金额</summary>
    [JsonPropertyName("amount")]
    public WechatPayAmount Amount { get; set; } = new();

    /// <summary>优惠功能</summary>
    [JsonPropertyName("detail")]
    public WechatPayDetail? Detail { get; set; }

    /// <summary>场景信息</summary>
    [JsonPropertyName("scene_info")]
    public WechatPaySceneInfo? SceneInfo { get; set; }

    /// <summary>结算信息</summary>
    [JsonPropertyName("settle_info")]
    public WechatPaySettleInfo? SettleInfo { get; set; }
}
