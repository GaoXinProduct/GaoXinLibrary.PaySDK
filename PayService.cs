using System.Globalization;
using System.Text.Json;
using GaoXinLibrary.PaySDK.Alipay.Models;
using GaoXinLibrary.PaySDK.Alipay.Services;
using GaoXinLibrary.PaySDK.Core;
using GaoXinLibrary.PaySDK.UnionPay.Models;
using GaoXinLibrary.PaySDK.UnionPay.Services;
using GaoXinLibrary.PaySDK.Wechat.Models;
using GaoXinLibrary.PaySDK.Wechat.Services;

namespace GaoXinLibrary.PaySDK;

/// <summary>
/// 统一支付服务实现
/// <para>聚合微信支付、支付宝、银联，根据渠道自动路由</para>
/// </summary>
public sealed class PayService : IPayService
{
    private readonly IWechatPayService? _wechat;
    private readonly IAlipayService? _alipay;
    private readonly IUnionPayService? _unionPay;

    /// <summary>
    /// 初始化统一支付服务
    /// </summary>
    public PayService(
        IWechatPayService? wechat = null,
        IAlipayService? alipay = null,
        IUnionPayService? unionPay = null)
    {
        _wechat = wechat;
        _alipay = alipay;
        _unionPay = unionPay;
    }

    /// <inheritdoc/>
    public async Task<CreateOrderResponse> CreateOrderAsync(CreateOrderRequest request, CancellationToken ct = default)
    {
        if (request.Channel.IsWechat())   return await CreateWechatOrderAsync(request, ct);
        if (request.Channel.IsAlipay())   return await CreateAlipayOrderAsync(request, ct);
        if (request.Channel.IsUnionPay()) return await CreateUnionPayOrderAsync(request, ct);
        throw new PayException("UNSUPPORTED_CHANNEL", $"不支持的支付渠道：{request.Channel}");
    }

    /// <inheritdoc/>
    public async Task<QueryOrderResponse> QueryOrderAsync(QueryOrderRequest request, CancellationToken ct = default)
    {
        if (request.Channel.IsWechat())   return await QueryWechatOrderAsync(request, ct);
        if (request.Channel.IsAlipay())   return await QueryAlipayOrderAsync(request, ct);
        if (request.Channel.IsUnionPay()) return await QueryUnionPayOrderAsync(request, ct);
        throw new PayException("UNSUPPORTED_CHANNEL", $"不支持的支付渠道：{request.Channel}");
    }

    /// <inheritdoc/>
    public async Task<RefundResponse> RefundAsync(RefundRequest request, CancellationToken ct = default)
    {
        if (request.Channel.IsWechat())   return await WechatRefundAsync(request, ct);
        if (request.Channel.IsAlipay())   return await AlipayRefundAsync(request, ct);
        if (request.Channel.IsUnionPay()) return await UnionPayRefundAsync(request, ct);
        throw new PayException("UNSUPPORTED_CHANNEL", $"不支持的支付渠道：{request.Channel}");
    }

    /// <inheritdoc/>
    public async Task<QueryRefundResponse> QueryRefundAsync(QueryRefundRequest request, CancellationToken ct = default)
    {
        if (request.Channel.IsWechat())   return await QueryWechatRefundAsync(request, ct);
        if (request.Channel.IsAlipay())   return await QueryAlipayRefundAsync(request, ct);
        if (request.Channel.IsUnionPay()) return await QueryUnionPayRefundAsync(request, ct);
        throw new PayException("UNSUPPORTED_CHANNEL", $"渠道 {request.Channel} 暂不支持退款查询");
    }

    /// <inheritdoc/>
    public async Task<CloseOrderResponse> CloseOrderAsync(CloseOrderRequest request, CancellationToken ct = default)
    {
        if (request.Channel.IsWechat())   return await CloseWechatOrderAsync(request, ct);
        if (request.Channel.IsAlipay())   return await CloseAlipayOrderAsync(request, ct);
        if (request.Channel.IsUnionPay()) return CloseUnionPayOrder(request);
        throw new PayException("UNSUPPORTED_CHANNEL", $"渠道 {request.Channel} 暂不支持关闭订单");
    }

    /// <inheritdoc/>
    public async Task<byte[]> DownloadBillAsync(DownloadBillRequest request, CancellationToken ct = default)
    {
        if (request.Channel.IsWechat())   return await DownloadWechatBillAsync(request, ct);
        if (request.Channel.IsAlipay())   return await DownloadAlipayBillAsync(request, ct);
        if (request.Channel.IsUnionPay()) return await DownloadUnionPayBillAsync(request, ct);
        throw new PayException("UNSUPPORTED_CHANNEL", $"渠道 {request.Channel} 暂不支持账单下载");
    }

