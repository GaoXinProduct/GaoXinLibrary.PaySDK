using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using GaoXinLibrary.PaySDK.Alipay.Core;
using GaoXinLibrary.PaySDK.Alipay.Models;
using GaoXinLibrary.PaySDK.UnionPay.Core;
using GaoXinLibrary.PaySDK.UnionPay.OpenApi;
using GaoXinLibrary.PaySDK.Wechat.Core;
using GaoXinLibrary.PaySDK.Wechat.Models;
using Xunit;

namespace GaoXinLibrary.PaySDK.Tests;

public class PaymentSecurityRegressionTests
{
    [Fact]
    public async Task AlipayGatewayResponse_MissingSignature_Throws()
    {
        using var keys = new RsaKeys();
        var response = "{\"alipay_trade_query_response\":{\"code\":\"10000\",\"msg\":\"Success\",\"out_trade_no\":\"order-1\"}}";
        var client = CreateAlipayClient(keys, response);

        await Assert.ThrowsAsync<AlipayException>(() =>
            client.ExecuteAsync<AlipayTradeQueryResponse>("alipay.trade.query", new AlipayTradeQueryContent { OutTradeNo = "order-1" }));
    }

    [Fact]
    public async Task AlipayGatewayResponse_TamperedSignature_Throws()
    {
        using var keys = new RsaKeys();
        var signedContent = "{\"code\":\"10000\",\"msg\":\"Success\",\"out_trade_no\":\"order-1\"}";
        var tamperedContent = "{\"code\":\"10000\",\"msg\":\"Success\",\"out_trade_no\":\"order-2\"}";
        var response = BuildAlipayResponse(tamperedContent, keys.SignBase64(signedContent));
        var client = CreateAlipayClient(keys, response);

        await Assert.ThrowsAsync<AlipayException>(() =>
            client.ExecuteAsync<AlipayTradeQueryResponse>("alipay.trade.query", new AlipayTradeQueryContent { OutTradeNo = "order-2" }));
    }

    [Fact]
    public async Task UnionPayAcpResponse_MissingSignature_Throws()
    {
        using var keys = new RsaKeys();
        var client = CreateUnionPayClient(keys, "respCode=00&respMsg=Success");

        await Assert.ThrowsAsync<UnionPayException>(() =>
            client.PostBackAsync(new Dictionary<string, string>(), "https://gateway.test/unionpay"));
    }

