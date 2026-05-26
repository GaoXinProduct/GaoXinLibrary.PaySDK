using GaoXinLibrary.PaySDK.Core;
using GaoXinLibrary.PaySDK.Wechat.Models;

namespace GaoXinLibrary.PaySDK.Wechat.Services;

/// <summary>
/// 微信支付服务接口
/// <para>
/// 支持 JSAPI、APP、H5、Native、小程序五种支付方式，以及退款、账单下载、回调解析。
/// </para>
/// </summary>
public interface IWechatPayService
{
    /// <summary>
    /// JSAPI 下单（公众号内网页支付）
    /// </summary>
    Task<WechatJsapiOrderResponse> CreateJsapiOrderAsync(WechatJsapiOrderRequest request, CancellationToken ct = default);

    /// <summary>
    /// APP 下单
    /// </summary>
    Task<WechatAppOrderResponse> CreateAppOrderAsync(WechatAppOrderRequest request, CancellationToken ct = default);

    /// <summary>
    /// H5 下单（在浏览器中拉起微信支付）
    /// </summary>
    Task<WechatH5OrderResponse> CreateH5OrderAsync(WechatH5OrderRequest request, CancellationToken ct = default);

    /// <summary>
    /// Native 下单（生成二维码，用户扫码支付）
    /// </summary>
    Task<WechatNativeOrderResponse> CreateNativeOrderAsync(WechatNativeOrderRequest request, CancellationToken ct = default);

    /// <summary>
    /// 小程序下单
    /// </summary>
    Task<WechatMiniProgramOrderResponse> CreateMiniProgramOrderAsync(WechatMiniProgramOrderRequest request, CancellationToken ct = default);

    /// <summary>
    /// 关闭订单（未支付状态下取消或超时后调用）
    /// <para>POST /v3/pay/transactions/out-trade-no/{out_trade_no}/close</para>
    /// </summary>
    Task CloseOrderAsync(string outTradeNo, CancellationToken ct = default);

    /// <summary>
    /// 通过商户订单号查询订单
    /// </summary>
    Task<WechatQueryOrderResponse> QueryOrderByOutTradeNoAsync(string outTradeNo, CancellationToken ct = default);

    /// <summary>
    /// 通过微信支付订单号查询订单
    /// </summary>
    Task<WechatQueryOrderResponse> QueryOrderByTransactionIdAsync(string transactionId, CancellationToken ct = default);

    /// <summary>
    /// 申请退款
    /// </summary>
    Task<WechatRefundResponse> RefundAsync(WechatRefundRequest request, CancellationToken ct = default);

    /// <summary>
    /// 查询退款
    /// <para>GET /v3/refund/domestic/refunds/{out_refund_no}</para>
    /// <para>https://pay.weixin.qq.com/doc/v3/merchant/4013070374</para>
    /// </summary>
    Task<WechatRefundQueryResponse> QueryRefundAsync(string outRefundNo, CancellationToken ct = default);

    /// <summary>
    /// 下载交易账单（返回 CSV 内容字节）
    /// </summary>
    Task<byte[]> DownloadTradeBillAsync(string billDate, string billType = "ALL", CancellationToken ct = default);

    /// <summary>
    /// 下载资金账单（返回 CSV 内容字节）
    /// </summary>
    Task<byte[]> DownloadFundFlowBillAsync(string billDate, string accountType = "BASIC", CancellationToken ct = default);

    /// <summary>
    /// 解析支付成功回调通知，验签并解密
    /// </summary>
    Task<WechatPayCallbackDecrypted> ParsePayCallbackAsync(
        string requestBody,
        WechatPayCallbackHeaders headers,
        CancellationToken ct = default);

    /// <summary>
    /// 解析退款结果回调通知，验签并解密
    /// <para>https://pay.weixin.qq.com/doc/v3/merchant/4013071196</para>
    /// </summary>
    Task<WechatRefundCallbackDecrypted> ParseRefundCallbackAsync(
        string requestBody,
        WechatPayCallbackHeaders headers,
        CancellationToken ct = default);

