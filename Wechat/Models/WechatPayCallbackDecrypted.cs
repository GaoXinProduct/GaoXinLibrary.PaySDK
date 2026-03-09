using System.Text.Json.Serialization;

namespace GaoXinLibrary.PaySDK.Wechat.Models;

/// <summary>
/// 微信支付回调通知解密后的支付订单信息
/// </summary>
public class WechatPayCallbackDecrypted
{
    /// <summary>公众号 ID</summary>
    [JsonPropertyName("appid")]
    public string AppId { get; set; } = string.Empty;

    /// <summary>商户号</summary>
    [JsonPropertyName("mchid")]
    public string MchId { get; set; } = string.Empty;

    /// <summary>商户订单号</summary>
    [JsonPropertyName("out_trade_no")]
    public string OutTradeNo { get; set; } = string.Empty;

    /// <summary>微信支付订单号</summary>
    [JsonPropertyName("transaction_id")]
    public string? TransactionId { get; set; }

    /// <summary>交易类型</summary>
    [JsonPropertyName("trade_type")]
    public string? TradeType { get; set; }

    /// <summary>交易状态</summary>
    [JsonPropertyName("trade_state")]
    public string TradeState { get; set; } = string.Empty;

    /// <summary>交易状态描述</summary>
    [JsonPropertyName("trade_state_desc")]
    public string? TradeStateDesc { get; set; }

    /// <summary>付款银行</summary>
    [JsonPropertyName("bank_type")]
    public string? BankType { get; set; }

    /// <summary>附加数据</summary>
    [JsonPropertyName("attach")]
    public string? Attach { get; set; }

    /// <summary>支付完成时间（rfc3339）</summary>
    [JsonPropertyName("success_time")]
    public string? SuccessTime { get; set; }

    /// <summary>支付者信息</summary>
    [JsonPropertyName("payer")]
    public WechatPayPayer? Payer { get; set; }

    /// <summary>订单金额</summary>
    [JsonPropertyName("amount")]
    public WechatQueryOrderAmount? Amount { get; set; }
}
