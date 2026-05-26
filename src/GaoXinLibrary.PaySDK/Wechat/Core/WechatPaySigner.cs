using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using GaoXinLibrary.PaySDK.Wechat.Models;

namespace GaoXinLibrary.PaySDK.Wechat.Core;

/// <summary>
/// 微信支付 v3 签名工具
/// <para>
/// 请求签名：SHA256withRSA（私钥）<br/>
/// 回调解密：AEAD_AES_256_GCM（API v3 Key）<br/>
/// JS-SDK 签名：SHA256withRSA
/// </para>
/// </summary>
public sealed class WechatPaySigner
{
    private readonly RSA _rsa;
    private readonly RSA? _platformRsa;
    private readonly byte[] _apiV3KeyBytes;
    private readonly ConcurrentDictionary<string, RSA> _certCache = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// 当前配置是否为新版微信支付公钥模式
    /// <para>
    /// <c>true</c>：<see cref="WechatPayOptions.PlatformPublicKey"/> 为独立 RSA 公钥，
    /// 请求头应携带 <c>Wechatpay-Serial: PUB_KEY_ID_xxx</c> 通知微信使用公钥模式签名。<br/>
    /// <c>false</c>：<see cref="WechatPayOptions.PlatformPublicKey"/> 为平台证书或未配置，
    /// 使用平台证书模式，不发送 <c>Wechatpay-Serial</c> 请求头。
    /// </para>
    /// </summary>
    public bool IsPlatformPublicKeyMode { get; }

    /// <summary>
    /// 用于敏感信息加密的 <c>Wechatpay-Serial</c> 请求头值
    /// <para>
    /// • <b>微信支付公钥模式</b>：值为微信支付公钥 ID（<c>PUB_KEY_ID_xxx</c>）<br/>
    /// • <b>平台证书模式</b>：值为平台证书序列号（十六进制）
    /// </para>
    /// <para>包含敏感信息加密字段的请求必须携带此值作为 <c>Wechatpay-Serial</c> 请求头。</para>
    /// </summary>
    public string? PlatformSerialNo { get; }

    /// <summary>
    /// 初始化签名工具
    /// </summary>
    /// <param name="options">微信支付配置</param>
    public WechatPaySigner(WechatPayOptions options)
    {
        _rsa = LoadPrivateKey(options.PrivateKey);
        _apiV3KeyBytes = Encoding.UTF8.GetBytes(options.ApiV3Key);

        if (!string.IsNullOrWhiteSpace(options.PlatformPublicKey))
        {
            _platformRsa = LoadPublicKey(options.PlatformPublicKey);

            // 模式检测优先基于 PlatformPublicKeyId（用户显式意图），而非密钥内容格式
            if (!string.IsNullOrWhiteSpace(options.PlatformPublicKeyId)
                && options.PlatformPublicKeyId.StartsWith("PUB_KEY_ID_", StringComparison.OrdinalIgnoreCase))
            {
                // 用户配置了 PUB_KEY_ID_ 前缀 → 公钥模式
                _certCache.TryAdd(options.PlatformPublicKeyId, _platformRsa);
                IsPlatformPublicKeyMode = true;
                PlatformSerialNo = options.PlatformPublicKeyId;

                // 如果 PlatformPublicKey 恰好是证书，也按证书序列号注册（灰度切换期间可能收到两种 serial）
                var certSerialNo = TryExtractCertSerialNo(options.PlatformPublicKey);
                if (certSerialNo is not null)
                    _certCache.TryAdd(certSerialNo, _platformRsa);
            }
            else
            {
                // 无 PUB_KEY_ID_ 前缀 → 平台证书模式
                var certSerialNo = TryExtractCertSerialNo(options.PlatformPublicKey);
                if (certSerialNo is not null)
                {
                    _certCache.TryAdd(certSerialNo, _platformRsa);
                    PlatformSerialNo = certSerialNo;
                }
                else if (!string.IsNullOrWhiteSpace(options.PlatformPublicKeyId))
                {
                    _certCache.TryAdd(options.PlatformPublicKeyId, _platformRsa);
                    PlatformSerialNo = options.PlatformPublicKeyId;
                }
                IsPlatformPublicKeyMode = false;
            }
        }
    }

