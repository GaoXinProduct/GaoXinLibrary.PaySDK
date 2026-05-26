namespace GaoXinLibrary.PaySDK.UnionPay.Models;

/// <summary>
/// 银联退款响应参数
/// </summary>
public class UnionPayRefundResponse
{
    /// <summary>应答码：00 成功，其他失败</summary>
    public string RespCode { get; set; } = string.Empty;

    /// <summary>应答信息</summary>
    public string? RespMsg { get; set; }

    /// <summary>退货订单号</summary>
    public string? OrderId { get; set; }

    /// <summary>退货查询流水号</summary>
    public string? QueryId { get; set; }

    /// <summary>原始键值对</summary>
    public Dictionary<string, string> RawParams { get; set; } = new();
}
