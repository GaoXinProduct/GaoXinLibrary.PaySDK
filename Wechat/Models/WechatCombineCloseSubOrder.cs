using System.Text.Json.Serialization;

namespace GaoXinLibrary.PaySDK.Wechat.Models;

/// <summary>
/// 合单关闭子订单信息
/// </summary>
public class WechatCombineCloseSubOrder
{
    /// <summary>子订单商户号</summary>
    [JsonPropertyName("mchid")]
    public string MchId { get; set; } = string.Empty;

    /// <summary>子订单商户订单号</summary>
    [JsonPropertyName("out_trade_no")]
    public string OutTradeNo { get; set; } = string.Empty;
}
