using System.Text.Json.Serialization;

namespace GaoXinLibrary.PaySDK.Wechat.Models;

/// <summary>
/// 合单支付查询响应中的子订单信息
/// <para>https://pay.weixin.qq.com/doc/v3/merchant/4012791857</para>
/// </summary>
public class WechatCombineSubOrderResponse
{
    /// <summary>子订单商户号</summary>
    [JsonPropertyName("mchid")]
    public string MchId { get; set; } = string.Empty;

    /// <summary>交易类型：JSAPI / NATIVE / APP / MWEB</summary>
    [JsonPropertyName("trade_type")]
    public string? TradeType { get; set; }

    /// <summary>交易状态：SUCCESS / REFUND / NOTPAY / CLOSED / USERPAYING / PAYERROR</summary>
    [JsonPropertyName("trade_state")]
    public string TradeState { get; set; } = string.Empty;

    /// <summary>付款银行</summary>
    [JsonPropertyName("bank_type")]
    public string? BankType { get; set; }

    /// <summary>附加数据</summary>
    [JsonPropertyName("attach")]
    public string? Attach { get; set; }

    /// <summary>子订单金额</summary>
    [JsonPropertyName("amount")]
    public WechatCombineSubOrderAmount? Amount { get; set; }

    /// <summary>支付完成时间（rfc3339）</summary>
    [JsonPropertyName("success_time")]
    public string? SuccessTime { get; set; }

    /// <summary>微信支付订单号</summary>
    [JsonPropertyName("transaction_id")]
    public string? TransactionId { get; set; }

    /// <summary>子订单商户订单号</summary>
    [JsonPropertyName("out_trade_no")]
    public string OutTradeNo { get; set; } = string.Empty;

    /// <summary>子商户号</summary>
    [JsonPropertyName("sub_mchid")]
    public string? SubMchId { get; set; }
}
