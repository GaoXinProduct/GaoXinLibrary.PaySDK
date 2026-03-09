using GaoXinLibrary.PaySDK.Alipay.Models;
using Microsoft.Extensions.Primitives;

namespace GaoXinLibrary.PaySDK.Alipay.Services;

/// <summary>
/// 支付宝支付服务接口
/// <para>
/// 支持当面付、订单码支付、App 支付、手机网站支付、电脑网站支付，
/// 以及退款、账单下载、回调解析。
/// </para>
/// </summary>
public interface IAlipayService
{
    /// <summary>
    /// 当面付（扫用户付款码）
    /// <para>alipay.trade.pay</para>
    /// </summary>
    Task<AlipayTradePayResponse> FaceToFacePayAsync(AlipayTradePayBizContent bizContent, CancellationToken ct = default);

    /// <summary>
    /// JSAPI / 小程序支付（统一收单交易创建，返回 trade_no，前端调用 JS SDK 唤起支付）
    /// <para>alipay.trade.create</para>
    /// </summary>
    /// <param name="notifyUrl">异步通知地址，为 null 时使用 <see cref="Core.AlipayOptions.NotifyUrl"/></param>
    Task<AlipayTradeCreateResponse> CreateOrderAsync(AlipayTradeCreateContent bizContent, string? notifyUrl = null, CancellationToken ct = default);

    /// <summary>
    /// 订单码支付（预下单，生成商家二维码让用户扫码）
    /// <para>alipay.trade.precreate</para>
    /// </summary>
    /// <param name="notifyUrl">异步通知地址，为 null 时使用 <see cref="Core.AlipayOptions.NotifyUrl"/></param>
    Task<AlipayTradePrecreateResponse> PrecreateAsync(AlipayTradePrecreateContent bizContent, string? notifyUrl = null, CancellationToken ct = default);

    /// <summary>
    /// App 支付（返回 SDK 签名字符串，给客户端 SDK 调起支付）
    /// <para>alipay.trade.app.pay</para>
    /// </summary>
    /// <param name="notifyUrl">异步通知地址，为 null 时使用 <see cref="Core.AlipayOptions.NotifyUrl"/></param>
    string BuildAppPayString(AlipayTradeAppPayContent bizContent, string? notifyUrl = null);

    /// <summary>
    /// 手机网站支付（返回 HTTP 302 跳转 URL，引导用户在浏览器中完成支付）
    /// <para>alipay.trade.wap.pay</para>
    /// </summary>
    /// <param name="notifyUrl">异步通知地址，为 null 时使用 <see cref="Core.AlipayOptions.NotifyUrl"/></param>
    /// <param name="returnUrl">同步跳转地址，为 null 时使用 <see cref="Core.AlipayOptions.ReturnUrl"/></param>
    string BuildWapPayUrl(AlipayTradeWapPayContent bizContent, string? notifyUrl = null, string? returnUrl = null);

    /// <summary>
    /// 电脑网站支付（返回表单 GET URL，前端直接跳转）
    /// <para>alipay.trade.page.pay</para>
    /// </summary>
    /// <param name="notifyUrl">异步通知地址，为 null 时使用 <see cref="Core.AlipayOptions.NotifyUrl"/></param>
    /// <param name="returnUrl">同步跳转地址，为 null 时使用 <see cref="Core.AlipayOptions.ReturnUrl"/></param>
    string BuildPagePayUrl(AlipayTradePagePayContent bizContent, string? notifyUrl = null, string? returnUrl = null);

    /// <summary>
    /// 撤销订单（当面付场景专用，可撤销未付款或已付款订单）
    /// <para>alipay.trade.cancel</para>
    /// <para>与 close 的区别：cancel 可撤销已付款交易（触发自动退款），close 仅适用于未付款交易。</para>
    /// <para>若返回 RetryFlag = "Y"，表示系统异常，调用方应重试直到 RetryFlag = "N" 为止。</para>
    /// </summary>
    Task<AlipayTradeCancelResponse> CancelOrderAsync(AlipayTradeCancelContent bizContent, CancellationToken ct = default);

    /// <summary>
    /// 关闭订单（未支付状态下取消或超时后调用）
    /// <para>alipay.trade.close</para>
    /// </summary>
    /// <param name="ignoreNotExist">
    /// 为 <c>true</c> 时：若支付宝返回 <c>ACQ.TRADE_NOT_EXIST</c>，视为幂等关闭成功（不抛异常）。<br/>
    /// 适用于 <b>App / 手机网站 / 电脑网站支付</b>：这三种方式仅在本地生成支付链接，
    /// 用户若未跳转到支付宝，支付宝侧根本没有创建交易记录，关闭时返回"不存在"是预期行为。<br/>
    /// 为 <c>false</c>（默认）时：交易不存在将抛出 <see cref="Core.AlipayException"/>，
    /// 适用于 <b>当面付 / 订单码（precreate）</b> 等调用 API 即立即创建交易的场景。
    /// </param>
    Task<AlipayTradeCloseResponse> CloseOrderAsync(AlipayTradeCloseContent bizContent, bool ignoreNotExist = false, CancellationToken ct = default);

    /// <summary>
    /// 查询订单
    /// <para>alipay.trade.query</para>
    /// </summary>
    Task<AlipayTradeQueryResponse> QueryOrderAsync(AlipayTradeQueryContent bizContent, CancellationToken ct = default);

    /// <summary>
    /// 申请退款
    /// <para>alipay.trade.refund</para>
    /// </summary>
    Task<AlipayTradeRefundResponse> RefundAsync(AlipayTradeRefundContent bizContent, CancellationToken ct = default);

    /// <summary>
    /// 查询退款（通过商户退款请求号精确查询单笔退款状态）
    /// <para>alipay.trade.fastpay.refund.query</para>
    /// </summary>
    Task<AlipayTradeRefundQueryResponse> QueryRefundAsync(AlipayTradeRefundQueryContent bizContent, CancellationToken ct = default);

    /// <summary>
    /// 获取账单下载 URL，然后下载并返回 CSV 字节
    /// <para>alipay.data.dataservice.bill.downloadurl.query</para>
    /// </summary>
    Task<byte[]> DownloadBillAsync(AlipayBillDownloadContent bizContent, CancellationToken ct = default);

    /// <summary>
    /// 解析并验签支付宝异步通知（Form 表单形式）
    /// </summary>
    /// <param name="formParams">从请求中读取的表单键值对</param>
    /// <returns>解析后的回调参数，IsValid 表示验签结果</returns>
    AlipayCallbackParams ParseCallback(IDictionary<string, string> formParams);

    /// <summary>
    /// 解析并验签支付宝异步通知（直接传入 <see cref="IFormCollection"/> 或任意
    /// <see cref="IEnumerable{T}"/> of <see cref="KeyValuePair{TKey,TValue}"/> 形式的表单数据）
    /// </summary>
    /// <param name="formParams">ASP.NET Core <c>Request.Form</c> 或等价键值序列</param>
    AlipayCallbackParams ParseCallback(IEnumerable<KeyValuePair<string, StringValues>> formParams);
}
