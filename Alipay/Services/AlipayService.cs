using System.Text;
using System.Web;
using GaoXinLibrary.PaySDK.Alipay.Core;
using GaoXinLibrary.PaySDK.Alipay.Models;
using Microsoft.Extensions.Primitives;

namespace GaoXinLibrary.PaySDK.Alipay.Services;

/// <summary>
/// 支付宝支付服务实现
/// </summary>
public sealed class AlipayService : IAlipayService
{
    private readonly AlipayHttpClient _http;
    private readonly AlipayOptions _options;
    private readonly AlipaySigner _signer;

    /// <summary>
    /// 初始化支付宝支付服务
    /// </summary>
    public AlipayService(AlipayHttpClient http, AlipayOptions options, AlipaySigner signer)
    {
        _http = http;
        _options = options;
        _signer = signer;
    }

    /// <inheritdoc/>
    public async Task<AlipayTradePayResponse> FaceToFacePayAsync(AlipayTradePayBizContent bizContent, CancellationToken ct = default)
        => await _http.ExecuteAsync<AlipayTradePayResponse>("alipay.trade.pay", bizContent, ct);

    /// <inheritdoc/>
    public async Task<AlipayTradeCreateResponse> CreateOrderAsync(AlipayTradeCreateContent bizContent, string? notifyUrl = null, CancellationToken ct = default)
    {
        var resolved = !string.IsNullOrEmpty(notifyUrl) ? notifyUrl : _options.NotifyUrl;
        Dictionary<string, string>? extra = !string.IsNullOrEmpty(resolved)
            ? new Dictionary<string, string>(StringComparer.Ordinal) { ["notify_url"] = resolved }
            : null;
        return await _http.ExecuteAsync<AlipayTradeCreateResponse>("alipay.trade.create", bizContent, ct, extra);
    }

    /// <inheritdoc/>
    public async Task<AlipayTradePrecreateResponse> PrecreateAsync(AlipayTradePrecreateContent bizContent, string? notifyUrl = null, CancellationToken ct = default)
    {
        var resolved = !string.IsNullOrEmpty(notifyUrl) ? notifyUrl : _options.NotifyUrl;
        Dictionary<string, string>? extra = !string.IsNullOrEmpty(resolved)
            ? new Dictionary<string, string>(StringComparer.Ordinal) { ["notify_url"] = resolved }
            : null;
        return await _http.ExecuteAsync<AlipayTradePrecreateResponse>("alipay.trade.precreate", bizContent, ct, extra);
    }

    /// <inheritdoc/>
    public string BuildAppPayString(AlipayTradeAppPayContent bizContent, string? notifyUrl = null)
    {
        var resolved = !string.IsNullOrEmpty(notifyUrl) ? notifyUrl : _options.NotifyUrl;
        var extra = new Dictionary<string, string>(StringComparer.Ordinal);
        if (!string.IsNullOrEmpty(resolved))
            extra["notify_url"] = resolved;
        return _http.BuildSdkStringWithExtra("alipay.trade.app.pay", bizContent, extra);
    }

    /// <inheritdoc/>
    public string BuildWapPayUrl(AlipayTradeWapPayContent bizContent, string? notifyUrl = null, string? returnUrl = null)
    {
        var resolvedNotify = !string.IsNullOrEmpty(notifyUrl) ? notifyUrl : _options.NotifyUrl;
        var resolvedReturn = !string.IsNullOrEmpty(returnUrl) ? returnUrl : _options.ReturnUrl;
        var extra = new Dictionary<string, string>(StringComparer.Ordinal);
        if (!string.IsNullOrEmpty(resolvedNotify))
            extra["notify_url"] = resolvedNotify;
        if (!string.IsNullOrEmpty(resolvedReturn))
            extra["return_url"] = resolvedReturn;

        return _http.BuildPageFormWithExtra("alipay.trade.wap.pay", bizContent, extra);
    }

    /// <inheritdoc/>
    public string BuildPagePayUrl(AlipayTradePagePayContent bizContent, string? notifyUrl = null, string? returnUrl = null)
    {
        var resolvedNotify = !string.IsNullOrEmpty(notifyUrl) ? notifyUrl : _options.NotifyUrl;
        var resolvedReturn = !string.IsNullOrEmpty(returnUrl) ? returnUrl : _options.ReturnUrl;
        var extra = new Dictionary<string, string>(StringComparer.Ordinal);
        if (!string.IsNullOrEmpty(resolvedNotify))
            extra["notify_url"] = resolvedNotify;
        if (!string.IsNullOrEmpty(resolvedReturn))
            extra["return_url"] = resolvedReturn;

        return _http.BuildPageFormWithExtra("alipay.trade.page.pay", bizContent, extra);
    }

    /// <inheritdoc/>
    public async Task<AlipayTradeCancelResponse> CancelOrderAsync(AlipayTradeCancelContent bizContent, CancellationToken ct = default)
        => await _http.ExecuteAsync<AlipayTradeCancelResponse>("alipay.trade.cancel", bizContent, ct);