    /// <summary>
    /// 手动注册平台证书，供回调验签使用
    /// <para>一般无需直接调用，<c>DownloadCertificatesAsync</c> 会自动注册。</para>
    /// </summary>
    void RegisterCertificate(string serialNo, string certificatePem);

    /// <summary>
    /// 构建 JSAPI / 小程序 前端调起支付参数
    /// </summary>
    WechatJsPayParams BuildJsPayParams(string prepayId);

    /// <summary>
    /// 构建 APP 调起支付参数
    /// </summary>
    WechatAppPayParams BuildAppPayParams(string prepayId);

    /// <summary>
    /// 下载微信支付平台证书列表（GET /v3/certificates）
    /// <para>下载并解密后自动注册到验签缓存，建议每 12 小时刷新一次。</para>
    /// </summary>
    Task<IReadOnlyList<(string SerialNo, string CertificatePem)>> DownloadCertificatesAsync(CancellationToken ct = default);

    /// <summary>
    /// 发起异常退款
    /// <para>POST /v3/refund/domestic/refunds/{refund_id}/apply-abnormal-refund</para>
    /// <para>
    /// 提交退款申请后，退款状态为异常时，可调用此接口发起异常退款处理。
    /// 支持退款至用户银行卡、退款至交易商户银行账户两种处理方式。
    /// </para>
    /// <para>https://pay.weixin.qq.com/doc/v3/merchant/4013071193</para>
    /// </summary>
    Task<WechatAbnormalRefundResponse> ApplyAbnormalRefundAsync(WechatAbnormalRefundRequest request, CancellationToken ct = default);

    /// <summary>
    /// 使用微信支付公钥或平台证书公钥对敏感字段进行 RSAES-OAEP 加密
    /// <para>
    /// 加密使用 RSA/ECB/OAEPWithSHA-1AndMGF1Padding。
    /// 用于发起异常退款等包含银行卡号、用户姓名等敏感信息的接口。
    /// </para>
    /// <para>
    /// • 微信支付公钥模式：<see href="https://pay.weixin.qq.com/doc/v3/merchant/4013053257"/><br/>
    /// • 平台证书模式：<see href="https://pay.weixin.qq.com/doc/v3/merchant/4013053264"/>
    /// </para>
    /// </summary>
    /// <param name="plainText">需要加密的明文字符串</param>
    /// <returns>Base64 编码的加密密文</returns>
    string EncryptSensitiveField(string plainText);

    /// <summary>
    /// 使用商户 API 证书私钥对微信支付下行的敏感字段密文进行 RSAES-OAEP 解密
    /// <para>
    /// 微信支付使用商户 API 证书中的公钥加密下行敏感信息（如银行卡号、用户姓名等），
    /// 开发者应使用商户私钥解密密文获取原文。
    /// </para>
    /// <para>https://pay.weixin.qq.com/doc/v3/merchant/4013053265</para>
    /// </summary>
    /// <param name="cipherText">Base64 编码的加密密文</param>
    /// <returns>解密后的明文字符串</returns>
    string DecryptSensitiveField(string cipherText);

    // ── 合单支付（Combine Payment） ───────────────────────────

    /// <summary>
    /// 合单 JSAPI 下单
    /// <para>POST /v3/combine-transactions/jsapi</para>
    /// <para>https://pay.weixin.qq.com/doc/v3/merchant/4012791856</para>
    /// </summary>
    Task<WechatCombineOrderResponse> CreateCombineJsapiOrderAsync(WechatCombineOrderRequest request, CancellationToken ct = default);

    /// <summary>
    /// 合单 APP 下单
    /// <para>POST /v3/combine-transactions/app</para>
    /// <para>https://pay.weixin.qq.com/doc/v3/merchant/4012791856</para>
    /// </summary>
    Task<WechatCombineOrderResponse> CreateCombineAppOrderAsync(WechatCombineOrderRequest request, CancellationToken ct = default);

