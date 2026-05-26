using GaoXinLibrary.PaySDK.Core;
using Xunit;

namespace GaoXinLibrary.PaySDK.Tests;

public class PayChannelExtensionsTests
{
    [Theory]
    [InlineData(PayChannel.WechatJsapi, true)]
    [InlineData(PayChannel.WechatApp, true)]
    [InlineData(PayChannel.WechatH5, true)]
    [InlineData(PayChannel.WechatNative, true)]
    [InlineData(PayChannel.WechatMiniProgram, true)]
    [InlineData(PayChannel.AlipayFaceToFace, false)]
    [InlineData(PayChannel.AlipayPage, false)]
    [InlineData(PayChannel.UnionPayGateway, false)]
    [InlineData(PayChannel.UnionPayApplePay, false)]
    public void IsWechat_ReturnsCorrectBool(PayChannel channel, bool expected)
    {
        Assert.Equal(expected, channel.IsWechat());
    }

    [Theory]
    [InlineData(PayChannel.AlipayFaceToFace, true)]
    [InlineData(PayChannel.AlipayPrecreate, true)]
    [InlineData(PayChannel.AlipayJsapi, true)]
    [InlineData(PayChannel.AlipayApp, true)]
    [InlineData(PayChannel.AlipayWap, true)]
    [InlineData(PayChannel.AlipayPage, true)]
    [InlineData(PayChannel.WechatJsapi, false)]
    [InlineData(PayChannel.WechatNative, false)]
    [InlineData(PayChannel.UnionPayGateway, false)]
    [InlineData(PayChannel.UnionPayQrCode, false)]
    public void IsAlipay_ReturnsCorrectBool(PayChannel channel, bool expected)
    {
        Assert.Equal(expected, channel.IsAlipay());
    }

    [Theory]
    [InlineData(PayChannel.UnionPayGateway, true)]
    [InlineData(PayChannel.UnionPayNoRedirect, true)]
    [InlineData(PayChannel.UnionPayWap, true)]
    [InlineData(PayChannel.UnionPayQrCode, true)]
    [InlineData(PayChannel.UnionPayContract, true)]
    [InlineData(PayChannel.UnionPayQuickPass, true)]
    [InlineData(PayChannel.UnionPayApplePay, true)]
    [InlineData(PayChannel.WechatJsapi, false)]
    [InlineData(PayChannel.AlipayPage, false)]
    public void IsUnionPay_ReturnsCorrectBool(PayChannel channel, bool expected)
    {
        Assert.Equal(expected, channel.IsUnionPay());
    }

    [Theory]
    [InlineData(PayChannel.WechatJsapi, "JSAPI")]
    [InlineData(PayChannel.WechatApp, "APP")]
    [InlineData(PayChannel.WechatH5, "H5")]
    [InlineData(PayChannel.WechatNative, "NATIVE")]
    [InlineData(PayChannel.WechatMiniProgram, "MINIPROGRAM")]
    public void ToWechatTradeType_ReturnsCorrectString(PayChannel channel, string expected)
    {
        Assert.Equal(expected, channel.ToWechatTradeType());
    }

    [Theory]
    [InlineData(PayChannel.AlipayPage)]
    [InlineData(PayChannel.UnionPayGateway)]
    public void ToWechatTradeType_NonWechatChannels_FallbackToNative(PayChannel channel)
    {
        Assert.Equal("NATIVE", channel.ToWechatTradeType());
    }

    [Theory]
    [InlineData(PayChannel.AlipayFaceToFace, "FACE_TO_FACE")]
    [InlineData(PayChannel.AlipayPrecreate, "PRECREATE")]
    [InlineData(PayChannel.AlipayJsapi, "JSAPI")]
    [InlineData(PayChannel.AlipayApp, "APP")]
    [InlineData(PayChannel.AlipayWap, "WAP")]
    [InlineData(PayChannel.AlipayPage, "PAGE")]
    public void ToAlipayPayMethod_ReturnsCorrectString(PayChannel channel, string expected)
    {
        Assert.Equal(expected, channel.ToAlipayPayMethod());
    }

    [Theory]
    [InlineData(PayChannel.WechatNative)]
    [InlineData(PayChannel.UnionPayGateway)]
    public void ToAlipayPayMethod_NonAlipayChannels_FallbackToPage(PayChannel channel)
    {
        Assert.Equal("PAGE", channel.ToAlipayPayMethod());
    }

    [Theory]
    [InlineData(PayChannel.UnionPayGateway, "GATEWAY")]
    [InlineData(PayChannel.UnionPayNoRedirect, "NO_REDIRECT")]
    [InlineData(PayChannel.UnionPayWap, "WAP")]
    [InlineData(PayChannel.UnionPayQrCode, "QR_CODE")]
    [InlineData(PayChannel.UnionPayContract, "CONTRACT")]
    [InlineData(PayChannel.UnionPayQuickPass, "QUICK_PASS")]
    [InlineData(PayChannel.UnionPayApplePay, "APPLE_PAY")]
    public void ToUnionPayProductType_ReturnsCorrectString(PayChannel channel, string expected)
    {
        Assert.Equal(expected, channel.ToUnionPayProductType());
    }

    [Theory]
    [InlineData(PayChannel.WechatNative)]
    [InlineData(PayChannel.AlipayPage)]
    public void ToUnionPayProductType_NonUnionPayChannels_FallbackToGateway(PayChannel channel)
    {
        Assert.Equal("GATEWAY", channel.ToUnionPayProductType());
    }

    [Fact]
    public void Channels_AreMutuallyExclusive()
    {
        foreach (PayChannel channel in Enum.GetValues<PayChannel>())
        {
            var isW = channel.IsWechat();
            var isA = channel.IsAlipay();
            var isU = channel.IsUnionPay();
            Assert.False((isW && isA) || (isA && isU) || (isW && isU),
                $"Channel {channel} belongs to multiple families");
            if ((int)channel >= 11)
                Assert.True(isW || isA || isU, $"Channel {channel} belongs to no family");
        }
    }

    [Fact]
    public void PayChannel_Count_AtLeast17Channels()
    {
        var channels = Enum.GetValues<PayChannel>();
        Assert.True(channels.Length >= 17, $"Expected at least 17 channels, got {channels.Length}");
    }
}