    /// <inheritdoc/>
    public async Task<AlipayTradeCloseResponse> CloseOrderAsync(AlipayTradeCloseContent bizContent, bool ignoreNotExist = false, CancellationToken ct = default)
    {
        try
        {
            return await _http.ExecuteAsync<AlipayTradeCloseResponse>("alipay.trade.close", bizContent, ct);
        }
        catch (Core.AlipayException ex) when (ignoreNotExist && ex.ErrorCode == "ACQ.TRADE_NOT_EXIST")
        {
            // 调用方明确声明可忽略"交易不存在"（如 App/网站/H5 支付用户从未跳转支付宝的场景）
            return new AlipayTradeCloseResponse
            {
                Code       = "10000",
                Msg        = "Success",
                OutTradeNo = bizContent.OutTradeNo,
                TradeNo    = bizContent.TradeNo
            };
        }
    }

    /// <inheritdoc/>
    public async Task<AlipayTradeQueryResponse> QueryOrderAsync(AlipayTradeQueryContent bizContent, CancellationToken ct = default)
        => await _http.ExecuteAsync<AlipayTradeQueryResponse>("alipay.trade.query", bizContent, ct);

    /// <inheritdoc/>
    public async Task<AlipayTradeRefundResponse> RefundAsync(AlipayTradeRefundContent bizContent, CancellationToken ct = default)
        => await _http.ExecuteAsync<AlipayTradeRefundResponse>("alipay.trade.refund", bizContent, ct);

    /// <inheritdoc/>
    public async Task<AlipayTradeRefundQueryResponse> QueryRefundAsync(AlipayTradeRefundQueryContent bizContent, CancellationToken ct = default)
        => await _http.ExecuteAsync<AlipayTradeRefundQueryResponse>("alipay.trade.fastpay.refund.query", bizContent, ct);

    /// <inheritdoc/>
    public async Task<byte[]> DownloadBillAsync(AlipayBillDownloadContent bizContent, CancellationToken ct = default)
    {
        var resp = await _http.ExecuteAsync<AlipayBillDownloadResponse>("alipay.data.dataservice.bill.downloadurl.query", bizContent, ct);

        if (string.IsNullOrEmpty(resp.BillDownloadUrl))
            throw new Core.AlipayException("EMPTY_BILL_URL", "支付宝账单下载地址为空");

        return await _http.DownloadBytesAsync(resp.BillDownloadUrl, ct);
    }

    /// <inheritdoc/>
    public AlipayCallbackParams ParseCallback(IDictionary<string, string> formParams)
    {
        var raw = new Dictionary<string, string>(formParams, StringComparer.Ordinal);

        var sign = raw.TryGetValue("sign", out var s) ? s : string.Empty;
        var signType = raw.TryGetValue("sign_type", out var st) ? st : "RSA2";

        var signContent = BuildCallbackSignContent(raw);
        var isValid = _signer.Verify(signContent, sign);

        var result = new AlipayCallbackParams
        {
            IsValid = isValid,
            NotifyTime = GetValue(raw, "notify_time"),
            NotifyType = GetValue(raw, "notify_type"),
            NotifyId = GetValue(raw, "notify_id"),
            AppId = GetValue(raw, "app_id"),
            Charset = GetValue(raw, "charset", "utf-8"),
            Version = GetValue(raw, "version", "1.0"),
            SignType = signType,
            Sign = sign,
            TradeNo = GetValue(raw, "trade_no"),
            OutTradeNo = GetValue(raw, "out_trade_no"),
            OutBizNo = raw.TryGetValue("out_biz_no", out var obn) ? obn : null,
            BuyerLogonId = raw.TryGetValue("buyer_logon_id", out var bli) ? bli : null,
            BuyerUserId = raw.TryGetValue("buyer_user_id", out var bui) ? bui : null,
            TradeStatus = GetValue(raw, "trade_status"),
            TotalAmount = GetValue(raw, "total_amount"),
            BuyerPayAmount = raw.TryGetValue("buyer_pay_amount", out var bpa) ? bpa : null,
            GmtCreate = raw.TryGetValue("gmt_create", out var gc) ? gc : null,
            GmtPayment = raw.TryGetValue("gmt_payment", out var gp) ? gp : null,
            GmtRefund = raw.TryGetValue("gmt_refund", out var gr) ? gr : null,
            GmtClose = raw.TryGetValue("gmt_close", out var gcl) ? gcl : null,
            RawParams = raw
        };

        return result;
    }

    private static string BuildCallbackSignContent(Dictionary<string, string> parameters)
    {
        var sb = new StringBuilder();
        foreach (var kv in parameters.OrderBy(x => x.Key, StringComparer.Ordinal))
        {
            if (kv.Key == "sign" || kv.Key == "sign_type") continue;
            if (string.IsNullOrEmpty(kv.Value)) continue;
            if (sb.Length > 0) sb.Append('&');
            sb.Append(kv.Key).Append('=').Append(kv.Value);
        }
        return sb.ToString();
    }

    /// <inheritdoc/>
    public AlipayCallbackParams ParseCallback(IEnumerable<KeyValuePair<string, StringValues>> formParams)
        => ParseCallback(formParams.ToDictionary(kv => kv.Key, kv => kv.Value.ToString()));

    private static string GetValue(Dictionary<string, string> dict, string key, string defaultValue = "")
        => dict.TryGetValue(key, out var v) ? v : defaultValue;
}
