namespace GaoXinLibrary.PaySDK.UnionPay.Models;

/// <summary>
/// 银联代付响应参数
/// </summary>
public class UnionPayPaymentResponse
{
    /// <summary>应答码：00 成功，其他失败</summary>
    public string RespCode { get; set; } = string.Empty;

    /// <summary>应答信息</summary>
    public string? RespMsg { get; set; }

    /// <summary>商户订单号</summary>
    public string? OrderId { get; set; }

    /// <summary>银联查询流水号</summary>
    public string? QueryId { get; set; }

    /// <summary>交易金额（分）</summary>
    public string? TxnAmt { get; set; }

    /// <summary>清算金额（分）</summary>
    public string? SettleAmt { get; set; }

    /// <summary>清算日期</summary>
    public string? SettleDate { get; set; }

    /// <summary>原始键值对</summary>
    public Dictionary<string, string> RawParams { get; set; } = new();
}
