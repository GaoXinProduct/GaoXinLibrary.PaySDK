using System.Security.Cryptography;
using System.Text;

namespace GaoXinLibrary.PaySDK.Alipay.Core;

/// <summary>
/// 支付宝 RSA2（SHA256withRSA）签名工具
/// </summary>
public sealed class AlipaySigner
{
    private readonly RSA _privateRsa;
    private readonly RSA _publicRsa;

    /// <summary>
    /// 初始化签名工具
    /// </summary>
    public AlipaySigner(AlipayOptions options)
    {
        _privateRsa = LoadRsaKey(options.PrivateKey, isPrivate: true);
        _publicRsa = LoadRsaKey(options.AlipayPublicKey, isPrivate: false);
    }

    /// <summary>
    /// 对字符串进行 RSA2（SHA256withRSA）签名，返回 Base64 编码结果
    /// </summary>
    public string Sign(string content)
    {
        var data = Encoding.UTF8.GetBytes(content);
        var signature = _privateRsa.SignData(data, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        return Convert.ToBase64String(signature);
    }

    /// <summary>
    /// 验证支付宝回调签名
    /// </summary>
    public bool Verify(string content, string signature)
    {
        try
        {
            var data = Encoding.UTF8.GetBytes(content);
            var sigBytes = Convert.FromBase64String(signature);
            return _publicRsa.VerifyData(data, sigBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }
        catch
        {
            return false;
        }
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
            // 裸 Base64
            var keyBytes = Convert.FromBase64String(pem);
            if (isPrivate)
                rsa.ImportPkcs8PrivateKey(keyBytes, out _);
            else
                rsa.ImportSubjectPublicKeyInfo(keyBytes, out _);
        }

        return rsa;
    }
}
