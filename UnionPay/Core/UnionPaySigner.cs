using System.Security.Cryptography;
using System.Text;

namespace GaoXinLibrary.PaySDK.UnionPay.Core;

/// <summary>
/// 银联支付 RSA 签名工具（SHA256withRSA）
/// <para>https://open.unionpay.com/tjweb/support/doc/online/3/125</para>
/// </summary>
public sealed class UnionPaySigner
{
    private readonly RSA _privateRsa;
    private readonly RSA _publicRsa;
    private readonly UnionPayOptions _options;

    /// <summary>
    /// 初始化银联签名工具
    /// </summary>
    public UnionPaySigner(UnionPayOptions options)
    {
        _options = options;
        _privateRsa = LoadRsaKey(options.PrivateKey, isPrivate: true);
        _publicRsa = LoadRsaKey(options.UnionPayPublicKey, isPrivate: false);
    }

    /// <summary>
    /// 生成签名：对参数字典排序后拼接，使用 SHA-256 哈希，再用 RSA 私钥签名
    /// </summary>
    public string Sign(Dictionary<string, string> parameters)
    {
        var signContent = BuildSignContent(parameters);
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(signContent));
        var signature = _privateRsa.SignHash(hash, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        return Convert.ToBase64String(signature);
    }

    /// <summary>
    /// 验证银联回调签名
    /// </summary>
    public bool Verify(Dictionary<string, string> parameters, string signature)
    {
        try
        {
            var signContent = BuildSignContent(parameters);
            var hash = SHA256.HashData(Encoding.UTF8.GetBytes(signContent));
            var sigBytes = Convert.FromBase64String(signature);
            return _publicRsa.VerifyHash(hash, sigBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// 构建签名原文：参数字典按 key 排序后拼接为 key=value&amp; 格式，再做 SHA-256
    /// </summary>
    public static string BuildSignContent(Dictionary<string, string> parameters)
    {
        var sb = new StringBuilder();
        foreach (var kv in parameters.OrderBy(x => x.Key, StringComparer.Ordinal))
        {
            if (kv.Key is "signature" or "signPubKeyCert") continue;
            if (string.IsNullOrEmpty(kv.Value)) continue;
            if (sb.Length > 0) sb.Append('&');
            sb.Append(kv.Key).Append('=').Append(kv.Value);
        }
        return sb.ToString();
    }

    private static RSA LoadRsaKey(string key, bool isPrivate)
    {
        var pem = key.Trim();
        var rsa = RSA.Create();

        if (pem.StartsWith("-----"))
        {
            if (isPrivate)
                rsa.ImportFromPem(pem);
            else
                rsa.ImportFromPem(pem);
        }
        else
        {
            var keyBytes = Convert.FromBase64String(pem);
            if (isPrivate)
                rsa.ImportPkcs8PrivateKey(keyBytes, out _);
            else
                rsa.ImportSubjectPublicKeyInfo(keyBytes, out _);
        }

        return rsa;
    }
}
