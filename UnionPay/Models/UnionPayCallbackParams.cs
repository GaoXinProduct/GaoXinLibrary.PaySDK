namespace GaoXinLibrary.PaySDK.UnionPay.Models;

/// <summary>
/// 银联后台通知回调参数
/// </summary>
public class UnionPayCallbackParams
{
    /// <summary>验签是否通过</summary>
    public bool IsValid { get; set; }

    /// <summary>应答码：00 成功</summary>
    public string? RespCode { get; set; }

    /// <summary>应答信息</summary>
    public string? RespMsg { get; set; }

    /// <summary>商户订单号</summary>
    public string? OrderId { get; set; }

    /// <summary>银联查询流水号</summary>
    public string? QueryId { get; set; }

    /// <summary>交易金额（分）</summary>
    public string? TxnAmt { get; set; }

    /// <summary>清算日期</summary>
    public string? SettleDate { get; set; }

    /// <summary>商户附加信息</summary>
    public string? ReqReserved { get; set; }

    /// <summary>原始键值对</summary>
    public Dictionary<string, string> RawParams { get; set; } = new();
}
