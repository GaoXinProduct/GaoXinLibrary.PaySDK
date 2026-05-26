namespace GaoXinLibrary.PaySDK.UnionPay.Models;

/// <summary>
/// 银联前台消费响应（构建 HTML 表单供前端自动提交）
/// </summary>
public class UnionPayFrontPayResponse
{
    /// <summary>自动提交 HTML 表单（前端直接 innerHTML 后立即跳转至银联支付页）</summary>
    public string FormHtml { get; set; } = string.Empty;

    /// <summary>银联交易流水号（前台交易无此字段，仅供记录）</summary>
    public string? QueryId { get; set; }
}
