using System.Security.Cryptography;

namespace GaoXinLibrary.PaySDK.Tests;

public static class TestKeyHelper
{
    public static (string PrivateKeyPem, string PublicKeyPem) GenerateRsaKeyPair()
    {
        using var rsa = RSA.Create(2048);
        var privatePem = rsa.ExportPkcs8PrivateKeyPem();
        var publicPem = rsa.ExportSubjectPublicKeyInfoPem();
        return (privatePem, publicPem);
    }
}
