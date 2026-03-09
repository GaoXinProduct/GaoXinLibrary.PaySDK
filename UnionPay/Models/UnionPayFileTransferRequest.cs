namespace GaoXinLibrary.PaySDK.UnionPay.Models;

/// <summary>
/// 银联文件传输请求参数
/// <para>
/// 商户可开发"文件传输类交易"接口获取对账文件。
/// </para>
/// </summary>
public class UnionPayFileTransferRequest
{
    /// <summary>交易类型，固定 76（文件传输）</summary>
    public string TxnType { get; set; } = "76";

    /// <summary>交易子类，固定 01（对账文件下载）</summary>
    public string TxnSubType { get; set; } = "01";

    /// <summary>产品类型，固定 000000</summary>
    public string BizType { get; set; } = "000000";

    /// <summary>商户订单号</summary>
    public string OrderId { get; set; } = string.Empty;

    /// <summary>交易时间，格式 yyyyMMddHHmmss</summary>
    public string TxnTime { get; set; } = string.Empty;

    /// <summary>
    /// 清算日期，格式 MMdd
    /// <para>要下载的对账文件日期</para>
    /// </summary>
    public string SettleDate { get; set; } = string.Empty;

    /// <summary>
    /// 文件类型
    /// <para>00 = 普通对账文件</para>
    /// </summary>
    public string FileType { get; set; } = "00";
}
