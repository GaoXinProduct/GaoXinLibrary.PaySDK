using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using GaoXinLibrary.PaySDK.Core;
using GaoXinLibrary.PaySDK.Wechat.Core;
using GaoXinLibrary.PaySDK.Wechat.Models;

namespace GaoXinLibrary.PaySDK.Wechat.Services;

/// <summary>
/// 微信支付服务实现
/// </summary>
public sealed class WechatPayService : IWechatPayService
{
    private readonly WechatPayHttpClient _http;
    private readonly WechatPayOptions _options;
    private readonly WechatPaySigner _signer;

    /// <summary>
    /// 初始化微信支付服务
    /// </summary>
    public WechatPayService(WechatPayHttpClient http, WechatPayOptions options, WechatPaySigner signer)
    {
        _http = http;
        _options = options;
        _signer = signer;
    }

    /// <inheritdoc/>
    public async Task<WechatJsapiOrderResponse> CreateJsapiOrderAsync(WechatJsapiOrderRequest request, CancellationToken ct = default)
    {
        FillCommonFields(request);
        return await _http.PostAsync<WechatJsapiOrderResponse>("/v3/pay/transactions/jsapi", request, ct);
    }

    /// <inheritdoc/>
    public async Task<WechatAppOrderResponse> CreateAppOrderAsync(WechatAppOrderRequest request, CancellationToken ct = default)
    {
        FillCommonFields(request);
        return await _http.PostAsync<WechatAppOrderResponse>("/v3/pay/transactions/app", request, ct);
    }

    /// <inheritdoc/>
    public async Task<WechatH5OrderResponse> CreateH5OrderAsync(WechatH5OrderRequest request, CancellationToken ct = default)
    {
        FillCommonFields(request);
        FillH5SceneFields(request);
        request.ValidateH5Fields();
        // redirect_url 必须携带，优先使用请求级别配置，其次使用全局配置
        var redirectUrl = !string.IsNullOrEmpty(request.RedirectUrl)
            ? request.RedirectUrl
            : _options.H5RedirectUrl;

        if (string.IsNullOrEmpty(redirectUrl))
            throw new ArgumentException(
                "H5 下单必须设置 RedirectUrl（支付完成后跳转地址），请通过 WechatH5OrderRequest.RedirectUrl 或 WechatPayOptions.H5RedirectUrl 配置",
                nameof(request));

        var resp = await _http.PostAsync<WechatH5OrderResponse>("/v3/pay/transactions/h5", request, ct);

        // 拼接 redirect_url：支付完成后跳转指定页面
        if (!string.IsNullOrEmpty(resp.H5Url))
        {
            var separator = resp.H5Url.Contains('?') ? "&" : "?";
            resp.H5Url = $"{resp.H5Url}{separator}redirect_url={Uri.EscapeDataString(redirectUrl)}";
        }

        return resp;
    }

    /// <inheritdoc/>
    public async Task<WechatNativeOrderResponse> CreateNativeOrderAsync(WechatNativeOrderRequest request, CancellationToken ct = default)
    {
        FillCommonFields(request);
        return await _http.PostAsync<WechatNativeOrderResponse>("/v3/pay/transactions/native", request, ct);
    }

    /// <inheritdoc/>
    public async Task<WechatMiniProgramOrderResponse> CreateMiniProgramOrderAsync(WechatMiniProgramOrderRequest request, CancellationToken ct = default)
    {
        FillCommonFields(request);
        return await _http.PostAsync<WechatMiniProgramOrderResponse>("/v3/pay/transactions/jsapi", request, ct);
    }

    /// <inheritdoc/>
    public async Task CloseOrderAsync(string outTradeNo, CancellationToken ct = default)
    {
        var body = new { mchid = _options.MchId };
        await _http.PostNoContentAsync($"/v3/pay/transactions/out-trade-no/{Uri.EscapeDataString(outTradeNo)}/close", body, ct);
    }

    /// <inheritdoc/>
    public async Task<WechatQueryOrderResponse> QueryOrderByOutTradeNoAsync(string outTradeNo, CancellationToken ct = default)
    {
        return await _http.GetAsync<WechatQueryOrderResponse>($"/v3/pay/transactions/out-trade-no/{Uri.EscapeDataString(outTradeNo)}?mchid={_options.MchId}", ct);
    }

