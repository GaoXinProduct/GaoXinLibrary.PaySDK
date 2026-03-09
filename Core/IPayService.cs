namespace GaoXinLibrary.PaySDK.Core;

/// <summary>
/// 统一支付服务接口
/// <para>聚合微信支付、支付宝、银联三个渠道，提供统一的支付、查询、退款、回调解析及账单下载能力。</para>
/// </summary>
public interface IPayService
{
    /// <summary>
    /// 创建支付订单
    /// </summary>
    /// <param name="request">创建订单请求</param>
    /// <param name="ct">取消令牌</param>
    /// <returns>创建订单响应</returns>
    Task<CreateOrderResponse> CreateOrderAsync(CreateOrderRequest request, CancellationToken ct = default);

    /// <summary>
    /// 查询支付订单
    /// </summary>
    /// <param name="request">查询订单请求</param>
    /// <param name="ct">取消令牌</param>
    /// <returns>查询订单响应</returns>
    Task<QueryOrderResponse> QueryOrderAsync(QueryOrderRequest request, CancellationToken ct = default);

    /// <summary>
    /// 申请退款
    /// </summary>
    /// <param name="request">退款请求</param>
    /// <param name="ct">取消令牌</param>
    /// <returns>退款响应</returns>
    Task<RefundResponse> RefundAsync(RefundRequest request, CancellationToken ct = default);

    /// <summary>
    /// 查询退款
    /// </summary>
    /// <param name="request">退款查询请求</param>
    /// <param name="ct">取消令牌</param>
    /// <returns>退款查询响应</returns>
    Task<QueryRefundResponse> QueryRefundAsync(QueryRefundRequest request, CancellationToken ct = default);

    /// <summary>
    /// 关闭订单（适用于未支付的订单）
    /// </summary>
    /// <param name="request">关闭订单请求</param>
    /// <param name="ct">取消令牌</param>
    /// <returns>关闭订单响应</returns>
    Task<CloseOrderResponse> CloseOrderAsync(CloseOrderRequest request, CancellationToken ct = default);

    /// <summary>
    /// 下载账单
    /// </summary>
    /// <param name="request">账单下载请求</param>
    /// <param name="ct">取消令牌</param>
    /// <returns>账单内容（CSV 字节流）</returns>
    Task<byte[]> DownloadBillAsync(DownloadBillRequest request, CancellationToken ct = default);

    /// <summary>
    /// 解析并验证支付回调通知
    /// </summary>
    /// <param name="channel">支付渠道</param>
    /// <param name="requestBody">原始请求体字符串</param>
    /// <param name="headers">HTTP 请求头（微信支付验签需要）</param>
    /// <returns>解析后的回调结果</returns>
    Task<PayCallbackResult> ParseCallbackAsync(PayChannel channel, string requestBody, IDictionary<string, string>? headers = null);
}
