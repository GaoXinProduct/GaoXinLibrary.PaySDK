using System.Security.Cryptography;
using System.Text;
using GaoXinLibrary.PaySDK.Alipay.Core;
using GaoXinLibrary.PaySDK.UnionPay.Core;
using GaoXinLibrary.PaySDK.Wechat.Core;
using GaoXinLibrary.PaySDK.Wechat.Models;
using Xunit;

namespace GaoXinLibrary.PaySDK.Tests;

public class SignerTests
{
    [Fact]
    public void AlipaySigner_Sign_ProducesVerifiableSignature()
    {
        var (privatePem, publicPem) = TestKeyHelper.GenerateRsaKeyPair();
        var options = new AlipayOptions
        {
            AppId = "test_app",
            PrivateKey = privatePem,
            AlipayPublicKey = publicPem
        };
        var signer = new AlipaySigner(options);
        var content = "app_id=test_app&method=alipay.trade.pay&charset=utf-8";
        var signature = signer.Sign(content);
        Assert.NotNull(signature);
        Assert.NotEmpty(signature);
        var sigBytes = Convert.FromBase64String(signature);
        using var rsa = RSA.Create();
        rsa.ImportFromPem(publicPem);
        var valid = rsa.VerifyData(Encoding.UTF8.GetBytes(content), sigBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        Assert.True(valid);
    }

    [Fact]
    public void AlipaySigner_Verify_ValidSignature_ReturnsTrue()
    {
        var (privatePem, publicPem) = TestKeyHelper.GenerateRsaKeyPair();
        var options = new AlipayOptions
        {
            AppId = "test_app",
            PrivateKey = privatePem,
            AlipayPublicKey = publicPem
        };
        var signer = new AlipaySigner(options);
        var content = "notify_id=abc123&total_amount=1.00&trade_no=2025010100001";
        var signature = signer.Sign(content);
        var verified = signer.Verify(content, signature);
        Assert.True(verified);
    }

    [Fact]
    public void AlipaySigner_Verify_TamperedContent_ReturnsFalse()
    {
        var (privatePem, publicPem) = TestKeyHelper.GenerateRsaKeyPair();
        var options = new AlipayOptions
        {
            AppId = "test_app",
            PrivateKey = privatePem,
            AlipayPublicKey = publicPem
        };
        var signer = new AlipaySigner(options);
        var content = "amount=1.00";
        var signature = signer.Sign(content);
        var verified = signer.Verify("amount=99.99", signature);
        Assert.False(verified);
    }

    [Fact]
    public void AlipaySigner_Verify_EmptyStringContent()
    {
        var (privatePem, publicPem) = TestKeyHelper.GenerateRsaKeyPair();
        var options = new AlipayOptions
        {
            AppId = "test_app",
            PrivateKey = privatePem,
            AlipayPublicKey = publicPem
        };
        var signer = new AlipaySigner(options);
        var emptyContent = "";
        var signature = signer.Sign(emptyContent);
        var verified = signer.Verify(emptyContent, signature);
        Assert.True(verified);
    }

    [Fact]
    public void AlipaySigner_Verify_InvalidBase64Signature_ReturnsFalse()
    {
        var (privatePem, publicPem) = TestKeyHelper.GenerateRsaKeyPair();
        var options = new AlipayOptions
        {
            AppId = "test_app",
            PrivateKey = privatePem,
            AlipayPublicKey = publicPem
        };
        var signer = new AlipaySigner(options);
        var verified = signer.Verify("some content", "!!!not-valid-base64!!!");
        Assert.False(verified);
    }

    [Fact]
    public void AlipaySigner_Verify_WrongKey_ReturnsFalse()
    {
        var (privatePem1, publicPem1) = TestKeyHelper.GenerateRsaKeyPair();
        var (privatePem2, publicPem2) = TestKeyHelper.GenerateRsaKeyPair();
        var signOptions = new AlipayOptions
        {
            AppId = "test_app",
            PrivateKey = privatePem1,
            AlipayPublicKey = publicPem2
        };
        var signer = new AlipaySigner(signOptions);
        var content = "test content";
        var signature = signer.Sign(content);
        var verified = signer.Verify(content, signature);
        Assert.False(verified);
    }

    [Fact]
    public void AlipaySigner_Sign_ChineseCharacters_HandledCorrectly()
    {
        var (privatePem, publicPem) = TestKeyHelper.GenerateRsaKeyPair();
        var options = new AlipayOptions
        {
            AppId = "test_app",
            PrivateKey = privatePem,
            AlipayPublicKey = publicPem
        };
        var signer = new AlipaySigner(options);
        var content = "subject=支付测试商品&body=订单描述文字";
        var signature = signer.Sign(content);
        var verified = signer.Verify(content, signature);
        Assert.True(verified);
    }

    [Fact]
    public void WechatPaySigner_Sign_ProducesVerifiableSignature()
    {
        var (privatePem, publicPem) = TestKeyHelper.GenerateRsaKeyPair();
        var options = new WechatPayOptions
        {
            AppId = "wx_test",
            MchId = "1600000000",
            ApiV3Key = "0123456789ABCDEF0123456789ABCDEF",
            PrivateKey = privatePem,
            CertSerialNo = "SERIAL001",
            PlatformPublicKey = publicPem
        };
        var signer = new WechatPaySigner(options);
        var message = "test message for signing";
        var signature = signer.Sign(message);
        Assert.NotNull(signature);
        Assert.NotEmpty(signature);
        using var rsa = RSA.Create();
        rsa.ImportFromPem(privatePem);
        var signedData = rsa.SignData(Encoding.UTF8.GetBytes(message), HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        Assert.Equal(signature, Convert.ToBase64String(signedData));
    }

    [Fact]
    public void WechatPaySigner_BuildAuthorization_ContainsRequiredFields()
    {
        var (privatePem, publicPem) = TestKeyHelper.GenerateRsaKeyPair();
        var options = new WechatPayOptions
        {
            AppId = "wx_test",
            MchId = "1600000000",
            ApiV3Key = "0123456789ABCDEF0123456789ABCDEF",
            PrivateKey = privatePem,
            CertSerialNo = "SERIAL001",
            PlatformPublicKey = publicPem
        };
        var signer = new WechatPaySigner(options);
        var auth = signer.BuildAuthorization("1600000000", "SERIAL001", "POST", "/v3/pay/transactions/native", "{}");
        Assert.Contains("WECHATPAY2-SHA256-RSA2048", auth);
        Assert.Contains("mchid=\"1600000000\"", auth);
        Assert.Contains("serial_no=\"SERIAL001\"", auth);
        Assert.Contains("signature=\"", auth);
        Assert.Contains("nonce_str=\"", auth);
        Assert.Contains("timestamp=\"", auth);
    }

    [Fact]
    public void WechatPaySigner_VerifySignature_WithCorrectSignature_ReturnsTrue()
    {
        var (privatePem, publicPem) = TestKeyHelper.GenerateRsaKeyPair();
        var options = new WechatPayOptions
        {
            AppId = "wx_test",
            MchId = "1600000000",
            ApiV3Key = "0123456789ABCDEF0123456789ABCDEF",
            PrivateKey = privatePem,
            CertSerialNo = "SERIAL001",
            PlatformPublicKey = publicPem,
            PlatformPublicKeyId = "PUB_KEY_ID_001"
        };
        var signer = new WechatPaySigner(options);
        using var rsa = RSA.Create();
        rsa.ImportFromPem(privatePem);
        var body = "{\"out_trade_no\":\"order_001\"}";
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
        var nonce = "testNonce123";
        var message = $"{timestamp}\n{nonce}\n{body}\n";
        var sigBytes = rsa.SignData(Encoding.UTF8.GetBytes(message), HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        var signature = Convert.ToBase64String(sigBytes);
        using var verifyRsa = RSA.Create();
        verifyRsa.ImportFromPem(publicPem);
        var headers = new WechatPayCallbackHeaders
        {
            Timestamp = timestamp,
            Nonce = nonce,
            Signature = signature,
            Serial = "PUB_KEY_ID_001"
        };
        var result = signer.VerifySignature(body, headers, verifyRsa);
        Assert.True(result);
    }

    [Fact]
    public void WechatPaySigner_VerifySignature_WithSignTestPrefix_ReturnsFalse()
    {
        var (privatePem, publicPem) = TestKeyHelper.GenerateRsaKeyPair();
        var options = new WechatPayOptions
        {
            AppId = "wx_test",
            MchId = "1600000000",
            ApiV3Key = "0123456789ABCDEF0123456789ABCDEF",
            PrivateKey = privatePem,
            CertSerialNo = "SERIAL001",
            PlatformPublicKey = publicPem
        };
        var signer = new WechatPaySigner(options);
        var headers = new WechatPayCallbackHeaders
        {
            Timestamp = "1234567890",
            Nonce = "test",
            Signature = "WECHATPAY/SIGNTEST/anything",
            Serial = "SERIAL"
        };
        var result = signer.VerifySignature("body", headers);
        Assert.False(result);
    }

    [Fact]
    public void WechatPaySigner_DecryptSensitiveField_RoundTrip()
    {
        var (privatePem, publicPem) = TestKeyHelper.GenerateRsaKeyPair();
        var options = new WechatPayOptions
        {
            AppId = "wx_test",
            MchId = "1600000000",
            ApiV3Key = "0123456789ABCDEF0123456789ABCDEF",
            PrivateKey = privatePem,
            CertSerialNo = "SERIAL001",
            PlatformPublicKey = publicPem
        };
        var signer = new WechatPaySigner(options);
        var plainText = "6222021234567890123";
        var encrypted = signer.EncryptSensitiveField(plainText);
        Assert.NotNull(encrypted);
        Assert.NotEmpty(encrypted);
        Assert.NotEqual(plainText, encrypted);
        var decrypted = signer.DecryptSensitiveField(encrypted);
        Assert.Equal(plainText, decrypted);
    }

    [Fact]
    public void WechatPaySigner_EncryptSensitiveField_ChineseText()
    {
        var (privatePem, publicPem) = TestKeyHelper.GenerateRsaKeyPair();
        var options = new WechatPayOptions
        {
            AppId = "wx_test",
            MchId = "1600000000",
            ApiV3Key = "0123456789ABCDEF0123456789ABCDEF",
            PrivateKey = privatePem,
            CertSerialNo = "SERIAL001",
            PlatformPublicKey = publicPem
        };
        var signer = new WechatPaySigner(options);
        var plainText = "张三";
        var encrypted = signer.EncryptSensitiveField(plainText);
        var decrypted = signer.DecryptSensitiveField(encrypted);
        Assert.Equal(plainText, decrypted);
    }

    [Fact]
    public void WechatPaySigner_DecryptCallback_RoundTrip()
    {
        var apiV3Key = "0123456789ABCDEF0123456789ABCDEF";
        var (privatePem, publicPem) = TestKeyHelper.GenerateRsaKeyPair();
        var options = new WechatPayOptions
        {
            AppId = "wx_test",
            MchId = "1600000000",
            ApiV3Key = apiV3Key,
            PrivateKey = privatePem,
            CertSerialNo = "SERIAL001",
            PlatformPublicKey = publicPem
        };
        var signer = new WechatPaySigner(options);
        var plainText = "{\"out_trade_no\":\"order_001\",\"trade_state\":\"SUCCESS\"}";
        var associatedData = "transaction";
        var nonce = "testNonce123";
        var nonceBytes = Encoding.UTF8.GetBytes(nonce);
        var adBytes = Encoding.UTF8.GetBytes(associatedData);
        var plainBytes = Encoding.UTF8.GetBytes(plainText);
        var keyBytes = Encoding.UTF8.GetBytes(apiV3Key);
        var ciphertext = new byte[plainBytes.Length + AesGcm.TagByteSizes.MaxSize];
        var tag = ciphertext.AsSpan(plainBytes.Length);
        using (var aes = new AesGcm(keyBytes, AesGcm.TagByteSizes.MaxSize))
        {
            aes.Encrypt(nonceBytes, plainBytes, ciphertext.AsSpan(0, plainBytes.Length), tag, adBytes);
        }
        var ciphertextB64 = Convert.ToBase64String(ciphertext);
        var decrypted = signer.DecryptCallback(associatedData, nonce, ciphertextB64);
        Assert.Equal(plainText, decrypted);
    }

    [Fact]
    public void UnionPaySigner_Sign_ProducesVerifiableSignature()
    {
        var (privatePem, publicPem) = TestKeyHelper.GenerateRsaKeyPair();
        var options = new UnionPayOptions
        {
            MerId = "777290058167097",
            PrivateKey = privatePem,
            CertId = "cert_001",
            UnionPayPublicKey = publicPem,
            FrontUrl = "https://example.com/front",
            BackUrl = "https://example.com/back"
        };
        var signer = new UnionPaySigner(options);
        var parameters = new Dictionary<string, string>
        {
            ["merId"] = "777290058167097",
            ["txnAmt"] = "100",
            ["orderId"] = "order_001",
            ["txnTime"] = "20250101120000"
        };
        var signature = signer.Sign(parameters);
        Assert.NotNull(signature);
        Assert.NotEmpty(signature);
    }

    [Fact]
    public void UnionPaySigner_Verify_ValidSignature_ReturnsTrue()
    {
        var (privatePem, publicPem) = TestKeyHelper.GenerateRsaKeyPair();
        var options = new UnionPayOptions
        {
            MerId = "777290058167097",
            PrivateKey = privatePem,
            CertId = "cert_001",
            UnionPayPublicKey = publicPem,
            FrontUrl = "https://example.com/front",
            BackUrl = "https://example.com/back"
        };
        var signer = new UnionPaySigner(options);
        var parameters = new Dictionary<string, string>
        {
            ["merId"] = "777290058167097",
            ["txnAmt"] = "100",
            ["orderId"] = "order_001",
            ["txnTime"] = "20250101120000"
        };
        var signature = signer.Sign(parameters);
        var verified = signer.Verify(parameters, signature);
        Assert.True(verified);
    }

    [Fact]
    public void UnionPaySigner_Verify_TamperedParameters_ReturnsFalse()
    {
        var (privatePem, publicPem) = TestKeyHelper.GenerateRsaKeyPair();
        var options = new UnionPayOptions
        {
            MerId = "777290058167097",
            PrivateKey = privatePem,
            CertId = "cert_001",
            UnionPayPublicKey = publicPem,
            FrontUrl = "https://example.com/front",
            BackUrl = "https://example.com/back"
        };
        var signer = new UnionPaySigner(options);
        var parameters = new Dictionary<string, string>
        {
            ["merId"] = "777290058167097",
            ["txnAmt"] = "100",
            ["orderId"] = "order_001"
        };
        var signature = signer.Sign(parameters);
        var tampered = new Dictionary<string, string>
        {
            ["merId"] = "777290058167097",
            ["txnAmt"] = "99999",
            ["orderId"] = "order_001"
        };
        var verified = signer.Verify(tampered, signature);
        Assert.False(verified);
    }

    [Fact]
    public void UnionPaySigner_BuildSignContent_SkipsSignatureAndSignPubKeyCert()
    {
        var content = UnionPaySigner.BuildSignContent(new Dictionary<string, string>
        {
            ["merId"] = "001",
            ["signature"] = "xxx",
            ["signPubKeyCert"] = "yyy",
            ["txnAmt"] = "100"
        });
        Assert.Contains("merId=001", content);
        Assert.Contains("txnAmt=100", content);
        Assert.DoesNotContain("signature", content);
        Assert.DoesNotContain("signPubKeyCert", content);
    }

    [Fact]
    public void UnionPaySigner_BuildSignContent_SkipsEmptyValues()
    {
        var content = UnionPaySigner.BuildSignContent(new Dictionary<string, string>
        {
            ["merId"] = "001",
            ["txnAmt"] = "100",
            ["emptyField"] = ""
        });
        Assert.Contains("merId=001", content);
        Assert.Contains("txnAmt=100", content);
        Assert.DoesNotContain("emptyField", content);
    }

    [Fact]
    public void UnionPaySigner_BuildSignContent_KeySortedOrder()
    {
        var content = UnionPaySigner.BuildSignContent(new Dictionary<string, string>
        {
            ["certId"] = "cert_001",
            ["merId"] = "777290058167097",
            ["txnAmt"] = "100",
            ["orderId"] = "order_001"
        });
        var parts = content.Split('&');
        Assert.Equal("certId=cert_001", parts[0]);
        Assert.Equal("merId=777290058167097", parts[1]);
        Assert.Equal("orderId=order_001", parts[2]);
        Assert.Equal("txnAmt=100", parts[3]);
    }
}