    /// <summary>
    /// 合单 H5 下单
    /// <para>POST /v3/combine-transactions/h5</para>
    /// <para>https://pay.weixin.qq.com/doc/v3/merchant/4012791856</para>
    /// </summary>
    Task<WechatCombineOrderResponse> CreateCombineH5OrderAsync(WechatCombineOrderRequest request, CancellationToken ct = default);

    /// <summary>
    /// 合单 Native 下单
    /// <para>POST /v3/combine-transactions/native</para>
    /// <para>https://pay.weixin.qq.com/doc/v3/merchant/4012791856</para>
    /// </summary>
    Task<WechatCombineOrderResponse> CreateCombineNativeOrderAsync(WechatCombineOrderRequest request, CancellationToken ct = default);

    /// <summary>
    /// 合单查询订单
    /// <para>GET /v3/combine-transactions/out-trade-no/{combine_out_trade_no}</para>
    /// <para>https://pay.weixin.qq.com/doc/v3/merchant/4012791857</para>
    /// </summary>
    Task<WechatCombineQueryResponse> QueryCombineOrderAsync(string combineOutTradeNo, CancellationToken ct = default);

    /// <summary>
    /// 合单关闭订单
    /// <para>POST /v3/combine-transactions/out-trade-no/{combine_out_trade_no}/close</para>
    /// <para>https://pay.weixin.qq.com/doc/v3/merchant/4012791863</para>
    /// </summary>
    Task CloseCombineOrderAsync(string combineOutTradeNo, WechatCombineCloseRequest request, CancellationToken ct = default);

    // ── 分账（Profit Sharing） ────────────────────────────────

    /// <summary>
    /// 请求分账
    /// <para>POST /v3/profitsharing/orders</para>
    /// <para>https://pay.weixin.qq.com/doc/v3/merchant/4012791906</para>
    /// </summary>
    Task<WechatProfitSharingResponse> CreateProfitSharingAsync(WechatProfitSharingRequest request, CancellationToken ct = default);

    /// <summary>
    /// 查询分账结果
    /// <para>GET /v3/profitsharing/orders/{out_order_no}?transaction_id={transaction_id}</para>
    /// <para>https://pay.weixin.qq.com/doc/v3/merchant/4012791908</para>
    /// </summary>
    Task<WechatProfitSharingQueryResponse> QueryProfitSharingAsync(string outOrderNo, string transactionId, CancellationToken ct = default);

    /// <summary>
    /// 请求分账回退
    /// <para>POST /v3/profitsharing/return-orders</para>
    /// <para>https://pay.weixin.qq.com/doc/v3/merchant/4012791911</para>
    /// </summary>
    Task<WechatProfitSharingReturnResponse> ReturnProfitSharingAsync(WechatProfitSharingReturnRequest request, CancellationToken ct = default);

    /// <summary>
    /// 查询分账回退结果
    /// <para>GET /v3/profitsharing/return-orders/{out_return_no}?out_order_no={out_order_no}</para>
    /// <para>https://pay.weixin.qq.com/doc/v3/merchant/4012791912</para>
    /// </summary>
    Task<WechatProfitSharingReturnQueryResponse> QueryProfitSharingReturnAsync(string outReturnNo, string outOrderNo, CancellationToken ct = default);

    // ── 商家转账到零钱（Transfer） ─────────────────────────────

    /// <summary>
    /// 商家转账到零钱
    /// <para>POST /v3/transfer/bills</para>
    /// <para>https://pay.weixin.qq.com/doc/v3/merchant/4012716434</para>
    /// </summary>
    Task<WechatTransferBillsResponse> TransferBillsAsync(WechatTransferBillsRequest request, CancellationToken ct = default);

    /// <summary>
    /// 查询商家转账到零钱
    /// <para>GET /v3/transfer/bills/out-bill-no/{out_bill_no}</para>
    /// <para>https://pay.weixin.qq.com/doc/v3/merchant/4012716436</para>
    /// </summary>
    Task<WechatTransferBillQueryResponse> QueryTransferBillAsync(string outBillNo, CancellationToken ct = default);
}
