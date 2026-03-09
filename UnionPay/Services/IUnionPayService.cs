using GaoXinLibrary.PaySDK.UnionPay.Models;

namespace GaoXinLibrary.PaySDK.UnionPay.Services;

/// <summary>
/// 银联支付服务接口
/// <para>
/// 支持在线网关支付、WAP支付、无跳转支付、二维码支付、签约支付、云闪付，
/// 以及订单查询、退款、回调验签、对账文件下载。
/// </para>
/// </summary>
public interface IUnionPayService
{
    /// <summary>
    /// 生成前台消费（在线网关支付）的 HTML 自动提交表单
    /// <para>将 FormHtml 注入页面，浏览器自动 POST 到银联支付页（channelType=07）</para>
    /// </summary>
    UnionPayFrontPayResponse CreateFrontPay(UnionPayFrontPayRequest request);

    /// <summary>
    /// 生成 WAP 前台消费（手机网页支付）的 HTML 自动提交表单
    /// <para>channelType=08，移动端浏览器自动跳转银联 WAP 支付页</para>
    /// </summary>
    UnionPayFrontPayResponse CreateWapPay(UnionPayWapPayRequest request);

    /// <summary>
    /// 后台消费交易（无跳转支付 / 签约支付 / 云闪付）
    /// <para>直接发起后台扣款，无须用户跳转</para>
    /// </summary>
    Task<UnionPayBackPayResponse> CreateBackPayAsync(UnionPayBackPayRequest request, CancellationToken ct = default);

    /// <summary>
    /// 申请二维码（主扫：商户生成二维码，用户扫码支付）
    /// </summary>
    Task<UnionPayQrCodeApplyResponse> ApplyQrCodeAsync(UnionPayQrCodeApplyRequest request, CancellationToken ct = default);

    /// <summary>
    /// 二维码消费（被扫：商户扫用户付款码，后台扣款）
    /// </summary>
    Task<UnionPayBackPayResponse> QrCodeConsumeAsync(UnionPayQrCodeConsumeRequest request, CancellationToken ct = default);

    /// <summary>
    /// 查询订单
    /// </summary>
    Task<UnionPayQueryResponse> QueryOrderAsync(UnionPayQueryRequest request, CancellationToken ct = default);

    /// <summary>
    /// 申请退款（退货）
    /// </summary>
    Task<UnionPayRefundResponse> RefundAsync(UnionPayRefundRequest request, CancellationToken ct = default);

    /// <summary>
    /// 下载对账文件
    /// </summary>
    Task<byte[]> DownloadBillAsync(string settleDate, string fileType, CancellationToken ct = default);

    /// <summary>
    /// 解析并验签银联后台通知（Form 表单键值对）
    /// </summary>
    UnionPayCallbackParams ParseCallback(IDictionary<string, string> formParams);
}
