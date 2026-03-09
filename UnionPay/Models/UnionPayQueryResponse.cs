namespace GaoXinLibrary.PaySDK.UnionPay.Models;

/// <summary>
/// 银联订单查询响应参数
/// </summary>
public class UnionPayQueryResponse
{
    /// <summary>应答码：00 成功，其他失败</summary>
    public string RespCode { get; set; } = string.Empty;

    /// <summary>应答信息</summary>
    public string? RespMsg { get; set; }

    /// <summary>商户订单号</summary>
    public string? OrderId { get; set; }

    /// <summary>银联查询流水号</summary>
    public string? QueryId { get; set; }

    /// <summary>
    /// 交易状态：00 成功 / 01 待支付 / 02 退款 / 03 已关闭 / 05 已撤销 / 06 未确认 / 07 已对账
    /// </summary>
    public string? OrigRespCode { get; set; }

    /// <summary>原始应答信息</summary>
    public string? OrigRespMsg { get; set; }

    /// <summary>交易金额（分）</summary>
    public string? TxnAmt { get; set; }

    /// <summary>清算金额（分）</summary>
    public string? SettleAmt { get; set; }

    /// <summary>清算日期</summary>
    public string? SettleDate { get; set; }

    /// <summary>原始键值对</summary>
    public Dictionary<string, string> RawParams { get; set; } = new();
}
