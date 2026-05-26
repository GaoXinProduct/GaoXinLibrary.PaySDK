using System.Text.Json.Serialization;

namespace GaoXinLibrary.PaySDK.Wechat.Models;

/// <summary>
/// 合单支付子订单信息
/// <para>https://pay.weixin.qq.com/doc/v3/merchant/4012791856</para>
/// </summary>
public class WechatCombineSubOrder
{
    /// <summary>子订单商户号</summary>
    [JsonPropertyName("mchid")]
    public string MchId { get; set; } = string.Empty;

    /// <summary>附加数据，在查询 API 和支付通知中原样返回</summary>
    [JsonPropertyName("attach")]
    public string? Attach { get; set; }

    /// <summary>子订单金额</summary>
    [JsonPropertyName("amount")]
    public WechatCombineSubOrderAmount Amount { get; set; } = new();

    /// <summary>子订单商品描述</summary>
    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    /// <summary>子订单商户订单号</summary>
    [JsonPropertyName("out_trade_no")]
    public string OutTradeNo { get; set; } = string.Empty;

    /// <summary>子商户号（服务商/连锁店等场景）</summary>
    [JsonPropertyName("sub_mchid")]
    public string? SubMchId { get; set; }

    /// <summary>订单优惠标记</summary>
    [JsonPropertyName("goods_tag")]
    public string? GoodsTag { get; set; }

    /// <summary>子订单优惠功能</summary>
    [JsonPropertyName("detail")]
    public WechatPayDetail? Detail { get; set; }

    /// <summary>子订单结算信息</summary>
    [JsonPropertyName("settle_info")]
    public WechatPaySettleInfo? SettleInfo { get; set; }
}
