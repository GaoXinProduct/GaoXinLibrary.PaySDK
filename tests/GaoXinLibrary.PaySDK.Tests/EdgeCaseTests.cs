using System.Text.Json;
using GaoXinLibrary.PaySDK.Alipay.Core;
using GaoXinLibrary.PaySDK.Core;
using GaoXinLibrary.PaySDK.UnionPay.Core;
using Xunit;

namespace GaoXinLibrary.PaySDK.Tests;

public class EdgeCaseTests
{
    [Fact]
    public void PayRetryOptions_MaxRetries_Negative_IsAllowed()
    {
        var options = new PayRetryOptions { MaxRetries = -1 };
        Assert.Equal(-1, options.MaxRetries);
    }

    [Fact]
    public void PayRetryOptions_ZeroDelay_IsAllowed()
    {
        var options = new PayRetryOptions
        {
            InitialDelay = TimeSpan.Zero,
            MaxDelay = TimeSpan.Zero
        };
        Assert.Equal(TimeSpan.Zero, options.InitialDelay);
        Assert.Equal(TimeSpan.Zero, options.MaxDelay);
    }

    [Fact]
    public void PayException_WithChannel_MessageContainsChannel()
    {
        var ex = new PayException("ERR001", "Something went wrong", PayChannel.WechatNative);
        Assert.Contains("WechatNative", ex.Message);
        Assert.Contains("ERR001", ex.Message);
        Assert.Equal(PayChannel.WechatNative, ex.Channel);
    }

    [Fact]
    public void PayException_WithoutChannel_MessageFormat()
    {
        var ex = new PayException("ERR002", "General error");
        Assert.DoesNotContain("WechatNative", ex.Message);
        Assert.DoesNotContain("Alipay", ex.Message);
        Assert.Equal("ERR002", ex.ErrorCode);
        Assert.Equal("General error", ex.ErrorMessage);
        Assert.Null(ex.Channel);
    }

    [Fact]
    public void PayException_WithInnerException()
    {
        var inner = new InvalidOperationException("inner detail");
        var ex = new PayException("outer message", inner);
        Assert.Equal("UNKNOWN", ex.ErrorCode);
        Assert.Same(inner, ex.InnerException);
    }

    [Fact]
    public void PayCallbackResult_Defaults()
    {
        var result = new PayCallbackResult();
        Assert.False(result.IsValid);
        Assert.Equal(string.Empty, result.OutTradeNo);
        Assert.Equal(string.Empty, result.TradeStatus);
        Assert.Equal(0, result.TotalFee);
        Assert.Null(result.TransactionId);
        Assert.Null(result.BuyerAccount);
        Assert.Null(result.SuccessTime);
        Assert.Null(result.RawBody);
        Assert.Null(result.ErrorMessage);
    }

    [Fact]
    public void CloseOrderResponse_Defaults()
    {
        var response = new CloseOrderResponse();
        Assert.False(response.Success);
        Assert.False(response.IsSimulated);
        Assert.Equal(CloseOrderOperationMode.Actual, response.OperationMode);
    }

    [Fact]
    public void UnionPaySigner_BuildSignContent_EmptyDictionary_ReturnsEmpty()
    {
        var content = UnionPaySigner.BuildSignContent(new Dictionary<string, string>());
        Assert.Equal(string.Empty, content);
    }

    [Fact]
    public void UnionPaySigner_BuildSignContent_NullValueKeys_Skipped()
    {
        var content = UnionPaySigner.BuildSignContent(new Dictionary<string, string>
        {
            ["merId"] = "001",
            ["optional"] = "",
            ["txnAmt"] = "100"
        });
        Assert.Contains("merId=001", content);
        Assert.Contains("txnAmt=100", content);
        Assert.DoesNotContain("optional", content);
    }

    [Fact]
    public void PayJsonSerializer_Serialize_EmptyString()
    {
        var json = PayJsonSerializer.Serialize("");
        Assert.Equal("\"\"", json);
    }

    [Fact]
    public void PayJsonSerializer_Deserialize_EmptyString_Throws()
    {
        Assert.Throws<JsonException>(() => PayJsonSerializer.Deserialize<PersonModel>(""));
    }

    [Fact]
    public void PayJsonSerializer_Deserialize_Whitespace_Throws()
    {
        Assert.Throws<JsonException>(() => PayJsonSerializer.Deserialize<PersonModel>("   "));
    }

    [Fact]
    public void PayJsonSerializer_Serialize_BooleanValues()
    {
        var obj = new { Active = true, Deleted = false };
        var json = PayJsonSerializer.Serialize(obj);
        Assert.Contains("\"active\":true", json);
        Assert.Contains("\"deleted\":false", json);
    }

    [Fact]
    public void PayJsonSerializer_Serialize_NumberValues()
    {
        var obj = new { Integer = 42, Float = 3.14, Zero = 0, Negative = -1 };
        var json = PayJsonSerializer.Serialize(obj);
        Assert.Contains("\"integer\":42", json);
        Assert.Contains("\"float\":3.14", json);
        Assert.Contains("\"zero\":0", json);
        Assert.Contains("\"negative\":-1", json);
    }

    [Fact]
    public void PayJsonSerializer_Serialize_ArrayValues()
    {
        var obj = new { Items = new[] { "a", "b", "c" } };
        var json = PayJsonSerializer.Serialize(obj);
        Assert.Contains("\"items\":[\"a\",\"b\",\"c\"]", json);
    }

    [Fact]
    public void PayJsonSerializer_Serialize_EmptyArray()
    {
        var obj = new { Items = Array.Empty<string>() };
        var json = PayJsonSerializer.Serialize(obj);
        Assert.Contains("\"items\":[]", json);
    }

    [Fact]
    public void AlipaySigner_Verify_NullSignature_ReturnsFalse()
    {
        var (privatePem, publicPem) = TestKeyHelper.GenerateRsaKeyPair();
        var options = new AlipayOptions
        {
            AppId = "test",
            PrivateKey = privatePem,
            AlipayPublicKey = publicPem
        };
        var signer = new AlipaySigner(options);
        var verified = signer.Verify("content", null!);
        Assert.False(verified);
    }

    [Fact]
    public void PayChannel_UnusedSlots_AreFalseForAllFamilies()
    {
        for (int i = 0; i <= 10; i++)
        {
            var channel = (PayChannel)i;
            Assert.False(channel.IsWechat(), $"Channel {channel} should not be WeChat");
            Assert.False(channel.IsAlipay(), $"Channel {channel} should not be Alipay");
            Assert.False(channel.IsUnionPay(), $"Channel {channel} should not be UnionPay");
        }
    }

    private class PersonModel
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
}
