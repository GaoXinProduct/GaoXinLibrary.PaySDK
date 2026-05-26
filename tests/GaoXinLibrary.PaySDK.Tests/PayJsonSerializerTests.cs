using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using GaoXinLibrary.PaySDK.Core;
using Xunit;

namespace GaoXinLibrary.PaySDK.Tests;

public class PayJsonSerializerTests
{
    [Fact]
    public void Serialize_SnakeCaseNaming_Applied()
    {
        var obj = new { FirstName = "Test", LastName = "User", EmailAddress = "test@example.com" };
        var json = PayJsonSerializer.Serialize(obj);
        Assert.Contains("\"first_name\"", json);
        Assert.Contains("\"last_name\"", json);
        Assert.Contains("\"email_address\"", json);
        Assert.DoesNotContain("\"FirstName\"", json);
    }

    [Fact]
    public void Serialize_NullProperties_Omitted()
    {
        var obj = new { Name = "Test", Optional = (string?)null, Value = 42 };
        var json = PayJsonSerializer.Serialize(obj);
        Assert.Contains("\"name\"", json);
        Assert.Contains("\"value\"", json);
        Assert.DoesNotContain("\"optional\"", json);
    }

    [Fact]
    public void Serialize_ChineseCharacters_NotEscaped()
    {
        var obj = new { Subject = "支付成功", Description = "订单已支付，金额1.00元" };
        var json = PayJsonSerializer.Serialize(obj);
        Assert.Contains("支付成功", json);
        Assert.Contains("订单已支付，金额1.00元", json);
        Assert.DoesNotContain("\\u", json);
    }

    [Fact]
    public void Serialize_ChineseCharacters_DefaultSerializer_Escapes()
    {
        var obj = new { Subject = "支付成功" };
        var json = JsonSerializer.Serialize(obj);
        Assert.Contains("\\u652F\\u4ED8\\u6210\\u529F", json);
    }

    [Fact]
    public void Deserialize_SnakeCaseJson_ReturnsObject()
    {
        var json = "{\"first_name\":\"Test\",\"last_name\":\"User\"}";
        var result = PayJsonSerializer.Deserialize<PersonModel>(json);
        Assert.NotNull(result);
        Assert.Equal("Test", result.FirstName);
        Assert.Equal("User", result.LastName);
    }

    [Fact]
    public void Deserialize_UnknownProperties_Ignored()
    {
        var json = "{\"first_name\":\"Test\",\"unknown_field\":999}";
        var result = PayJsonSerializer.Deserialize<PersonModel>(json);
        Assert.NotNull(result);
        Assert.Equal("Test", result.FirstName);
    }

    [Fact]
    public void Options_Encoder_IsUnsafeRelaxed()
    {
        var encoder = PayJsonSerializer.Options.Encoder;
        Assert.Equal(JavaScriptEncoder.UnsafeRelaxedJsonEscaping, encoder);
    }

    [Fact]
    public void Options_NamingPolicy_IsSnakeCase()
    {
        var policy = PayJsonSerializer.Options.PropertyNamingPolicy;
        Assert.Same(JsonNamingPolicy.SnakeCaseLower, policy);
    }

    [Fact]
    public void Options_DefaultIgnoreCondition_IsWhenWritingNull()
    {
        var condition = PayJsonSerializer.Options.DefaultIgnoreCondition;
        Assert.Equal(JsonIgnoreCondition.WhenWritingNull, condition);
    }

    [Fact]
    public void Serialize_NullInput_ReturnsNull()
    {
        var json = PayJsonSerializer.Serialize<object?>(null);
        Assert.Equal("null", json);
    }

    [Fact]
    public void Deserialize_NullJson_ReturnsDefault()
    {
        var result = PayJsonSerializer.Deserialize<PersonModel>("null");
        Assert.Null(result);
    }

    [Fact]
    public void Serialize_EmptyObject_ReturnsBraces()
    {
        var json = PayJsonSerializer.Serialize(new { });
        Assert.Equal("{}", json);
    }

    [Fact]
    public void RoundTrip_ComplexObject()
    {
        var original = new PersonModel
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com"
        };
        var json = PayJsonSerializer.Serialize(original);
        var restored = PayJsonSerializer.Deserialize<PersonModel>(json);
        Assert.NotNull(restored);
        Assert.Equal(original.FirstName, restored.FirstName);
        Assert.Equal(original.LastName, restored.LastName);
        Assert.Equal(original.Email, restored.Email);
    }

    private class PersonModel
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Email { get; set; }
    }
}