    /// <inheritdoc/>
    public async Task<WechatQueryOrderResponse> QueryOrderByTransactionIdAsync(string transactionId, CancellationToken ct = default)
    {
        return await _http.GetAsync<WechatQueryOrderResponse>($"/v3/pay/transactions/id/{Uri.EscapeDataString(transactionId)}?mchid={_options.MchId}", ct);
    }

    /// <inheritdoc/>
    public async Task<WechatRefundResponse> RefundAsync(WechatRefundRequest request, CancellationToken ct = default)
    {
        if (string.IsNullOrEmpty(request.NotifyUrl) && !string.IsNullOrEmpty(_options.RefundNotifyUrl))
            request.NotifyUrl = _options.RefundNotifyUrl;
        return await _http.PostAsync<WechatRefundResponse>("/v3/refund/domestic/refunds", request, ct);
    }

    /// <inheritdoc/>
    public async Task<WechatRefundQueryResponse> QueryRefundAsync(string outRefundNo, CancellationToken ct = default)
    {
        return await _http.GetAsync<WechatRefundQueryResponse>($"/v3/refund/domestic/refunds/{Uri.EscapeDataString(outRefundNo)}", ct);
    }

    /// <inheritdoc/>
    public async Task<byte[]> DownloadTradeBillAsync(string billDate, string billType = "ALL", CancellationToken ct = default)
    {
        var urlResp = await _http.GetAsync<WechatBillDownloadUrlResponse>(
            $"/v3/bill/tradebill?bill_date={Uri.EscapeDataString(billDate)}&bill_type={Uri.EscapeDataString(billType)}", ct);
        return await _http.GetBytesAsync(ExtractPath(urlResp.DownloadUrl), ct);
    }

    /// <inheritdoc/>
    public async Task<byte[]> DownloadFundFlowBillAsync(string billDate, string accountType = "BASIC", CancellationToken ct = default)
    {
        var urlResp = await _http.GetAsync<WechatBillDownloadUrlResponse>(
            $"/v3/bill/fundflowbill?bill_date={Uri.EscapeDataString(billDate)}&account_type={Uri.EscapeDataString(accountType)}", ct);
        return await _http.GetBytesAsync(ExtractPath(urlResp.DownloadUrl), ct);
    }

    /// <inheritdoc/>
    public Task<WechatPayCallbackDecrypted> ParsePayCallbackAsync(
        string requestBody,
        WechatPayCallbackHeaders headers,
        CancellationToken ct = default)
    {
        return Task.FromResult(
            VerifyAndDecrypt<WechatPayCallbackDecrypted>(requestBody, headers, "支付"));
    }

    /// <inheritdoc/>
    public Task<WechatRefundCallbackDecrypted> ParseRefundCallbackAsync(
        string requestBody,
        WechatPayCallbackHeaders headers,
        CancellationToken ct = default)
    {
        return Task.FromResult(
            VerifyAndDecrypt<WechatRefundCallbackDecrypted>(requestBody, headers, "退款"));
    }

    /// <summary>
    /// 回调通知通用处理：验签 → 解密 → 反序列化
    /// </summary>
    private T VerifyAndDecrypt<T>(string requestBody, WechatPayCallbackHeaders headers, string scene)
    {
        var callbackBody = JsonSerializer.Deserialize<WechatPayCallbackBody>(requestBody, WechatPayHttpClient.JsonOptions)
            ?? throw new PayException("INVALID_CALLBACK", $"解析微信支付{scene}回调失败", null);

        var valid = _signer.VerifySignature(requestBody, headers);
        if (!valid)
            throw new PayException("SIGNATURE_INVALID", $"微信支付{scene}回调签名验证失败", null);

        var resource = callbackBody.Resource;
        var plaintext = _signer.DecryptCallback(
            resource.AssociatedData ?? string.Empty,
            resource.Nonce,
            resource.Ciphertext);

        return JsonSerializer.Deserialize<T>(plaintext, WechatPayHttpClient.JsonOptions)
            ?? throw new PayException("DECRYPT_FAILED", $"微信支付{scene}回调解密失败", null);
    }

