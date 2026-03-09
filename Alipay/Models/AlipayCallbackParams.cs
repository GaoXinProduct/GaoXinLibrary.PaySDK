namespace GaoXinLibrary.PaySDK.Alipay.Models;

/// <summary>
/// 支付宝回调通知参数（Form 表单键值对）
/// <para>
/// 支付宝通过 HTTP POST 将参数以 application/x-www-form-urlencoded 形式发送，
/// 解析后存入此对象进行验签和业务处理。
/// </para>
/// </summary>
public class AlipayCallbackParams
{
    /// <summary>通知时间，格式为 yyyy-MM-dd HH:mm:ss</summary>
    public string NotifyTime { get; set; } = string.Empty;

    /// <summary>通知类型，固定值 trade_status_sync</summary>
    public string NotifyType { get; set; } = string.Empty;

    /// <summary>通知校验 ID</summary>
    public string NotifyId { get; set; } = string.Empty;

    /// <summary>支付宝分配给开发者的应用 ID</summary>
    public string AppId { get; set; } = string.Empty;

    /// <summary>编码格式</summary>
    public string Charset { get; set; } = "utf-8";

    /// <summary>接口版本</summary>
    public string Version { get; set; } = "1.0";

    /// <summary>签名类型</summary>
    public string SignType { get; set; } = "RSA2";

    /// <summary>签名</summary>
    public string Sign { get; set; } = string.Empty;

    /// <summary>签名验证是否通过</summary>
    public bool IsValid { get; set; }

    /// <summary>支付宝交易号</summary>
    public string TradeNo { get; set; } = string.Empty;

    /// <summary>商户订单号</summary>
    public string OutTradeNo { get; set; } = string.Empty;

    /// <summary>商户业务号（退款时存在）</summary>
    public string? OutBizNo { get; set; }

    /// <summary>买家支付宝账号</summary>
    public string? BuyerLogonId { get; set; }

    /// <summary>买家用户 id</summary>
    public string? BuyerUserId { get; set; }

    /// <summary>
    /// 交易状态：WAIT_BUYER_PAY / TRADE_CLOSED / TRADE_SUCCESS / TRADE_FINISHED
    /// </summary>
    public string TradeStatus { get; set; } = string.Empty;

    /// <summary>订单金额，单位为元，两位小数</summary>
    public string TotalAmount { get; set; } = string.Empty;

    /// <summary>实付款金额</summary>
    public string? BuyerPayAmount { get; set; }

    /// <summary>交易创建时间</summary>
    public string? GmtCreate { get; set; }

    /// <summary>交易付款时间</summary>
    public string? GmtPayment { get; set; }

    /// <summary>交易退款时间</summary>
    public string? GmtRefund { get; set; }

    /// <summary>交易结束时间</summary>
    public string? GmtClose { get; set; }

    /// <summary>原始键值对（用于验签）</summary>
    public Dictionary<string, string> RawParams { get; set; } = new();
}
