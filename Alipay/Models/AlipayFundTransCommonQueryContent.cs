using System.Text.Json.Serialization;

namespace GaoXinLibrary.PaySDK.Alipay.Models;

/// <summary>
/// 支付宝资金转账查询请求
/// <para>alipay.fund.trans.common.query</para>
/// </summary>
public sealed class AlipayFundTransCommonQueryContent
{
    /// <summary>商户转账唯一订单号（与 order_id 二选一）</summary>
    [JsonPropertyName("out_biz_no")]
    public string? OutBizNo { get; set; }

    /// <summary>支付宝转账单据号（与 out_biz_no 二选一）</summary>
    [JsonPropertyName("order_id")]
    public string? OrderId { get; set; }

    /// <summary>产品码</summary>
    [JsonPropertyName("product_code")]
    public string? ProductCode { get; set; }

    /// <summary>业务场景</summary>
    [JsonPropertyName("biz_scene")]
    public string? BizScene { get; set; }
}
