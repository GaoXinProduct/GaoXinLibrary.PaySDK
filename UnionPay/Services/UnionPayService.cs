using GaoXinLibrary.PaySDK.UnionPay.Core;
using GaoXinLibrary.PaySDK.UnionPay.Models;

namespace GaoXinLibrary.PaySDK.UnionPay.Services;

/// <summary>
/// 银联支付服务实现
/// </summary>
public sealed class UnionPayService : IUnionPayService
{
    private readonly UnionPayHttpClient _http;
    private readonly UnionPayOptions _options;

    /// <summary>
    /// 初始化银联支付服务
    /// </summary>
    public UnionPayService(UnionPayHttpClient http, UnionPayOptions options)
    {
        _http = http;
        _options = options;
    }

    /// <inheritdoc/>
    public UnionPayFrontPayResponse CreateFrontPay(UnionPayFrontPayRequest request)
    {
        var parameters = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["txnType"]    = request.TxnType,
            ["txnSubType"] = request.TxnSubType,
            ["bizType"]    = request.BizType,
            ["orderId"]    = request.OrderId,
            ["txnTime"]    = request.TxnTime,
            ["txnAmt"]     = request.TxnAmt,
            ["orderDesc"]  = request.OrderDesc,
            ["frontUrl"]   = string.IsNullOrEmpty(request.FrontUrl) ? _options.FrontUrl : request.FrontUrl,
            ["backUrl"]    = string.IsNullOrEmpty(request.BackUrl) ? _options.BackUrl : request.BackUrl
        };

        if (!string.IsNullOrEmpty(request.PayTimeout))
            parameters["payTimeout"] = request.PayTimeout;
        if (!string.IsNullOrEmpty(request.ReqReserved))
            parameters["reqReserved"] = request.ReqReserved;

