namespace GaoXinLibrary.PaySDK.Core;

/// <summary>
/// 下载账单请求
/// </summary>
public class DownloadBillRequest
{
    /// <summary>支付渠道</summary>
    public PayChannel Channel { get; set; }

    /// <summary>账单日期，格式 yyyyMMdd</summary>
    public string BillDate { get; set; } = string.Empty;

    /// <summary>
    /// 账单类型
    /// <para>微信：ALL / SUCCESS / REFUND；支付宝：trade / signcustomer</para>
    /// </summary>
    public string BillType { get; set; } = "ALL";
}
