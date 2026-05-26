using GaoXinLibrary.PaySDK.Core;
using Xunit;

namespace GaoXinLibrary.PaySDK.Tests;

public class AmountConversionTests
{
    [Theory]
    [InlineData(0, "0.00")]
    [InlineData(1, "0.01")]
    [InlineData(100, "1.00")]
    [InlineData(999, "9.99")]
    [InlineData(1000, "10.00")]
    [InlineData(9999, "99.99")]
    [InlineData(12345, "123.45")]
    [InlineData(100000, "1000.00")]
    [InlineData(9999999, "99999.99")]
    public void FenToYuan_ConvertsCorrectly(int fen, string expectedYuan)
    {
        var yuan = (fen / 100m).ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
        Assert.Equal(expectedYuan, yuan);
    }

    [Theory]
    [InlineData("0.00", 0)]
    [InlineData("0.01", 1)]
    [InlineData("1.00", 100)]
    [InlineData("9.99", 999)]
    [InlineData("10.00", 1000)]
    [InlineData("99.99", 9999)]
    [InlineData("123.45", 12345)]
    [InlineData("1000.00", 100000)]
    public void YuanToFen_ConvertsCorrectly(string yuan, int expectedFen)
    {
        var fen = (int)(decimal.Parse(yuan, System.Globalization.CultureInfo.InvariantCulture) * 100);
        Assert.Equal(expectedFen, fen);
    }

    [Fact]
    public void FenToYuan_BoundaryMaxInt()
    {
        var fen = int.MaxValue;
        var yuan = (fen / 100m).ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
        Assert.StartsWith("21474836", yuan);
    }

    [Fact]
    public void FenToYuan_BoundaryNegative()
    {
        var fen = -100;
        var yuan = (fen / 100m).ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
        Assert.Equal("-1.00", yuan);
    }

    [Fact]
    public void YuanToFen_TruncationBehavior()
    {
        var fen = (int)(decimal.Parse("1.009", System.Globalization.CultureInfo.InvariantCulture) * 100);
        Assert.Equal(100, fen);
    }

    [Fact]
    public void YuanToFen_RoundingBehavior()
    {
        var fen = (int)(decimal.Parse("1.005", System.Globalization.CultureInfo.InvariantCulture) * 100);
        Assert.Equal(100, fen);
    }

    [Fact]
    public void FenToYuan_OneFen()
    {
        var yuan = (1 / 100m).ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
        Assert.Equal("0.01", yuan);
    }

    [Fact]
    public void FenToYuan_Zero()
    {
        var yuan = (0 / 100m).ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
        Assert.Equal("0.00", yuan);
    }
}
