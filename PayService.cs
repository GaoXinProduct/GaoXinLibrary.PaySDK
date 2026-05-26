using System.Globalization;
using System.Text.Json;
using GaoXinLibrary.PaySDK.Alipay.Models;
using GaoXinLibrary.PaySDK.Alipay.Services;
using GaoXinLibrary.PaySDK.Core;
using GaoXinLibrary.PaySDK.UnionPay.Models;
using GaoXinLibrary.PaySDK.UnionPay.Services;
using GaoXinLibrary.PaySDK.Wechat.Models;
using GaoXinLibrary.PaySDK.Wechat.Services;
using Microsoft.Extensions.Logging;

namespace GaoXinLibrary.PaySDK;

/// <summary>
/// 统一支付服务实现
/// <para>聚合微信支付、支付宝、银联，根据渠道自动路由</para>
/// </summary>
public sealed partial class PayService : IPayService
{
    private readonly IWechatPayService? _wechat;
    private readonly IAlipayService? _alipay;
    private readonly IUnionPayService? _unionPay;
    private readonly ILogger<PayService>? _logger;

    /// <summary>
    /// 初始化统一支付服务
    /// </summary>
    public PayService(
        IWechatPayService? wechat = null,
        IAlipayService? alipay = null,
        IUnionPayService? unionPay = null,
        ILogger<PayService>? logger = null)
    {
        _wechat = wechat;
        _alipay = alipay;
        _unionPay = unionPay;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<CreateOrderResponse> CreateOrderAsync(CreateOrderRequest request, CancellationToken ct = default)
    {
        try
        {
            var response = request.Channel switch
            {
                _ when request.Channel.IsWechat() => await CreateWechatOrderAsync(request, ct),
                _ when request.Channel.IsAlipay() => await CreateAlipayOrderAsync(request, ct),
                _ when request.Channel.IsUnionPay() => await CreateUnionPayOrderAsync(request, ct),
                _ => throw new PayException("UNSUPPORTED_CHANNEL", $"不支持的支付渠道：{request.Channel}")
            };
            _logger?.LogInformation("PayCreateOrder success channel={Channel} outTradeNo={OutTradeNo}", request.Channel, request.OutTradeNo);
            return response;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "PayCreateOrder failed channel={Channel} outTradeNo={OutTradeNo}", request.Channel, request.OutTradeNo);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<QueryOrderResponse> QueryOrderAsync(QueryOrderRequest request, CancellationToken ct = default)
    {
        try
        {
            var response = request.Channel switch
            {
                _ when request.Channel.IsWechat() => await QueryWechatOrderAsync(request, ct),
                _ when request.Channel.IsAlipay() => await QueryAlipayOrderAsync(request, ct),
                _ when request.Channel.IsUnionPay() => await QueryUnionPayOrderAsync(request, ct),
                _ => throw new PayException("UNSUPPORTED_CHANNEL", $"不支持的支付渠道：{request.Channel}")
            };
            _logger?.LogInformation("PayQueryOrder success channel={Channel} outTradeNo={OutTradeNo} tradeStatus={TradeStatus}", request.Channel, response.OutTradeNo, response.TradeStatus);
            return response;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "PayQueryOrder failed channel={Channel} outTradeNo={OutTradeNo} transactionId={TransactionId}", request.Channel, request.OutTradeNo, request.TransactionId);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<RefundResponse> RefundAsync(RefundRequest request, CancellationToken ct = default)
    {
        try
        {
            var response = request.Channel switch
            {
                _ when request.Channel.IsWechat() => await WechatRefundAsync(request, ct),
                _ when request.Channel.IsAlipay() => await AlipayRefundAsync(request, ct),
                _ when request.Channel.IsUnionPay() => await UnionPayRefundAsync(request, ct),
                _ => throw new PayException("UNSUPPORTED_CHANNEL", $"不支持的支付渠道：{request.Channel}")
            };
            _logger?.LogInformation("PayRefund success channel={Channel} outTradeNo={OutTradeNo} outRefundNo={OutRefundNo} status={RefundStatus}", request.Channel, request.OutTradeNo, response.OutRefundNo, response.RefundStatus);
            return response;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "PayRefund failed channel={Channel} outTradeNo={OutTradeNo} outRefundNo={OutRefundNo}", request.Channel, request.OutTradeNo, request.OutRefundNo);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<QueryRefundResponse> QueryRefundAsync(QueryRefundRequest request, CancellationToken ct = default)
    {
        try
        {
            var response = request.Channel switch
            {
                _ when request.Channel.IsWechat() => await QueryWechatRefundAsync(request, ct),
                _ when request.Channel.IsAlipay() => await QueryAlipayRefundAsync(request, ct),
                _ when request.Channel.IsUnionPay() => await QueryUnionPayRefundAsync(request, ct),
                _ => throw new PayException("UNSUPPORTED_CHANNEL", $"渠道 {request.Channel} 暂不支持退款查询")
            };
            _logger?.LogInformation("PayQueryRefund success channel={Channel} outRefundNo={OutRefundNo} status={RefundStatus}", request.Channel, response.OutRefundNo, response.RefundStatus);
            return response;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "PayQueryRefund failed channel={Channel} outRefundNo={OutRefundNo}", request.Channel, request.OutRefundNo);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<CloseOrderResponse> CloseOrderAsync(CloseOrderRequest request, CancellationToken ct = default)
    {
        try
        {
            var response = request.Channel switch
            {
                _ when request.Channel.IsWechat() => await CloseWechatOrderAsync(request, ct),
                _ when request.Channel.IsAlipay() => await CloseAlipayOrderAsync(request, ct),
                _ when request.Channel.IsUnionPay() => CloseUnionPayOrder(request),
                _ => throw new PayException("UNSUPPORTED_CHANNEL", $"渠道 {request.Channel} 暂不支持关闭订单")
            };
            _logger?.LogInformation("PayCloseOrder success channel={Channel} outTradeNo={OutTradeNo} operationMode={OperationMode}", request.Channel, response.OutTradeNo, response.OperationMode);
            return response;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "PayCloseOrder failed channel={Channel} outTradeNo={OutTradeNo}", request.Channel, request.OutTradeNo);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<byte[]> DownloadBillAsync(DownloadBillRequest request, CancellationToken ct = default)
    {
        try
        {
            var response = request.Channel switch
            {
                _ when request.Channel.IsWechat() => await DownloadWechatBillAsync(request, ct),
                _ when request.Channel.IsAlipay() => await DownloadAlipayBillAsync(request, ct),
                _ when request.Channel.IsUnionPay() => await DownloadUnionPayBillAsync(request, ct),
                _ => throw new PayException("UNSUPPORTED_CHANNEL", $"渠道 {request.Channel} 暂不支持账单下载")
            };
            _logger?.LogInformation("PayDownloadBill success channel={Channel} billDate={BillDate} billType={BillType} size={Size}", request.Channel, request.BillDate, request.BillType, response.Length);
            return response;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "PayDownloadBill failed channel={Channel} billDate={BillDate} billType={BillType}", request.Channel, request.BillDate, request.BillType);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<PayCallbackResult> ParseCallbackAsync(
        PayChannel channel,
        string requestBody,
        IDictionary<string, string>? headers = null,
        CancellationToken ct = default)
    {
        try
        {
            var response = channel switch
            {
                _ when channel.IsWechat() => await ParseWechatCallbackAsync(channel, requestBody, headers, ct),
                _ when channel.IsAlipay() => ParseAlipayCallback(channel, requestBody),
                _ when channel.IsUnionPay() => ParseUnionPayCallback(requestBody),
                _ => throw new PayException("UNSUPPORTED_CHANNEL", $"不支持的支付渠道：{channel}")
            };
            _logger?.LogInformation("PayParseCallback processed channel={Channel} isValid={IsValid} outTradeNo={OutTradeNo} tradeStatus={TradeStatus}", channel, response.IsValid, response.OutTradeNo, response.TradeStatus);
            return response;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "PayParseCallback failed channel={Channel}", channel);
            throw;
        }
    }

    // ─── 辅助 ──────────────────────────────────────────────────────────────────

    private static Dictionary<string, string> ParseFormString(string body)
    {
        var result = new Dictionary<string, string>(StringComparer.Ordinal);
        if (string.IsNullOrWhiteSpace(body)) return result;
        foreach (var pair in body.Split('&'))
        {
            var idx = pair.IndexOf('=');
            if (idx < 0) continue;
            var key = System.Web.HttpUtility.UrlDecode(pair[..idx]);
            var val = System.Web.HttpUtility.UrlDecode(pair[(idx + 1)..]);
            result[key] = val;
        }
        return result;
    }
}
