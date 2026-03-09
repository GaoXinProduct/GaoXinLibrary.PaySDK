namespace GaoXinLibrary.PaySDK.UnionPay.Models;

/// <summary>
/// 银联跨境电商海关申报响应参数
/// <para>同步应答仅为受理结果，申报结果需通过申报查询接口获取</para>
/// </summary>
public class UnionPayCustomsDeclarationResponse
{
    /// <summary>应答码：00 受理成功，其他失败</summary>
    public string RespCode { get; set; } = string.Empty;

    /// <summary>应答信息</summary>
    public string? RespMsg { get; set; }

    /// <summary>商户订单号</summary>
    public string? OrderId { get; set; }

    /// <summary>
    /// 原始交易流水号
    /// <para>txnSubType=00 时，用作报关流水号</para>
    /// </summary>
    public string? OrigQryId { get; set; }

    /// <summary>
    /// 支付单据号
    /// <para>txnSubType=01 时，用作报关流水号</para>
    /// </summary>
    public string? PayTransNo { get; set; }

    /// <summary>原始键值对</summary>
    public Dictionary<string, string> RawParams { get; set; } = new();
}