    /// <inheritdoc/>
    public async Task<PayCallbackResult> ParseCallbackAsync(PayChannel channel, string requestBody, IDictionary<string, string>? headers = null)
    {
        if (channel.IsWechat())   return await ParseWechatCallbackAsync(channel, requestBody, headers);
        if (channel.IsAlipay())   return ParseAlipayCallback(channel, requestBody);
        if (channel.IsUnionPay()) return ParseUnionPayCallback(requestBody);
        throw new PayException("UNSUPPORTED_CHANNEL", $"不支持的支付渠道：{channel}");
    }

    // ─── 微信支付 ─────────────────────────────────────────────────────────────

    private async Task<CreateOrderResponse> CreateWechatOrderAsync(CreateOrderRequest req, CancellationToken ct)
    {
        EnsureWechat();
        switch (req.Channel.ToWechatTradeType())
        {
            case "JSAPI":
            {
                var orderReq = new WechatJsapiOrderRequest
                {
                    OutTradeNo = req.OutTradeNo,
                    Description = req.Subject,
                    NotifyUrl = req.NotifyUrl,
                    Amount = new WechatPayAmount { Total = req.TotalFee, Currency = req.Currency },
                    Payer = new WechatPayPayer { OpenId = req.OpenId ?? string.Empty },
                    TimeExpire = req.ExpireTime?.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                    Attach = req.Attach
                };
                var resp = await _wechat!.CreateJsapiOrderAsync(orderReq, ct);
                var jsParams = _wechat.BuildJsPayParams(resp.PrepayId);
                return new CreateOrderResponse
                {
                    Channel = req.Channel,
                    OutTradeNo = req.OutTradeNo,
                    PrepayId = resp.PrepayId,
                    JsPayParams = jsParams
                };
            }
            case "MINIPROGRAM":
            {
                var orderReq = new WechatMiniProgramOrderRequest
                {
                    OutTradeNo = req.OutTradeNo,
                    Description = req.Subject,
                    NotifyUrl = req.NotifyUrl,
                    Amount = new WechatPayAmount { Total = req.TotalFee, Currency = req.Currency },
                    Payer = new WechatPayPayer { OpenId = req.OpenId ?? string.Empty },
                    TimeExpire = req.ExpireTime?.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                    Attach = req.Attach
                };
                var resp = await _wechat!.CreateMiniProgramOrderAsync(orderReq, ct);
                var jsParams = _wechat.BuildJsPayParams(resp.PrepayId);
                return new CreateOrderResponse
                {
                    Channel = req.Channel,
                    OutTradeNo = req.OutTradeNo,
                    PrepayId = resp.PrepayId,
                    JsPayParams = jsParams
                };
            }
            case "APP":
            {
                var orderReq = new WechatAppOrderRequest
                {
                    OutTradeNo = req.OutTradeNo,
                    Description = req.Subject,
                    NotifyUrl = req.NotifyUrl,
                    Amount = new WechatPayAmount { Total = req.TotalFee, Currency = req.Currency },
                    TimeExpire = req.ExpireTime?.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                    Attach = req.Attach
                };
                var resp = await _wechat!.CreateAppOrderAsync(orderReq, ct);
                var appParams = _wechat.BuildAppPayParams(resp.PrepayId);
                return new CreateOrderResponse
                {
                    Channel = req.Channel,
                    OutTradeNo = req.OutTradeNo,
                    PrepayId = resp.PrepayId,
                    SdkOrderString = JsonSerializer.Serialize(appParams)
                };
            }
            case "H5":
            {
                var orderReq = new WechatH5OrderRequest
                {
                    OutTradeNo = req.OutTradeNo,
                    Description = req.Subject,
                    NotifyUrl = req.NotifyUrl,
                    Amount = new WechatPayAmount { Total = req.TotalFee, Currency = req.Currency },
                    SceneInfo =
                    {
                        PayerClientIp = req.ClientIp ?? "127.0.0.1",
                        H5Info = { Type = req.SceneType ?? "Wap" }
                    },
                    TimeExpire = req.ExpireTime?.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                    Attach = req.Attach
                };
                var resp = await _wechat!.CreateH5OrderAsync(orderReq, ct);
                return new CreateOrderResponse
                {
                    Channel = req.Channel,
                    OutTradeNo = req.OutTradeNo,
                    PayUrl = resp.H5Url
                };
            }
            default: // NATIVE
            {
                var orderReq = new WechatNativeOrderRequest
                {
                    OutTradeNo = req.OutTradeNo,
                    Description = req.Subject,
                    NotifyUrl = req.NotifyUrl,
                    Amount = new WechatPayAmount { Total = req.TotalFee, Currency = req.Currency },
                    TimeExpire = req.ExpireTime?.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                    Attach = req.Attach
                };
                var resp = await _wechat!.CreateNativeOrderAsync(orderReq, ct);
                return new CreateOrderResponse
                {
                    Channel = req.Channel,
                    OutTradeNo = req.OutTradeNo,
                    CodeUrl = resp.CodeUrl
                };
            }
        }
    }

