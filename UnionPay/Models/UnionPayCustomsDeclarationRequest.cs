namespace GaoXinLibrary.PaySDK.UnionPay.Models;

/// <summary>
/// 银联跨境电商海关申报请求参数
/// <para>
/// 用于商户提交银联支付订单进行报关申请。
/// txnSubType=00 时不支持拆单申报；txnSubType=01 时支持拆单及重推。
/// </para>
/// </summary>
public class UnionPayCustomsDeclarationRequest
{
    /// <summary>交易类型，固定 69（海关申报）</summary>
    public string TxnType { get; set; } = "69";

    /// <summary>
    /// 交易子类
    /// <para>00 = 不支持拆单/重推；01 = 支持拆单及重推</para>
    /// </summary>
    public string TxnSubType { get; set; } = "01";

    /// <summary>产品类型，固定 000000</summary>
    public string BizType { get; set; } = "000000";

    /// <summary>商户订单号</summary>
    public string OrderId { get; set; } = string.Empty;

    /// <summary>交易时间，格式 yyyyMMddHHmmss</summary>
    public string TxnTime { get; set; } = string.Empty;

    /// <summary>交易金额（分）</summary>
    public string TxnAmt { get; set; } = string.Empty;

    /// <summary>
    /// 原始交易流水号（origQryId）
    /// <para>对应原始支付交易的 queryId</para>
    /// </summary>
    public string OrigQueryId { get; set; } = string.Empty;

    /// <summary>
    /// 海关代码
    /// <para>如 GUANGZHOU（广州海关）、HANGZHOU（杭州海关）等</para>
    /// </summary>
    public string? CustomsCode { get; set; }

    /// <summary>
    /// 商户在海关备案的名称
    /// </summary>
    public string? MerAbbr { get; set; }

    /// <summary>
    /// 商户在海关备案的编号
    /// </summary>
    public string? MerCatCode { get; set; }

    /// <summary>
    /// 请求方保留域（透传字段）
    /// </summary>
    public string? ReqReserved { get; set; }
}
