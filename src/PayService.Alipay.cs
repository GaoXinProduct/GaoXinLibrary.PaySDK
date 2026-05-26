using System.Globalization;
using System.Text.Json;
using GaoXinLibrary.PaySDK.Alipay.Models;
using GaoXinLibrary.PaySDK.Alipay.Services;
using GaoXinLibrary.PaySDK.Core;

namespace GaoXinLibrary.PaySDK;

public sealed partial class PayService : IPayService
{
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

    private void EnsureAlipay()
    {
        if (_alipay is null)
            throw new PayException("SERVICE_NOT_CONFIGURED", "支付宝服务未配置，请调用 AddAlipay() 注册");
    }
}
