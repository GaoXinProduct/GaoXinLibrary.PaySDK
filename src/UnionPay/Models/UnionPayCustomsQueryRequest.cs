namespace GaoXinLibrary.PaySDK.UnionPay.Models;

/// <summary>
/// 银联跨境电商海关申报查询请求参数
/// </summary>
public class UnionPayCustomsQueryRequest
{
    /// <summary>交易类型，固定 00（查询）</summary>
    public string TxnType { get; set; } = "00";

    /// <summary>交易子类，固定 00</summary>
    public string TxnSubType { get; set; } = "00";

    /// <summary>产品类型，固定 000000</summary>
    public string BizType { get; set; } = "000000";

    /// <summary>商户订单号（与申报时保持一致）</summary>
    public string OrderId { get; set; } = string.Empty;

    /// <summary>交易时间（与申报时保持一致），格式 yyyyMMddHHmmss</summary>
    public string TxnTime { get; set; } = string.Empty;
}
