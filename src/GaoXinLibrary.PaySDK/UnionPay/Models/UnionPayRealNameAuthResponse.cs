namespace GaoXinLibrary.PaySDK.UnionPay.Models;

/// <summary>
/// 银联实名认证响应参数
/// </summary>
public class UnionPayRealNameAuthResponse
{
    /// <summary>应答码：00 成功，其他失败</summary>
    public string RespCode { get; set; } = string.Empty;

    /// <summary>应答信息</summary>
    public string? RespMsg { get; set; }

    /// <summary>商户订单号</summary>
    public string? OrderId { get; set; }

    /// <summary>系统跟踪号</summary>
    public string? TraceNo { get; set; }

    /// <summary>交易传输时间</summary>
    public string? TraceTime { get; set; }

    /// <summary>原始键值对</summary>
    public Dictionary<string, string> RawParams { get; set; } = new();
}