    /// <inheritdoc/>
    public WechatJsPayParams BuildJsPayParams(string prepayId)
    {
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
        var nonceStr = GenerateNonce();
        var paySign = _signer.BuildJsPaySign(_options.AppId, timestamp, nonceStr, prepayId);

        return new WechatJsPayParams
        {
            AppId = _options.AppId,
            TimeStamp = timestamp,
            NonceStr = nonceStr,
            Package = $"prepay_id={prepayId}",
            SignType = "RSA",
            PaySign = paySign
        };
    }

    /// <inheritdoc/>
    public WechatAppPayParams BuildAppPayParams(string prepayId)
    {
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
        var nonceStr = GenerateNonce();
        var message = $"{_options.AppId}\n{timestamp}\n{nonceStr}\n{prepayId}\n";
        var sign = _signer.Sign(message);

        return new WechatAppPayParams
        {
            AppId = _options.AppId,
            PartnerId = _options.MchId,
            PrepayId = prepayId,
            Package = "Sign=WXPay",
            NonceStr = nonceStr,
            Timestamp = timestamp,
            Sign = sign
        };
    }

    private void FillCommonFields(WechatCreateOrderRequestBase request)
    {
        if (string.IsNullOrEmpty(request.AppId))
            request.AppId = _options.AppId;
        if (string.IsNullOrEmpty(request.MchId))
            request.MchId = _options.MchId;
        if (string.IsNullOrEmpty(request.NotifyUrl))
            request.NotifyUrl = _options.NotifyUrl;
    }

    private void FillH5SceneFields(WechatH5OrderRequest request)
    {
        request.SceneInfo ??= new WechatPaySceneInfo();
        request.SceneInfo.H5Info ??= new WechatPayH5Info();
    }

    private static string GenerateNonce()
    {
        var bytes = new byte[16];
        RandomNumberGenerator.Fill(bytes);
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }

    private static string ExtractPath(string downloadUrl)
    {
        if (Uri.TryCreate(downloadUrl, UriKind.Absolute, out var uri))
            return uri.PathAndQuery;
        return downloadUrl;
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<(string SerialNo, string CertificatePem)>> DownloadCertificatesAsync(CancellationToken ct = default)
    {
        var resp = await _http.GetAsync<WechatCertificatesResponse>("/v3/certificates", ct);

        var result = new List<(string SerialNo, string CertificatePem)>(resp.Data.Count);
        foreach (var item in resp.Data)
        {
            var enc = item.EncryptCertificate;
            var pem = _signer.DecryptCallback(
                enc.AssociatedData ?? "certificate",
                enc.Nonce,
                enc.Ciphertext);
            _signer.RegisterCertificate(item.SerialNo, pem);
            result.Add((item.SerialNo, pem));
        }
        return result;
    }

    /// <inheritdoc/>
    public void RegisterCertificate(string serialNo, string certificatePem)
        => _signer.RegisterCertificate(serialNo, certificatePem);

    /// <inheritdoc/>
    public async Task<WechatAbnormalRefundResponse> ApplyAbnormalRefundAsync(WechatAbnormalRefundRequest request, CancellationToken ct = default)
    {
        if (!string.IsNullOrEmpty(request.BankAccount))
            request.BankAccount = _signer.EncryptSensitiveField(request.BankAccount);
        if (!string.IsNullOrEmpty(request.RealName))
            request.RealName = _signer.EncryptSensitiveField(request.RealName);

        var refundId = Uri.EscapeDataString(request.RefundId);
        return await _http.PostWithEncryptionAsync<WechatAbnormalRefundResponse>(
            $"/v3/refund/domestic/refunds/{refundId}/apply-abnormal-refund", request, ct);
    }

    /// <inheritdoc/>
    public string EncryptSensitiveField(string plainText)
        => _signer.EncryptSensitiveField(plainText);

    /// <inheritdoc/>
    public string DecryptSensitiveField(string cipherText)
        => _signer.DecryptSensitiveField(cipherText);
}