    private async Task<QueryOrderResponse> QueryWechatOrderAsync(QueryOrderRequest req, CancellationToken ct)
    {
        EnsureWechat();
        WechatQueryOrderResponse raw;
        if (!string.IsNullOrEmpty(req.OutTradeNo))
            raw = await _wechat!.QueryOrderByOutTradeNoAsync(req.OutTradeNo, ct);
        else
            raw = await _wechat!.QueryOrderByTransactionIdAsync(req.TransactionId!, ct);

        return new QueryOrderResponse
        {
            Channel = req.Channel,
            OutTradeNo = raw.OutTradeNo,
            TransactionId = raw.TransactionId,
            TradeStatus = raw.TradeState,
            TotalFee = raw.Amount?.Total ?? 0,
            PayerFee = raw.Amount?.PayerTotal ?? 0,
            SuccessTime = DateTimeOffset.TryParse(raw.SuccessTime, CultureInfo.InvariantCulture, DateTimeStyles.None, out var wqt) ? wqt : null,
            BuyerAccount = raw.Payer?.OpenId,
            RawResponse = JsonSerializer.Serialize(raw)
        };
    }

    private async Task<RefundResponse> WechatRefundAsync(RefundRequest req, CancellationToken ct)
    {
        EnsureWechat();
        var refundReq = new WechatRefundRequest
        {
            OutTradeNo = req.OutTradeNo,
            TransactionId = req.TransactionId,
            OutRefundNo = req.OutRefundNo,
            Reason = req.Reason,
            NotifyUrl = req.NotifyUrl,
            Amount = new WechatRefundAmount
            {
                Refund = req.RefundFee,
                Total = req.TotalFee,
                Currency = "CNY"
            }
        };
        var raw = await _wechat!.RefundAsync(refundReq, ct);
        return new RefundResponse
        {
            Channel = req.Channel,
            OutRefundNo = raw.OutRefundNo ?? req.OutRefundNo,
            RefundId = raw.RefundId,
            RefundStatus = raw.Status,
            RefundFee = raw.Amount?.Refund ?? req.RefundFee,
            RawResponse = JsonSerializer.Serialize(raw)
        };
    }

    private async Task<QueryRefundResponse> QueryWechatRefundAsync(QueryRefundRequest req, CancellationToken ct)
    {
        EnsureWechat();
        var raw = await _wechat!.QueryRefundAsync(req.OutRefundNo, ct);
        return new QueryRefundResponse
        {
            Channel = req.Channel,
            OutTradeNo = raw.OutTradeNo,
            TransactionId = raw.TransactionId,
            OutRefundNo = raw.OutRefundNo,
            RefundStatus = raw.Status,
            RefundFee = raw.Amount?.Refund ?? 0,
            TotalFee = raw.Amount?.Total ?? 0,
            RawResponse = JsonSerializer.Serialize(raw)
        };
    }

    private async Task<CloseOrderResponse> CloseWechatOrderAsync(CloseOrderRequest req, CancellationToken ct)
    {
        EnsureWechat();
        var outTradeNo = req.OutTradeNo ?? throw new PayException("MISSING_PARAM", "微信关闭订单需要提供 OutTradeNo");
        await _wechat!.CloseOrderAsync(outTradeNo, ct);
        return new CloseOrderResponse
        {
            Channel = req.Channel,
            OutTradeNo = outTradeNo,
            Success = true
        };
    }

    private async Task<byte[]> DownloadWechatBillAsync(DownloadBillRequest req, CancellationToken ct)
    {
        EnsureWechat();
        return await _wechat!.DownloadTradeBillAsync(req.BillDate, req.BillType, ct);
    }

    private async Task<PayCallbackResult> ParseWechatCallbackAsync(PayChannel channel, string body, IDictionary<string, string>? headers)
    {
        EnsureWechat();
        var callbackHeaders = new WechatPayCallbackHeaders
        {
            Timestamp = headers?.TryGetValue("Wechatpay-Timestamp", out var t) == true ? t : string.Empty,
            Nonce     = headers?.TryGetValue("Wechatpay-Nonce",     out var n) == true ? n : string.Empty,
            Signature = headers?.TryGetValue("Wechatpay-Signature", out var s) == true ? s : string.Empty,
            Serial    = headers?.TryGetValue("Wechatpay-Serial",    out var sn) == true ? sn : null
        };

        try
        {
            var decrypted = await _wechat!.ParsePayCallbackAsync(body, callbackHeaders, ct: CancellationToken.None);
            return new PayCallbackResult
            {
                Channel = channel,
                IsValid = true,
                OutTradeNo = decrypted.OutTradeNo,
                TransactionId = decrypted.TransactionId,
                TradeStatus = decrypted.TradeState,
                TotalFee = decrypted.Amount?.Total ?? 0,
                BuyerAccount = decrypted.Payer?.OpenId,
                SuccessTime = DateTimeOffset.TryParse(decrypted.SuccessTime, CultureInfo.InvariantCulture, DateTimeStyles.None, out var wct) ? wct : null,
                RawBody = body
            };
        }
        catch (Exception ex)
        {
            return new PayCallbackResult
            {
                Channel = channel,
                IsValid = false,
                ErrorMessage = ex.Message,
                RawBody = body
            };
        }
    }

