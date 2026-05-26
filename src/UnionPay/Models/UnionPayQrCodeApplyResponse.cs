namespace GaoXinLibrary.PaySDK.UnionPay.Models;

/// <summary>
/// 银联申请二维码（主扫）响应参数
/// </summary>
public class UnionPayQrCodeApplyResponse
{
    /// <summary>应答码：00 成功，其他失败</summary>
    public string RespCode { get; set; } = string.Empty;

    /// <summary>应答信息</summary>
    public string? RespMsg { get; set; }

    /// <summary>商户订单号</summary>
    public string? OrderId { get; set; }

    /// <summary>银联查询流水号</summary>
    public string? QueryId { get; set; }

    /// <summary>二维码字符串（持卡人扫此码完成支付）</summary>
    public string? QrCode { get; set; }

    /// <summary>原始键值对</summary>
    public Dictionary<string, string> RawParams { get; set; } = new();
}
