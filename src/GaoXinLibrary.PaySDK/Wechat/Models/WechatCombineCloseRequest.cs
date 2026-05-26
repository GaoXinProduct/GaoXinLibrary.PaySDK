using System.Text.Json.Serialization;

namespace GaoXinLibrary.PaySDK.Wechat.Models;

/// <summary>
/// 合单支付关闭订单请求
/// <para>https://pay.weixin.qq.com/doc/v3/merchant/4012791863</para>
/// </summary>
public class WechatCombineCloseRequest
{
    /// <summary>合单商户 appid</summary>
    [JsonPropertyName("combine_appid")]
    public string CombineAppId { get; set; } = string.Empty;

    /// <summary>子订单信息列表</summary>
    [JsonPropertyName("sub_orders")]
    public List<WechatCombineCloseSubOrder> SubOrders { get; set; } = new();
}