    [Fact]
    public async Task UnionPayAcpResponse_TamperedSignature_Throws()
    {
        using var keys = new RsaKeys();
        var signedFields = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["respCode"] = "00",
            ["orderId"] = "order1"
        };
        var tamperedFields = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["respCode"] = "00",
            ["orderId"] = "order2",
            ["signature"] = keys.SignHashBase64(UnionPaySigner.BuildSignContent(signedFields))
        };
        var client = CreateUnionPayClient(keys, ToForm(tamperedFields));

        await Assert.ThrowsAsync<UnionPayException>(() =>
            client.PostBackAsync(new Dictionary<string, string>(), "https://gateway.test/unionpay"));
    }

    [Fact]
    public void WechatPaySignature_MissingSerial_ReturnsFalse()
    {
        using var keys = new RsaKeys();
        var signer = CreateWechatSigner(keys);
        var headers = BuildWechatHeaders(keys, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), "nonce", "{}");
        headers.Serial = null;

        Assert.False(signer.VerifySignature("{}", headers));
    }

    [Fact]
    public void WechatPaySignature_StaleTimestamp_ReturnsFalse()
    {
        using var keys = new RsaKeys();
        var signer = CreateWechatSigner(keys);
        var stale = DateTimeOffset.UtcNow.AddMinutes(-10).ToUnixTimeSeconds().ToString();
        var headers = BuildWechatHeaders(keys, stale, "nonce", "{}");

        Assert.False(signer.VerifySignature("{}", headers));
    }

    [Fact]
    public void WechatPayCallbackHeaders_FromHeaders_IsCaseInsensitive()
    {
        var headers = WechatPayCallbackHeaders.FromHeaders(new Dictionary<string, string>
        {
            ["wechatpay-timestamp"] = "1",
            ["wechatpay-nonce"] = "nonce",
            ["wechatpay-signature"] = "signature",
            ["wechatpay-serial"] = "serial"
        });

        Assert.Equal("1", headers.Timestamp);
        Assert.Equal("nonce", headers.Nonce);
        Assert.Equal("signature", headers.Signature);
        Assert.Equal("serial", headers.Serial);
    }

    [Fact]
    public async Task UnionPayOpenApiOAuth2_SendsOpenHeadersAndSignature()
    {
        HttpRequestMessage? captured = null;
        string? capturedBody = null;
        using var handler = new FakeHttpMessageHandler(request =>
        {
            captured = request;
            capturedBody = request.Content?.ReadAsStringAsync().GetAwaiter().GetResult();
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{}", Encoding.UTF8, "application/json")
            };
        });
        var service = new UnionPayOpenApiService(new HttpClient(handler), new UnionPayOpenApiOptions
        {
            AppId = "app-id",
            AuthMode = UnionPayOpenApiAuthMode.OAuth2,
            OAuthToken = "token-value",
            OAuthSignatureKey = "signature-secret"
        });

        await service.PostAsync("cardbin.cardinfo", new { cardNo = "6214830215395277" });

        Assert.NotNull(captured);
        Assert.Null(captured!.Headers.Authorization);
        Assert.True(captured.Headers.TryGetValues("X-OPEN-TOKEN", out var tokenValues));
        Assert.True(captured.Headers.TryGetValues("X-OPEN-TS", out var timestampValues));
        Assert.True(captured.Headers.TryGetValues("X-OPEN-SIGN", out var signatureValues));
        Assert.Equal("token-value", tokenValues!.Single());
        var timestamp = timestampValues!.Single();
        var expectedSignature = Sha256Hex("signature-secret" + capturedBody + timestamp);
        Assert.Equal(expectedSignature, signatureValues!.Single());
    }

    private static AlipayHttpClient CreateAlipayClient(RsaKeys keys, string response)
    {
        var options = new AlipayOptions
        {
            AppId = "app-id",
            PrivateKey = keys.PrivateKeyPem,
            AlipayPublicKey = keys.PublicKeyPem,
            GatewayUrl = "https://openapi.alipay.test/gateway.do"
        };
        var handler = new FakeHttpMessageHandler(_ => new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(response, Encoding.UTF8, "application/json")
        });
        return new AlipayHttpClient(new HttpClient(handler), options, new AlipaySigner(options));
    }

    private static UnionPayHttpClient CreateUnionPayClient(RsaKeys keys, string response)
    {
        var options = new UnionPayOptions
        {
            MerId = "777290058110048",
            CertId = "cert-id",
            PrivateKey = keys.PrivateKeyPem,
            UnionPayPublicKey = keys.PublicKeyPem
        };
        var handler = new FakeHttpMessageHandler(_ => new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(response, Encoding.UTF8, "text/plain")
        });
        return new UnionPayHttpClient(new HttpClient(handler), options, new UnionPaySigner(options));
    }

    private static WechatPaySigner CreateWechatSigner(RsaKeys keys)
    {
        return new WechatPaySigner(new WechatPayOptions
        {
            AppId = "wx-app",
            MchId = "mch-id",
            ApiV3Key = "0123456789abcdef0123456789abcdef",
            PrivateKey = keys.PrivateKeyPem,
            CertSerialNo = "merchant-cert",
            PlatformPublicKey = keys.PublicKeyPem,
            PlatformPublicKeyId = "PUB_KEY_ID_test"
        });
    }

    private static WechatPayCallbackHeaders BuildWechatHeaders(RsaKeys keys, string timestamp, string nonce, string body)
    {
        return new WechatPayCallbackHeaders
        {
            Timestamp = timestamp,
            Nonce = nonce,
            Signature = keys.SignBase64($"{timestamp}\n{nonce}\n{body}\n"),
            Serial = "PUB_KEY_ID_test"
        };
    }

    private static string BuildAlipayResponse(string responseContent, string signature)
        => "{\"alipay_trade_query_response\":" + responseContent + ",\"sign\":" + JsonSerializer.Serialize(signature) + "}";

    private static string ToForm(Dictionary<string, string> fields)
        => string.Join("&", fields.Select(kv => $"{Uri.EscapeDataString(kv.Key)}={Uri.EscapeDataString(kv.Value)}"));

    private static string Sha256Hex(string text)
        => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(text))).ToLowerInvariant();

    private sealed class FakeHttpMessageHandler(Func<HttpRequestMessage, HttpResponseMessage> handler) : HttpMessageHandler, IDisposable
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            => Task.FromResult(handler(request));
    }

    private sealed class RsaKeys : IDisposable
    {
        private readonly RSA _rsa = RSA.Create(2048);

        public string PrivateKeyPem => _rsa.ExportPkcs8PrivateKeyPem();

        public string PublicKeyPem => _rsa.ExportSubjectPublicKeyInfoPem();

        public string SignBase64(string content)
            => Convert.ToBase64String(_rsa.SignData(Encoding.UTF8.GetBytes(content), HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1));

        public string SignHashBase64(string content)
            => Convert.ToBase64String(_rsa.SignHash(SHA256.HashData(Encoding.UTF8.GetBytes(content)), HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1));

        public void Dispose() => _rsa.Dispose();
    }
}
