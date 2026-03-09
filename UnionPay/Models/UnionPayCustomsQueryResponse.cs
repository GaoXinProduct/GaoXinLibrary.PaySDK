namespace GaoXinLibrary.PaySDK.UnionPay.Models;

/// <summary>
/// 银联跨境电商海关申报查询响应参数
/// </summary>
public class UnionPayCustomsQueryResponse
{
    /// <summary>应答码：00 成功，其他失败</summary>
    public string RespCode { get; set; } = string.Empty;

    /// <summary>应答信息</summary>
    public string? RespMsg { get; set; }

    /// <summary>商户订单号</summary>
    public string? OrderId { get; set; }

    /// <summary>
    /// 原始应答码（申报结果）
    /// <para>00 = 申报成功</para>
    /// </summary>
    public string? OrigRespCode { get; set; }

    /// <summary>原始应答信息</summary>
    public string? OrigRespMsg { get; set; }

    /// <summary>原始键值对</summary>
    public Dictionary<string, string> RawParams { get; set; } = new();
}
