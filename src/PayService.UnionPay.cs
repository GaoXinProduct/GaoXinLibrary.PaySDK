using System.Globalization;
using System.Text.Json;
using GaoXinLibrary.PaySDK.Core;
using GaoXinLibrary.PaySDK.UnionPay.Models;
using GaoXinLibrary.PaySDK.UnionPay.Services;

namespace GaoXinLibrary.PaySDK;

public sealed partial class PayService : IPayService
{
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
        // 未支付订单会自动超时关闭，此处返回模拟成功以保持统一接口一致性
        // 注意：IsSimulated = true 表示实际并未向银联发送关闭请求
        return new CloseOrderResponse
        {
            Channel = req.Channel,
            OutTradeNo = req.OutTradeNo,
            Success = true,
            IsSimulated = true,
            OperationMode = CloseOrderOperationMode.Simulated
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

    private void EnsureUnionPay()
    {
        if (_unionPay is null)
            throw new PayException("SERVICE_NOT_CONFIGURED", "银联服务未配置，请调用 AddUnionPay() 注册");
    }
}