    // ─── 支付宝 ───────────────────────────────────────────────────────────────

    private async Task<CreateOrderResponse> CreateAlipayOrderAsync(CreateOrderRequest req, CancellationToken ct)
    {
        EnsureAlipay();
        var amountStr = (req.TotalFee / 100m).ToString("F2", CultureInfo.InvariantCulture);

        switch (req.Channel.ToAlipayPayMethod())
        {
            case "FACE_TO_FACE":
            {
                var content = new AlipayTradePayBizContent
                {
                    OutTradeNo = req.OutTradeNo,
                    Subject = req.Subject,
                    TotalAmount = amountStr,
                    AuthCode = req.AuthCode ?? string.Empty,
                    TimeExpire = req.ExpireTime?.ToString("yyyy-MM-dd HH:mm:ss")
                };
                var resp = await _alipay!.FaceToFacePayAsync(content, ct);
                return new CreateOrderResponse
                {
                    Channel = req.Channel,
                    OutTradeNo = req.OutTradeNo,
                    PrepayId = resp.TradeNo
                };
            }
            case "PRECREATE":
            {
                var content = new AlipayTradePrecreateContent
                {
                    OutTradeNo = req.OutTradeNo,
                    Subject = req.Subject,
                    TotalAmount = amountStr,
                    TimeExpire = req.ExpireTime?.ToString("yyyy-MM-dd HH:mm:ss")
                };
                var resp = await _alipay!.PrecreateAsync(content, notifyUrl: req.NotifyUrl, ct: ct);
                return new CreateOrderResponse
                {
                    Channel = req.Channel,
                    OutTradeNo = req.OutTradeNo,
                    CodeUrl = resp.QrCode
                };
            }
            case "JSAPI":
            {
                var content = new AlipayTradeCreateContent
                {
                    OutTradeNo = req.OutTradeNo,
                    Subject = req.Subject,
                    TotalAmount = amountStr,
                    BuyerOpenId = req.OpenId,
                    OpAppId = req.Extra?.TryGetValue("OpAppId", out var opAppId) == true ? opAppId : null,
                    ProductCode = req.Extra?.TryGetValue("ProductCode", out var pc) == true ? pc : null,
                    TimeExpire = req.ExpireTime?.ToString("yyyy-MM-dd HH:mm:ss")
                };
                var resp = await _alipay!.CreateOrderAsync(content, notifyUrl: req.NotifyUrl, ct: ct);
                return new CreateOrderResponse
                {
                    Channel = req.Channel,
                    OutTradeNo = req.OutTradeNo,
                    PrepayId = resp.TradeNo
                };
            }
            case "APP":
            {
                var content = new AlipayTradeAppPayContent
                {
                    OutTradeNo = req.OutTradeNo,
                    Subject = req.Subject,
                    TotalAmount = amountStr,
                    TimeExpire = req.ExpireTime?.ToString("yyyy-MM-dd HH:mm:ss")
                };
                var sdkStr = _alipay!.BuildAppPayString(content, req.NotifyUrl);
                return new CreateOrderResponse
                {
                    Channel = req.Channel,
                    OutTradeNo = req.OutTradeNo,
                    SdkOrderString = sdkStr
                };
            }
            case "WAP":
            {
                var content = new AlipayTradeWapPayContent
                {
                    OutTradeNo = req.OutTradeNo,
                    Subject = req.Subject,
                    TotalAmount = amountStr,
                    TimeExpire = req.ExpireTime?.ToString("yyyy-MM-dd HH:mm:ss")
                };
                var url = _alipay!.BuildWapPayUrl(content, req.NotifyUrl, req.ReturnUrl);
                return new CreateOrderResponse
                {
                    Channel = req.Channel,
                    OutTradeNo = req.OutTradeNo,
                    PayUrl = url
                };
            }
            default: // PAGE
            {
                var content = new AlipayTradePagePayContent
                {
                    OutTradeNo = req.OutTradeNo,
                    Subject = req.Subject,
                    TotalAmount = amountStr,
                    TimeExpire = req.ExpireTime?.ToString("yyyy-MM-dd HH:mm:ss")
                };
                var url = _alipay!.BuildPagePayUrl(content, req.NotifyUrl, req.ReturnUrl);
                return new CreateOrderResponse
                {
                    Channel = req.Channel,
                    OutTradeNo = req.OutTradeNo,
                    PayUrl = url
                };
            }
        }
    }

