using System.Text.Json.Serialization;

namespace GaoXinLibrary.PaySDK.Alipay.Models;

/// <summary>
/// 支付宝下载账单业务请求内容
/// <para>alipay.data.dataservice.bill.downloadurl.query</para>
/// <para>https://opendocs.alipay.com/open/02ivbs</para>
/// </summary>
public class AlipayBillDownloadContent
{
    /// <summary>
    /// 账单类型：trade（商户实际收单的资金流水账单）/ signcustomer（合同账单）
    /// </summary>
    [JsonPropertyName("bill_type")]
    public string BillType { get; set; } = "trade";

    /// <summary>账单时间：日账单格式为 yyyy-MM-dd，月账单格式为 yyyy-MM</summary>
    [JsonPropertyName("bill_date")]
    public string BillDate { get; set; } = string.Empty;
}