    /// <summary>
    /// 注册验签公钥，供应答/回调签名验证使用
    /// <para>
    /// <paramref name="serialNo"/> 为 <c>Wechatpay-Serial</c> 请求头值：<br/>
    /// • 新版公钥模式：以 <c>PUB_KEY_ID_</c> 开头的微信支付公钥 ID<br/>
    /// • 旧版证书模式：十六进制平台证书序列号<br/>
    /// 通常在调用 <c>DownloadCertificatesAsync</c> 后由服务层自动注册，也可手动调用。
    /// </para>
    /// </summary>
    /// <param name="serialNo">公钥 ID 或证书序列号（对应 <c>Wechatpay-Serial</c> 请求头）</param>
    /// <param name="certificatePem">X.509 证书 PEM 或 RSA 公钥 PEM 字符串</param>
    public void RegisterCertificate(string serialNo, string certificatePem)
    {
        var rsa = LoadPublicKey(certificatePem);
        _certCache.AddOrUpdate(serialNo, rsa, (_, old) =>
        {
            if (!ReferenceEquals(old, _platformRsa)) old.Dispose();
            return rsa;
        });
    }

    /// <summary>
    /// 使用 SHA256withRSA 对字符串进行签名，返回 Base64 编码结果
    /// </summary>
    public string Sign(string message)
    {
        var msgBytes = Encoding.UTF8.GetBytes(message);
        var signature = _rsa.SignData(msgBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        return Convert.ToBase64String(signature);
    }

    /// <summary>
    /// 构造微信支付 v3 Authorization 请求头
    /// </summary>
    public string BuildAuthorization(string mchId, string certSerialNo, string method, string url, string body)
    {
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
        var nonce = GenerateNonce();
        var message = $"{method}\n{url}\n{timestamp}\n{nonce}\n{body}\n";
        var signature = Sign(message);
        return $"WECHATPAY2-SHA256-RSA2048 mchid=\"{mchId}\",nonce_str=\"{nonce}\",timestamp=\"{timestamp}\",serial_no=\"{certSerialNo}\",signature=\"{signature}\"";
    }

    /// <summary>
    /// 验证微信支付应答/回调通知签名
    /// <para>
    /// 根据 <paramref name="headers"/> 中的 <c>Serial</c>（<c>Wechatpay-Serial</c> 请求头）自适应判断验签模式：<br/>
    /// • <b>新版微信支付公钥模式</b>：<c>Serial</c> 以 <c>PUB_KEY_ID_</c> 开头，
    ///   通过缓存或配置的 <see cref="WechatPayOptions.PlatformPublicKey"/> 验签。<br/>
    /// • <b>旧版平台证书模式</b>：<c>Serial</c> 为证书序列号（十六进制），
    ///   从证书缓存（<c>DownloadCertificatesAsync</c> / <c>RegisterCertificate</c> 填充）中查找。<br/>
    /// 灰度切换期间两种模式可能交替出现，SDK 自动按 serial 匹配。
    /// </para>
    /// </summary>
    /// <param name="body">待验签的报文主体（原始字符串）。</param>
    /// <param name="headers">回调/应答签名头，包含 Timestamp、Nonce、Signature、Serial 四个字段。</param>
    /// <param name="platformPublicKey">可传入外部平台公钥覆盖默认配置，一般传 <c>null</c>。</param>
    public bool VerifySignature(string body, WechatPayCallbackHeaders headers, RSA? platformPublicKey = null)
    {
        // 微信支付会在极少数通知回调中返回以 "WECHATPAY/SIGNTEST/" 开头的探测签名，验签必然失败
        if (headers.Signature.StartsWith("WECHATPAY/SIGNTEST/", StringComparison.OrdinalIgnoreCase))
            return false;

        RSA? verifyKey = null;

        // 1. 按 Wechatpay-Serial 从缓存中统一查找
        //    缓存同时包含：
        //    - 构造函数中注册的 PlatformPublicKeyId → PlatformPublicKey（公钥模式）
        //    - DownloadCertificatesAsync / RegisterCertificate 注册的 证书序列号 → 证书公钥（证书模式）
        if (headers.Serial is not null)
            _certCache.TryGetValue(headers.Serial, out verifyKey);

        // 2. 缓存未命中时的兜底策略
        if (verifyKey is null)
        {
            if (headers.Serial is not null && headers.Serial.StartsWith("PUB_KEY_ID_", StringComparison.OrdinalIgnoreCase))
            {
                // PUB_KEY_ID_ 模式：仅当配置的确实是独立公钥（非平台证书）时使用 _platformRsa
                // 若 PlatformPublicKey 是平台证书，其密钥与微信支付公钥不同，不能用于公钥模式验签
                verifyKey = IsPlatformPublicKeyMode ? _platformRsa : null;
            }
            else if (headers.Serial is not null)
            {
                // 平台证书模式：有明确的 Wechatpay-Serial 但缓存中未命中
                // 仅使用外部显式传入的 platformPublicKey（如有），不使用 _platformRsa
                // 因为 _platformRsa 可能来自商户证书等非平台密钥，使用错误密钥会导致验签失败
                verifyKey = platformPublicKey;
            }
            else
            {
                // 无 serial 的场景（如手动调用）：外部传入公钥 → 配置的平台公钥
                verifyKey = platformPublicKey ?? _platformRsa;
            }
        }

        if (verifyKey is null)
            return false;

        var message = $"{headers.Timestamp}\n{headers.Nonce}\n{body}\n";
        var msgBytes = Encoding.UTF8.GetBytes(message);
        var sigBytes = Convert.FromBase64String(headers.Signature);

        return verifyKey.VerifyData(msgBytes, sigBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
    }

    /// <summary>
    /// 解密微信支付回调通知密文（AEAD_AES_256_GCM）
    /// </summary>
    public string DecryptCallback(string associatedData, string nonce, string ciphertext)
    {
        var ciphertextBytes = Convert.FromBase64String(ciphertext);
        using var aes = new AesGcm(_apiV3KeyBytes, AesGcm.TagByteSizes.MaxSize);

        var nonceBytes = Encoding.UTF8.GetBytes(nonce);
        var associatedDataBytes = Encoding.UTF8.GetBytes(associatedData);

        // 密文 = 加密数据 + 16 字节 tag
        var tagLength = AesGcm.TagByteSizes.MaxSize;
        var cipherDataLength = ciphertextBytes.Length - tagLength;

        var plaintext = new byte[cipherDataLength];
        var tag = new byte[tagLength];

        ciphertextBytes.AsSpan(0, cipherDataLength).CopyTo(plaintext);
        ciphertextBytes.AsSpan(cipherDataLength).CopyTo(tag);

        aes.Decrypt(nonceBytes, plaintext, tag, plaintext, associatedDataBytes);
        return Encoding.UTF8.GetString(plaintext);
    }

    /// <summary>
    /// 构建 JSAPI / 小程序 调起支付签名
    /// </summary>
    public string BuildJsPaySign(string appId, string timestamp, string nonceStr, string prepayId)
    {
        var message = $"{appId}\n{timestamp}\n{nonceStr}\nprepay_id={prepayId}\n";
        return Sign(message);
    }

    private static RSA LoadPrivateKey(string privateKey)
    {
        var pem = privateKey.Trim();
        if (!pem.StartsWith("-----"))
        {
            pem = $"-----BEGIN PRIVATE KEY-----\n{pem}\n-----END PRIVATE KEY-----";
        }
        var rsa = RSA.Create();
        rsa.ImportFromPem(pem);
        return rsa;
    }

    /// <summary>
    /// 使用微信支付公钥或平台证书公钥对敏感字段进行 RSAES-OAEP 加密，返回 Base64 编码密文
    /// <para>
    /// 加密使用 RSA/ECB/OAEPWithSHA-1AndMGF1Padding，对应 .NET 的 <see cref="RSAEncryptionPadding.OaepSHA1"/>。
    /// </para>
    /// <para>
    /// • <b>微信支付公钥模式</b>：使用配置的微信支付公钥加密，请求头 <c>Wechatpay-Serial</c> 传入公钥 ID（<c>PUB_KEY_ID_xxx</c>）。
    ///   参考 <see href="https://pay.weixin.qq.com/doc/v3/merchant/4013053257"/><br/>
    /// • <b>平台证书模式</b>：使用平台证书中的公钥加密，请求头 <c>Wechatpay-Serial</c> 传入平台证书序列号。
    ///   参考 <see href="https://pay.weixin.qq.com/doc/v3/merchant/4013053264"/>
    /// </para>
    /// </summary>
    /// <param name="plainText">需要加密的明文字符串</param>
    /// <returns>Base64 编码的加密密文</returns>
    /// <exception cref="InvalidOperationException">未配置微信支付公钥或平台证书时抛出</exception>
    public string EncryptSensitiveField(string plainText)
    {
        if (_platformRsa is null)
            throw new InvalidOperationException("未配置微信支付公钥或平台证书（PlatformPublicKey），无法加密敏感字段");

        var plainBytes = Encoding.UTF8.GetBytes(plainText);
        var cipherBytes = _platformRsa.Encrypt(plainBytes, RSAEncryptionPadding.OaepSHA1);
        return Convert.ToBase64String(cipherBytes);
    }

    /// <summary>
    /// 使用商户 API 证书私钥对微信支付下行的敏感字段密文进行 RSAES-OAEP 解密
    /// <para>
    /// 微信支付使用商户 API 证书中的公钥对下行敏感信息进行加密，
    /// 开发者应使用商户私钥对密文进行解密。
    /// </para>
    /// <para>
    /// 解密使用 RSA/ECB/OAEPWithSHA-1AndMGF1Padding，对应 .NET 的 <see cref="RSAEncryptionPadding.OaepSHA1"/>。
    /// </para>
    /// <para>https://pay.weixin.qq.com/doc/v3/merchant/4013053265</para>
    /// </summary>
    /// <param name="cipherText">Base64 编码的加密密文</param>
    /// <returns>解密后的明文字符串</returns>
    public string DecryptSensitiveField(string cipherText)
    {
        var cipherBytes = Convert.FromBase64String(cipherText);
        var plainBytes = _rsa.Decrypt(cipherBytes, RSAEncryptionPadding.OaepSHA1);
        return Encoding.UTF8.GetString(plainBytes);
    }

    /// <summary>
    /// 尝试从 PlatformPublicKey 内容中提取 X.509 证书序列号。
    /// 返回 null 表示输入不是证书（是独立公钥）。
    /// </summary>
    private static string? TryExtractCertSerialNo(string publicKey)
    {
        var pem = publicKey.Trim();

        // PEM 格式证书
        if (pem.StartsWith("-----BEGIN CERTIFICATE-----", StringComparison.Ordinal))
        {
            using var cert = X509Certificate2.CreateFromPem(pem);
            return cert.SerialNumber;
        }

        // 其他 PEM 格式（-----BEGIN PUBLIC KEY----- 等）→ 不是证书
        if (pem.StartsWith("-----", StringComparison.Ordinal))
            return null;

        // 无头裸 Base64：检查 DER 结构
        try
        {
            var der = Convert.FromBase64String(pem);
            if (IsX509CertificateDer(der))
            {
                using var cert = X509Certificate2.CreateFromPem(
                    $"-----BEGIN CERTIFICATE-----\n{pem}\n-----END CERTIFICATE-----");
                return cert.SerialNumber;
            }
        }
        catch { /* 非证书格式，忽略 */ }

        return null;
    }

    private static RSA LoadPublicKey(string publicKey)
    {
        var pem = publicKey.Trim();

        // X.509 证书 PEM（-----BEGIN CERTIFICATE-----）→ 提取 RSA 公钥
        if (pem.StartsWith("-----BEGIN CERTIFICATE-----", StringComparison.Ordinal))
        {
            using var cert = X509Certificate2.CreateFromPem(pem);
            return cert.GetRSAPublicKey()
                ?? throw new InvalidOperationException("平台证书中不包含 RSA 公钥");
        }

        // 其他带 PEM 头的格式（-----BEGIN PUBLIC KEY----- 等）→ 直接 ImportFromPem
        if (pem.StartsWith("-----", StringComparison.Ordinal))
        {
            var rsa = RSA.Create();
            rsa.ImportFromPem(pem);
            return rsa;
        }

        // 无头裸 Base64：通过检查 DER 结构区分 X.509 证书与 SPKI 公钥
        // X.509 cert: SEQUENCE { SEQUENCE { [0]EXPLICIT(0xA0) version ... } }
        // SPKI:       SEQUENCE { SEQUENCE { OID(0x06) algorithm ... } BIT STRING }
        var derBytes = Convert.FromBase64String(pem);
        if (IsX509CertificateDer(derBytes))
        {
            // 平台证书 DER（Base64），包装成 PEM 后用 CreateFromPem 加载
            using var cert = X509Certificate2.CreateFromPem(
                $"-----BEGIN CERTIFICATE-----\n{pem}\n-----END CERTIFICATE-----");
            return cert.GetRSAPublicKey()
                ?? throw new InvalidOperationException("平台证书中不包含 RSA 公钥");
        }

        // SPKI 裸 DER
        var spkiRsa = RSA.Create();
        spkiRsa.ImportSubjectPublicKeyInfo(derBytes, out _);
        return spkiRsa;
    }

    /// <summary>
    /// 检测 DER 字节是否为 X.509 证书结构（而非 SPKI）
    /// 在两层 SEQUENCE TL 之后：0xA0 = 证书版本字段，0x06 = SPKI 算法 OID
    /// </summary>
    private static bool IsX509CertificateDer(byte[] der)
    {
        try
        {
            int pos = 0;
            if (der.Length < 6 || der[pos] != 0x30) return false;
            pos += 1 + GetDerLengthSize(der, pos + 1); // 跳过外层 SEQUENCE TL
            if (pos >= der.Length || der[pos] != 0x30) return false;
            pos += 1 + GetDerLengthSize(der, pos + 1); // 跳过内层 SEQUENCE TL
            return pos < der.Length && der[pos] == 0xA0; // [0]EXPLICIT = cert version
        }
        catch { return false; }
    }

    /// <summary>返回 DER 长度字段本身占用的字节数（短格式=1，长格式=1+N）</summary>
    private static int GetDerLengthSize(byte[] der, int offset)
    {
        if (offset >= der.Length) return 0;
        return der[offset] < 0x80 ? 1 : 1 + (der[offset] & 0x7F);
    }

    private static string GenerateNonce()
    {
        var bytes = new byte[16];
        RandomNumberGenerator.Fill(bytes);
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }
}