    private async Task<QueryOrderResponse> QueryAlipayOrderAsync(QueryOrderRequest req, CancellationToken ct)
    {
        EnsureAlipay();
        var content = new AlipayTradeQueryContent
        {
            OutTradeNo = req.OutTradeNo,
            TradeNo = req.TransactionId
        };
        var raw = await _alipay!.QueryOrderAsync(content, ct);

        var totalFee = 0;
        if (decimal.TryParse(raw.TotalAmount, NumberStyles.Any, CultureInfo.InvariantCulture, out var total))
            totalFee = (int)(total * 100);

        var payerFee = 0;
        if (decimal.TryParse(raw.BuyerPayAmount, NumberStyles.Any, CultureInfo.InvariantCulture, out var payer))
            payerFee = (int)(payer * 100);

        return new QueryOrderResponse
        {
            Channel = req.Channel,
            OutTradeNo = raw.OutTradeNo ?? req.OutTradeNo ?? string.Empty,
            TransactionId = raw.TradeNo,
            TradeStatus = raw.TradeStatus ?? string.Empty,
            TotalFee = totalFee,
            PayerFee = payerFee,
            BuyerAccount = raw.BuyerLogonId,
            SuccessTime = raw.SendPayDate is not null
                ? DateTimeOffset.ParseExact(raw.SendPayDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
                : null,
            RawResponse = JsonSerializer.Serialize(raw)
        };
    }

    private async Task<RefundResponse> AlipayRefundAsync(RefundRequest req, CancellationToken ct)
    {
        EnsureAlipay();
        var refundAmountStr = (req.RefundFee / 100m).ToString("F2", CultureInfo.InvariantCulture);
        var content = new AlipayTradeRefundContent
        {
            OutTradeNo = req.OutTradeNo,
            TradeNo = req.TransactionId,
            RefundAmount = refundAmountStr,
            RefundReason = req.Reason,
            OutRequestNo = req.OutRefundNo
        };
        var raw = await _alipay!.RefundAsync(content, ct);

        var refundFee = 0;
        if (decimal.TryParse(raw.RefundFee, NumberStyles.Any, CultureInfo.InvariantCulture, out var fee))
            refundFee = (int)(fee * 100);

        return new RefundResponse
        {
            Channel = req.Channel,
            OutRefundNo = req.OutRefundNo,
            RefundId = raw.TradeNo,
            RefundStatus = raw.FundChange == "Y" ? "SUCCESS" : "PROCESSING",
            RefundFee = refundFee,
            RawResponse = JsonSerializer.Serialize(raw)
        };
    }

    private async Task<QueryRefundResponse> QueryAlipayRefundAsync(QueryRefundRequest req, CancellationToken ct)
    {
        EnsureAlipay();
        var content = new AlipayTradeRefundQueryContent
        {
            OutTradeNo = req.OutTradeNo,
            TradeNo = req.TransactionId,
            OutRequestNo = req.OutRefundNo
        };
        var raw = await _alipay!.QueryRefundAsync(content, ct);

        var refundFee = 0;
        if (decimal.TryParse(raw.RefundAmount, NumberStyles.Any, CultureInfo.InvariantCulture, out var fee))
            refundFee = (int)(fee * 100);

        var totalFee = 0;
        if (decimal.TryParse(raw.TotalAmount, NumberStyles.Any, CultureInfo.InvariantCulture, out var total))
            totalFee = (int)(total * 100);

        return new QueryRefundResponse
        {
            Channel = req.Channel,
            OutTradeNo = raw.OutTradeNo,
            TransactionId = raw.TradeNo,
            OutRefundNo = raw.OutRequestNo ?? req.OutRefundNo,
            RefundStatus = raw.RefundAmount is not null ? "SUCCESS" : "PROCESSING",
            RefundFee = refundFee,
            TotalFee = totalFee,
            RawResponse = JsonSerializer.Serialize(raw)
        };
    }

    private async Task<CloseOrderResponse> CloseAlipayOrderAsync(CloseOrderRequest req, CancellationToken ct)
    {
        EnsureAlipay();
        var content = new AlipayTradeCloseContent
        {
            OutTradeNo = req.OutTradeNo,
            TradeNo = req.TransactionId
        };
        var raw = await _alipay!.CloseOrderAsync(content, ct: ct);
        return new CloseOrderResponse
        {
            Channel = req.Channel,
            OutTradeNo = raw.OutTradeNo ?? req.OutTradeNo,
            TransactionId = raw.TradeNo,
            Success = true,
            RawResponse = JsonSerializer.Serialize(raw)
        };
    }

    private async Task<byte[]> DownloadAlipayBillAsync(DownloadBillRequest req, CancellationToken ct)
    {
        EnsureAlipay();
        var content = new AlipayBillDownloadContent
        {
            BillType = req.BillType == "ALL" ? "trade" : req.BillType,
            BillDate = req.BillDate
        };
        return await _alipay!.DownloadBillAsync(content, ct);
    }

    private PayCallbackResult ParseAlipayCallback(PayChannel channel, string body)
    {
        EnsureAlipay();
        var formParams = ParseFormString(body);
        try
        {
            var result = _alipay!.ParseCallback(formParams);

            if (!result.IsValid)
            {
                return new PayCallbackResult
                {
                    Channel = channel,
                    IsValid = false,
                    ErrorMessage = "支付宝回调签名验证失败",
                    RawBody = body
                };
            }

            var totalFee = 0;
            if (decimal.TryParse(result.TotalAmount, NumberStyles.Any, CultureInfo.InvariantCulture, out var total))
                totalFee = (int)(total * 100);

            return new PayCallbackResult
            {
                Channel = channel,
                IsValid = true,
                OutTradeNo = result.OutTradeNo,
                TransactionId = result.TradeNo,
                TradeStatus = result.TradeStatus,
                TotalFee = totalFee,
                BuyerAccount = result.BuyerLogonId,
                SuccessTime = result.GmtPayment is not null
                    ? DateTimeOffset.ParseExact(result.GmtPayment, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
                    : null,
                RawBody = body
            };
        }
        catch (Exception ex)
        {
            return new PayCallbackResult
            {
                Channel = channel,
                IsValid = false,
                ErrorMessage = ex.Message,
                RawBody = body
            };
        }
    }

    // ─── 银联 ─────────────────────────────────────────────────────────────────

    private async Task<CreateOrderResponse> CreateUnionPayOrderAsync(CreateOrderRequest req, CancellationToken ct)
    {
        EnsureUnionPay();
        switch (req.Channel.ToUnionPayProductType())
        {
            case "WAP":
            {
                var payReq = new UnionPayWapPayRequest
                {
                    OrderId = req.OutTradeNo,
                    TxnTime = DateTime.Now.ToString("yyyyMMddHHmmss"),
                    TxnAmt = req.TotalFee.ToString(),
                    OrderDesc = req.Subject,
                    FrontUrl = req.ReturnUrl ?? string.Empty,
                    BackUrl = req.NotifyUrl,
                    ReqReserved = req.Attach
                };
                var resp = _unionPay!.CreateWapPay(payReq);
                return new CreateOrderResponse
                {
                    Channel = req.Channel,
                    OutTradeNo = req.OutTradeNo,
                    PayUrl = resp.FormHtml
                };
            }
            case "NO_REDIRECT":
            {
                var payReq = new UnionPayBackPayRequest
                {
                    BizType = "000301",
                    OrderId = req.OutTradeNo,
                    TxnTime = DateTime.Now.ToString("yyyyMMddHHmmss"),
                    TxnAmt = req.TotalFee.ToString(),
                    OrderDesc = req.Subject,
                    BackUrl = req.NotifyUrl,
                    AccNo = req.Extra?.TryGetValue("AccNo", out var accNo) == true ? accNo : null,
                    CustomerInfo = req.Extra?.TryGetValue("CustomerInfo", out var ci) == true ? ci : null,
                    SmsCode = req.Extra?.TryGetValue("SmsCode", out var sms) == true ? sms : null,
                    ReqReserved = req.Attach
                };
                var resp = await _unionPay!.CreateBackPayAsync(payReq, ct);
                return new CreateOrderResponse
                {
                    Channel = req.Channel,
                    OutTradeNo = req.OutTradeNo,
                    PrepayId = resp.QueryId
                };
            }
            case "QR_CODE":
            {
                if (!string.IsNullOrEmpty(req.AuthCode))
                {
                    var consumeReq = new UnionPayQrCodeConsumeRequest
                    {
                        OrderId = req.OutTradeNo,
                        TxnTime = DateTime.Now.ToString("yyyyMMddHHmmss"),
                        TxnAmt = req.TotalFee.ToString(),
                        OrderDesc = req.Subject,
                        BackUrl = req.NotifyUrl,
                        QrNo = req.AuthCode,
                        ReqReserved = req.Attach
                    };
                    var consumeResp = await _unionPay!.QrCodeConsumeAsync(consumeReq, ct);
                    return new CreateOrderResponse
                    {
                        Channel = req.Channel,
                        OutTradeNo = req.OutTradeNo,
                        PrepayId = consumeResp.QueryId
                    };
                }
                else
                {
                    var applyReq = new UnionPayQrCodeApplyRequest
                    {
                        OrderId = req.OutTradeNo,
                        TxnTime = DateTime.Now.ToString("yyyyMMddHHmmss"),
                        TxnAmt = req.TotalFee.ToString(),
                        OrderDesc = req.Subject,
                        BackUrl = req.NotifyUrl,
                        ReqReserved = req.Attach
                    };
                    var applyResp = await _unionPay!.ApplyQrCodeAsync(applyReq, ct);
                    return new CreateOrderResponse
                    {
                        Channel = req.Channel,
                        OutTradeNo = req.OutTradeNo,
                        CodeUrl = applyResp.QrCode
                    };
                }
            }
            case "CONTRACT":
            {
                var payReq = new UnionPayBackPayRequest
                {
                    BizType = "000301",
                    OrderId = req.OutTradeNo,
                    TxnTime = DateTime.Now.ToString("yyyyMMddHHmmss"),
                    TxnAmt = req.TotalFee.ToString(),
                    OrderDesc = req.Subject,
                    BackUrl = req.NotifyUrl,
                    ContractNo = req.Extra?.TryGetValue("ContractNo", out var cn) == true ? cn : null,
                    ReqReserved = req.Attach
                };
                var resp = await _unionPay!.CreateBackPayAsync(payReq, ct);
                return new CreateOrderResponse
                {
                    Channel = req.Channel,
                    OutTradeNo = req.OutTradeNo,
                    PrepayId = resp.QueryId
                };
            }
            case "QUICK_PASS":
            {
                var payReq = new UnionPayBackPayRequest
                {
                    BizType = "000902",
                    OrderId = req.OutTradeNo,
                    TxnTime = DateTime.Now.ToString("yyyyMMddHHmmss"),
                    TxnAmt = req.TotalFee.ToString(),
                    OrderDesc = req.Subject,
                    BackUrl = req.NotifyUrl,
                    TokenPayData = req.Extra?.TryGetValue("TokenPayData", out var tpd) == true ? tpd : null,
                    ReqReserved = req.Attach
                };
                var resp = await _unionPay!.CreateBackPayAsync(payReq, ct);
                return new CreateOrderResponse
                {
                    Channel = req.Channel,
                    OutTradeNo = req.OutTradeNo,
                    PrepayId = resp.QueryId
                };
            }
            case "APPLE_PAY":
            {
                var payReq = new UnionPayBackPayRequest
                {
                    BizType = "000802",
                    OrderId = req.OutTradeNo,
                    TxnTime = DateTime.Now.ToString("yyyyMMddHHmmss"),
                    TxnAmt = req.TotalFee.ToString(),
                    OrderDesc = req.Subject,
                    BackUrl = req.NotifyUrl,
                    TokenPayData = req.Extra?.TryGetValue("TokenPayData", out var appleToken) == true ? appleToken : null,
                    ReqReserved = req.Attach
                };
                var resp = await _unionPay!.CreateBackPayAsync(payReq, ct);
                return new CreateOrderResponse
                {
                    Channel = req.Channel,
                    OutTradeNo = req.OutTradeNo,
                    PrepayId = resp.QueryId
                };
            }
            default: // GATEWAY
            {
                var payReq = new UnionPayFrontPayRequest
                {
                    OrderId = req.OutTradeNo,
                    TxnTime = DateTime.Now.ToString("yyyyMMddHHmmss"),
                    TxnAmt = req.TotalFee.ToString(),
                    OrderDesc = req.Subject,
                    FrontUrl = req.ReturnUrl ?? string.Empty,
                    BackUrl = req.NotifyUrl,
                    ReqReserved = req.Attach
                };
                var resp = _unionPay!.CreateFrontPay(payReq);
                return new CreateOrderResponse
                {
                    Channel = req.Channel,
                    OutTradeNo = req.OutTradeNo,
                    PayUrl = resp.FormHtml
                };
            }
        }
    }

    private async Task<QueryOrderResponse> QueryUnionPayOrderAsync(QueryOrderRequest req, CancellationToken ct)
    {
        EnsureUnionPay();
        var queryReq = new UnionPayQueryRequest
        {
            OrderId = req.OutTradeNo ?? string.Empty,
            TxnTime = req.Extra?.TryGetValue("TxnTime", out var t) == true ? t : DateTime.Now.ToString("yyyyMMddHHmmss")
        };
        var raw = await _unionPay!.QueryOrderAsync(queryReq, ct);

        var totalFee = 0;
        if (int.TryParse(raw.TxnAmt, out var fee))
            totalFee = fee;

        return new QueryOrderResponse
        {
            Channel = req.Channel,
            OutTradeNo = raw.OrderId ?? string.Empty,
            TransactionId = raw.QueryId,
            TradeStatus = raw.OrigRespCode == "00" ? "SUCCESS" : raw.RespCode,
            TotalFee = totalFee,
            RawResponse = JsonSerializer.Serialize(raw.RawParams)
        };
    }

    private async Task<RefundResponse> UnionPayRefundAsync(RefundRequest req, CancellationToken ct)
    {
        EnsureUnionPay();
        var refundReq = new UnionPayRefundRequest
        {
            OrderId = req.OutRefundNo,
            TxnTime = DateTime.Now.ToString("yyyyMMddHHmmss"),
            TxnAmt = req.RefundFee.ToString(),
            OrigQueryId = req.TransactionId ?? string.Empty,
            BackUrl = req.NotifyUrl ?? string.Empty
        };
        var raw = await _unionPay!.RefundAsync(refundReq, ct);
        return new RefundResponse
        {
            Channel = req.Channel,
            OutRefundNo = req.OutRefundNo,
            RefundId = raw.QueryId,
            RefundStatus = raw.RespCode == "00" ? "SUCCESS" : raw.RespCode,
            RefundFee = req.RefundFee,
            RawResponse = JsonSerializer.Serialize(raw.RawParams)
        };
    }

    private PayCallbackResult ParseUnionPayCallback(string body)
    {
        EnsureUnionPay();
        var formParams = ParseFormString(body);
        var result = _unionPay!.ParseCallback(formParams);

        var totalFee = 0;
        if (int.TryParse(result.TxnAmt, out var fee))
            totalFee = fee;

        return new PayCallbackResult
        {
            Channel = PayChannel.UnionPayGateway,
            IsValid = result.IsValid,
            OutTradeNo = result.OrderId ?? string.Empty,
            TransactionId = result.QueryId,
            TradeStatus = result.RespCode == "00" ? "SUCCESS" : result.RespCode ?? string.Empty,
            TotalFee = totalFee,
            RawBody = body,
            ErrorMessage = result.IsValid ? null : "银联回调签名验证失败"
        };
    }

    private static CloseOrderResponse CloseUnionPayOrder(CloseOrderRequest req)
    {
        // 银联网关支付模式下，不提供独立的关闭订单 API
        // 未支付订单会自动超时关闭，此处返回成功以保持统一接口一致性
        return new CloseOrderResponse
        {
            Channel = req.Channel,
            OutTradeNo = req.OutTradeNo,
            Success = true
        };
    }

    private async Task<QueryRefundResponse> QueryUnionPayRefundAsync(QueryRefundRequest req, CancellationToken ct)
    {
        EnsureUnionPay();
        // 银联使用交易查询接口查询退款结果（退款订单也是一笔交易）
        var queryReq = new UnionPayQueryRequest
        {
            OrderId = req.OutRefundNo,
            TxnTime = req.Extra?.TryGetValue("TxnTime", out var t) == true ? t : DateTime.Now.ToString("yyyyMMddHHmmss")
        };
        var raw = await _unionPay!.QueryOrderAsync(queryReq, ct);

        var refundFee = 0;
        if (int.TryParse(raw.TxnAmt, out var fee))
            refundFee = fee;

        return new QueryRefundResponse
        {
            Channel = req.Channel,
            OutRefundNo = raw.OrderId ?? req.OutRefundNo,
            TransactionId = raw.QueryId,
            RefundStatus = raw.OrigRespCode == "00" ? "SUCCESS" : raw.OrigRespCode ?? "PROCESSING",
            RefundFee = refundFee,
            RawResponse = JsonSerializer.Serialize(raw.RawParams)
        };
    }

    private async Task<byte[]> DownloadUnionPayBillAsync(DownloadBillRequest req, CancellationToken ct)
    {
        EnsureUnionPay();
        var fileType = req.BillType == "ALL" ? "00" : req.BillType;
        return await _unionPay!.DownloadBillAsync(req.BillDate, fileType, ct);
    }

    // ─── 辅助 ──────────────────────────────────────────────────────────────────

    private void EnsureWechat()
    {
        if (_wechat is null)
            throw new PayException("SERVICE_NOT_CONFIGURED", "微信支付服务未配置，请调用 AddWechatPay() 注册");
    }

    private void EnsureAlipay()
    {
        if (_alipay is null)
            throw new PayException("SERVICE_NOT_CONFIGURED", "支付宝服务未配置，请调用 AddAlipay() 注册");
    }

    private void EnsureUnionPay()
    {
        if (_unionPay is null)
            throw new PayException("SERVICE_NOT_CONFIGURED", "银联服务未配置，请调用 AddUnionPay() 注册");
    }

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
