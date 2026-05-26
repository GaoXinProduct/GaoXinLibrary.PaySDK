using System.Text.Json.Serialization;
using GaoXinLibrary.PaySDK.Alipay.Core;

namespace GaoXinLibrary.PaySDK.Alipay.Models;

/// <summary>
/// 支付宝订单码（预下单）响应
/// </summary>
public class AlipayTradePrecreateResponse : AlipayBaseResponse
{
    /// <summary>商户订单号</summary>
    [JsonPropertyName("out_trade_no")]
    public string? OutTradeNo { get; set; }

    /// <summary>二维码内容（前端使用该内容生成二维码）</summary>
    [JsonPropertyName("qr_code")]
    public string? QrCode { get; set; }
}