        var html = _http.BuildFrontFormHtml(parameters);
        return new UnionPayFrontPayResponse { FormHtml = html };
    }

    /// <inheritdoc/>
    public UnionPayFrontPayResponse CreateWapPay(UnionPayWapPayRequest request)
    {
        var parameters = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["txnType"]     = request.TxnType,
            ["txnSubType"]  = request.TxnSubType,
            ["bizType"]     = request.BizType,
            ["channelType"] = request.ChannelType,
            ["orderId"]     = request.OrderId,
            ["txnTime"]     = request.TxnTime,
            ["txnAmt"]      = request.TxnAmt,
            ["orderDesc"]   = request.OrderDesc,
            ["frontUrl"]    = string.IsNullOrEmpty(request.FrontUrl) ? _options.FrontUrl : request.FrontUrl,
            ["backUrl"]     = string.IsNullOrEmpty(request.BackUrl) ? _options.BackUrl : request.BackUrl
        };

        if (!string.IsNullOrEmpty(request.PayTimeout))
            parameters["payTimeout"] = request.PayTimeout;
        if (!string.IsNullOrEmpty(request.ReqReserved))
            parameters["reqReserved"] = request.ReqReserved;

        var html = _http.BuildFrontFormHtml(parameters, _options.AppGatewayUrl);
        return new UnionPayFrontPayResponse { FormHtml = html };
    }

    /// <inheritdoc/>
    public async Task<UnionPayBackPayResponse> CreateBackPayAsync(UnionPayBackPayRequest request, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["txnType"]     = request.TxnType,
            ["txnSubType"]  = request.TxnSubType,
            ["bizType"]     = request.BizType,
            ["channelType"] = request.ChannelType,
            ["orderId"]     = request.OrderId,
            ["txnTime"]     = request.TxnTime,
            ["txnAmt"]      = request.TxnAmt,
            ["orderDesc"]   = request.OrderDesc,
            ["backUrl"]     = string.IsNullOrEmpty(request.BackUrl) ? _options.BackUrl : request.BackUrl
        };

        if (!string.IsNullOrEmpty(request.AccNo))
            parameters["accNo"] = request.AccNo;
        if (!string.IsNullOrEmpty(request.CustomerInfo))
            parameters["customerInfo"] = request.CustomerInfo;
        if (!string.IsNullOrEmpty(request.SmsCode))
            parameters["smsCode"] = request.SmsCode;
        if (!string.IsNullOrEmpty(request.TokenPayData))
            parameters["tokenPayData"] = request.TokenPayData;
        if (!string.IsNullOrEmpty(request.ContractNo))
            parameters["contractNo"] = request.ContractNo;
        if (!string.IsNullOrEmpty(request.ReqReserved))
            parameters["reqReserved"] = request.ReqReserved;

        var raw = await _http.PostBackAsync(parameters, _options.BackGatewayUrl, ct);
        ValidateResponse(raw);

        return new UnionPayBackPayResponse
        {
            RespCode  = GetValue(raw, "respCode"),
            RespMsg   = GetValue(raw, "respMsg"),
            OrderId   = GetValue(raw, "orderId"),
            QueryId   = GetValue(raw, "queryId"),
            TxnAmt    = GetValue(raw, "txnAmt"),
            SettleAmt = GetValue(raw, "settleAmt"),
            SettleDate = GetValue(raw, "settleDate"),
            RawParams = raw
        };
    }

    /// <inheritdoc/>
    public async Task<UnionPayQrCodeApplyResponse> ApplyQrCodeAsync(UnionPayQrCodeApplyRequest request, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["txnType"]    = request.TxnType,
            ["txnSubType"] = request.TxnSubType,
            ["bizType"]    = request.BizType,
            ["orderId"]    = request.OrderId,
            ["txnTime"]    = request.TxnTime,
            ["txnAmt"]     = request.TxnAmt,
            ["orderDesc"]  = request.OrderDesc,
            ["backUrl"]    = string.IsNullOrEmpty(request.BackUrl) ? _options.BackUrl : request.BackUrl
        };

        if (!string.IsNullOrEmpty(request.PayTimeout))
            parameters["payTimeout"] = request.PayTimeout;
        if (!string.IsNullOrEmpty(request.ReqReserved))
            parameters["reqReserved"] = request.ReqReserved;

        var raw = await _http.PostBackAsync(parameters, _options.BackGatewayUrl, ct);
        ValidateResponse(raw);

        return new UnionPayQrCodeApplyResponse
        {
            RespCode  = GetValue(raw, "respCode"),
            RespMsg   = GetValue(raw, "respMsg"),
            OrderId   = GetValue(raw, "orderId"),
            QueryId   = GetValue(raw, "queryId"),
            QrCode    = GetValue(raw, "qrCode"),
            RawParams = raw
        };
    }

    /// <inheritdoc/>
    public async Task<UnionPayBackPayResponse> QrCodeConsumeAsync(UnionPayQrCodeConsumeRequest request, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["txnType"]    = request.TxnType,
            ["txnSubType"] = request.TxnSubType,
            ["bizType"]    = request.BizType,
            ["orderId"]    = request.OrderId,
            ["txnTime"]    = request.TxnTime,
            ["txnAmt"]     = request.TxnAmt,
            ["orderDesc"]  = request.OrderDesc,
            ["backUrl"]    = string.IsNullOrEmpty(request.BackUrl) ? _options.BackUrl : request.BackUrl,
            ["qrNo"]       = request.QrNo
        };

        if (!string.IsNullOrEmpty(request.ReqReserved))
            parameters["reqReserved"] = request.ReqReserved;

        var raw = await _http.PostBackAsync(parameters, _options.BackGatewayUrl, ct);
        ValidateResponse(raw);

        return new UnionPayBackPayResponse
        {
            RespCode  = GetValue(raw, "respCode"),
            RespMsg   = GetValue(raw, "respMsg"),
            OrderId   = GetValue(raw, "orderId"),
            QueryId   = GetValue(raw, "queryId"),
            TxnAmt    = GetValue(raw, "txnAmt"),
            SettleAmt = GetValue(raw, "settleAmt"),
            SettleDate = GetValue(raw, "settleDate"),
            RawParams = raw
        };
    }

    /// <inheritdoc/>
    public async Task<UnionPayQueryResponse> QueryOrderAsync(UnionPayQueryRequest request, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["txnType"]    = request.TxnType,
            ["txnSubType"] = request.TxnSubType,
            ["bizType"]    = request.BizType,
            ["orderId"]    = request.OrderId,
            ["txnTime"]    = request.TxnTime
        };

        var raw = await _http.PostBackAsync(parameters, _options.QueryGatewayUrl, ct);
        ValidateResponse(raw);

        return new UnionPayQueryResponse
        {
            RespCode    = GetValue(raw, "respCode"),
            RespMsg     = GetValue(raw, "respMsg"),
            OrderId     = GetValue(raw, "orderId"),
            QueryId     = GetValue(raw, "queryId"),
            OrigRespCode = GetValue(raw, "origRespCode"),
            OrigRespMsg  = GetValue(raw, "origRespMsg"),
            TxnAmt      = GetValue(raw, "txnAmt"),
            SettleAmt   = GetValue(raw, "settleAmt"),
            SettleDate  = GetValue(raw, "settleDate"),
            RawParams   = raw
        };
    }

    /// <inheritdoc/>
    public async Task<UnionPayRefundResponse> RefundAsync(UnionPayRefundRequest request, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["txnType"]     = request.TxnType,
            ["txnSubType"]  = request.TxnSubType,
            ["bizType"]     = request.BizType,
            ["orderId"]     = request.OrderId,
            ["txnTime"]     = request.TxnTime,
            ["txnAmt"]      = request.TxnAmt,
            ["origQueryId"] = request.OrigQueryId,
            ["backUrl"]     = string.IsNullOrEmpty(request.BackUrl) ? _options.BackUrl : request.BackUrl
        };

        if (!string.IsNullOrEmpty(request.ReqReserved))
            parameters["reqReserved"] = request.ReqReserved;

        var raw = await _http.PostBackAsync(parameters, _options.BackGatewayUrl, ct);
        ValidateResponse(raw);

        return new UnionPayRefundResponse
        {
            RespCode  = GetValue(raw, "respCode"),
            RespMsg   = GetValue(raw, "respMsg"),
            OrderId   = GetValue(raw, "orderId"),
            QueryId   = GetValue(raw, "queryId"),
            RawParams = raw
        };
    }

    /// <inheritdoc/>
    public UnionPayCallbackParams ParseCallback(IDictionary<string, string> formParams)
    {
        var raw = new Dictionary<string, string>(formParams, StringComparer.Ordinal);
        var isValid = _http.VerifyCallback(raw);

        return new UnionPayCallbackParams
        {
            IsValid      = isValid,
            RespCode     = GetValue(raw, "respCode"),
            RespMsg      = GetValue(raw, "respMsg"),
            OrderId      = GetValue(raw, "orderId"),
            QueryId      = GetValue(raw, "queryId"),
            TxnAmt       = GetValue(raw, "txnAmt"),
            SettleDate   = GetValue(raw, "settleDate"),
            ReqReserved  = raw.TryGetValue("reqReserved", out var rr) ? rr : null,
            RawParams    = raw
        };
    }

    /// <inheritdoc/>
    public async Task<byte[]> DownloadBillAsync(string settleDate, string fileType, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["txnType"]    = "76",
            ["txnSubType"] = "01",
            ["bizType"]    = "000000",
            ["settleDate"] = settleDate,
            ["fileType"]   = fileType
        };

        return await _http.DownloadFileAsync(parameters, _options.FileGatewayUrl, ct);
    }

    private static void ValidateResponse(Dictionary<string, string> raw)
    {
        var respCode = GetValue(raw, "respCode");
        if (respCode != "00")
        {
            var respMsg = GetValue(raw, "respMsg");
            throw new UnionPayException(respCode, respMsg);
        }
    }

    private static string GetValue(Dictionary<string, string> dict, string key)
        => dict.TryGetValue(key, out var v) ? v : string.Empty;
}
