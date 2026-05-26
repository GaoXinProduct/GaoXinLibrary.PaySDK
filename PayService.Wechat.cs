using System.Globalization;
using System.Text.Json;
using GaoXinLibrary.PaySDK.Core;
using GaoXinLibrary.PaySDK.Wechat.Models;
using GaoXinLibrary.PaySDK.Wechat.Services;

namespace GaoXinLibrary.PaySDK;

public sealed partial class PayService : IPayService
{
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

    private async Task<PayCallbackResult> ParseWechatCallbackAsync(
        PayChannel channel,
        string body,
        IDictionary<string, string>? headers,
        CancellationToken ct)
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
            var decrypted = await _wechat!.ParsePayCallbackAsync(body, callbackHeaders, ct);
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

    private void EnsureWechat()
    {
        if (_wechat is null)
            throw new PayException("SERVICE_NOT_CONFIGURED", "微信支付服务未配置，请调用 AddWechatPay() 注册");
    }
}
