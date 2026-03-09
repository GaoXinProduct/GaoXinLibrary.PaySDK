using System.IO.Compression;
using System.Text;
using GaoXinLibrary.PaySDK.UnionPay.Core;
using GaoXinLibrary.PaySDK.UnionPay.Models;

namespace GaoXinLibrary.PaySDK.UnionPay.Services;

/// <summary>
/// 银联跨境电商海关申报服务实现
/// </summary>
public sealed class UnionPayCustomsService : IUnionPayCustomsService
{
    private readonly UnionPayHttpClient _http;
    private readonly UnionPayOptions _options;

    /// <summary>
    /// 初始化海关申报服务
    /// </summary>
    public UnionPayCustomsService(UnionPayHttpClient http, UnionPayOptions options)
    {
        _http = http;
        _options = options;
    }

    /// <inheritdoc/>
    public async Task<UnionPayCustomsDeclarationResponse> DeclareAsync(UnionPayCustomsDeclarationRequest request, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["txnType"]     = request.TxnType,
            ["txnSubType"]  = request.TxnSubType,
            ["bizType"]     = request.BizType,
            ["orderId"]     = request.OrderId,
            ["txnTime"]     = request.TxnTime,
            ["txnAmt"]      = request.TxnAmt,
            ["origQryId"]   = request.OrigQueryId
        };

        if (!string.IsNullOrEmpty(request.CustomsCode))
            parameters["customsCode"] = request.CustomsCode;
        if (!string.IsNullOrEmpty(request.MerAbbr))
            parameters["merAbbr"] = request.MerAbbr;
        if (!string.IsNullOrEmpty(request.MerCatCode))
            parameters["merCatCode"] = request.MerCatCode;
        if (!string.IsNullOrEmpty(request.ReqReserved))
            parameters["reqReserved"] = request.ReqReserved;

        var raw = await _http.PostBackAsync(parameters, _options.BackGatewayUrl, ct);
        ValidateResponse(raw);

        return new UnionPayCustomsDeclarationResponse
        {
            RespCode   = GetValue(raw, "respCode"),
            RespMsg    = GetValue(raw, "respMsg"),
            OrderId    = GetValue(raw, "orderId"),
            OrigQryId  = GetValue(raw, "origQryId"),
            PayTransNo = raw.TryGetValue("payTransNo", out var ptn) ? ptn : null,
            RawParams  = raw
        };
    }

    /// <inheritdoc/>
    public async Task<UnionPayCustomsQueryResponse> QueryDeclarationAsync(UnionPayCustomsQueryRequest request, CancellationToken ct = default)
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

        return new UnionPayCustomsQueryResponse
        {
            RespCode     = GetValue(raw, "respCode"),
            RespMsg      = GetValue(raw, "respMsg"),
            OrderId      = GetValue(raw, "orderId"),
            OrigRespCode = GetValue(raw, "origRespCode"),
            OrigRespMsg  = GetValue(raw, "origRespMsg"),
            RawParams    = raw
        };
    }

    /// <inheritdoc/>
    public async Task<UnionPayEncryptKeyQueryResponse> QueryEncryptKeyAsync(UnionPayEncryptKeyQueryRequest request, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["txnType"]    = request.TxnType,
            ["txnSubType"] = request.TxnSubType,
            ["bizType"]    = request.BizType,
            ["orderId"]    = request.OrderId,
            ["txnTime"]    = request.TxnTime,
            ["certType"]   = request.CertType
        };

        var raw = await _http.PostBackAsync(parameters, _options.BackGatewayUrl, ct);
        ValidateResponse(raw);

        return new UnionPayEncryptKeyQueryResponse
        {
            RespCode       = GetValue(raw, "respCode"),
            RespMsg        = GetValue(raw, "respMsg"),
            SignPubKeyCert = raw.TryGetValue("signPubKeyCert", out var cert) ? cert : null,
            CertType       = raw.TryGetValue("certType", out var ct2) ? ct2 : null,
            RawParams      = raw
        };
    }

    /// <inheritdoc/>
    public async Task<UnionPayRealNameAuthResponse> RealNameAuthAsync(UnionPayRealNameAuthRequest request, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["txnType"]    = request.TxnType,
            ["txnSubType"] = request.TxnSubType,
            ["bizType"]    = request.BizType,
            ["orderId"]    = request.OrderId,
            ["txnTime"]    = request.TxnTime,
            ["accNo"]      = request.AccNo,
            ["txnAmt"]     = request.TxnAmt
        };

        if (!string.IsNullOrEmpty(request.CustomerInfo))
            parameters["customerInfo"] = request.CustomerInfo;
        if (!string.IsNullOrEmpty(request.ReqReserved))
            parameters["reqReserved"] = request.ReqReserved;

        var raw = await _http.PostBackAsync(parameters, _options.BackGatewayUrl, ct);
        ValidateResponse(raw);

        return new UnionPayRealNameAuthResponse
        {
            RespCode  = GetValue(raw, "respCode"),
            RespMsg   = GetValue(raw, "respMsg"),
            OrderId   = raw.TryGetValue("orderId", out var oid) ? oid : null,
            TraceNo   = raw.TryGetValue("traceNo", out var tn) ? tn : null,
            TraceTime = raw.TryGetValue("traceTime", out var tt) ? tt : null,
            RawParams = raw
        };
    }

    /// <inheritdoc/>
    public async Task<UnionPayFileTransferResponse> FileTransferAsync(UnionPayFileTransferRequest request, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["txnType"]    = request.TxnType,
            ["txnSubType"] = request.TxnSubType,
            ["bizType"]    = request.BizType,
            ["orderId"]    = request.OrderId,
            ["txnTime"]    = request.TxnTime,
            ["settleDate"] = request.SettleDate,
            ["fileType"]   = request.FileType
        };

        var raw = await _http.PostBackAsync(parameters, _options.FileGatewayUrl, ct);
        ValidateResponse(raw);

        string? fileContent = null;
        byte[]? fileData = null;

        if (raw.TryGetValue("fileContent", out var fc) && !string.IsNullOrEmpty(fc))
        {
            fileData = Convert.FromBase64String(fc);
            fileContent = TryInflate(fileData);
        }

        return new UnionPayFileTransferResponse
        {
            RespCode    = GetValue(raw, "respCode"),
            RespMsg     = GetValue(raw, "respMsg"),
            FileContent = fileContent,
            FileData    = fileData,
            RawParams   = raw
        };
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

    /// <summary>
    /// 尝试使用 Deflate 解压缩文件内容，如失败则按 UTF-8 直接解码
    /// </summary>
    private static string TryInflate(byte[] data)
    {
        try
        {
            using var input = new MemoryStream(data);
            using var deflate = new DeflateStream(input, CompressionMode.Decompress);
            using var output = new MemoryStream();
            deflate.CopyTo(output);
            return Encoding.UTF8.GetString(output.ToArray());
        }
        catch
        {
            return Encoding.UTF8.GetString(data);
        }
    }
}
