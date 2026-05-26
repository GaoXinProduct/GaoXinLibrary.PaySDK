namespace GaoXinLibrary.PaySDK.Core;

/// <summary>
/// <see cref="PayChannel"/> 扩展方法
/// </summary>
public static class PayChannelExtensions
{
    /// <summary>是否为微信支付子渠道</summary>
    public static bool IsWechat(this PayChannel channel) =>
        channel is PayChannel.WechatJsapi
                or PayChannel.WechatApp
                or PayChannel.WechatH5
                or PayChannel.WechatNative
                or PayChannel.WechatMiniProgram;

    /// <summary>是否为支付宝子渠道</summary>
    public static bool IsAlipay(this PayChannel channel) =>
        channel is PayChannel.AlipayFaceToFace
                or PayChannel.AlipayPrecreate
                or PayChannel.AlipayJsapi
                or PayChannel.AlipayApp
                or PayChannel.AlipayWap
                or PayChannel.AlipayPage;

    /// <summary>是否为银联支付子渠道</summary>
    public static bool IsUnionPay(this PayChannel channel) =>
        channel is PayChannel.UnionPayGateway
                or PayChannel.UnionPayNoRedirect
                or PayChannel.UnionPayWap
                or PayChannel.UnionPayQrCode
                or PayChannel.UnionPayContract
                or PayChannel.UnionPayQuickPass
                or PayChannel.UnionPayApplePay;

    /// <summary>将微信子渠道转换为内部 TradeType 字符串</summary>
    internal static string ToWechatTradeType(this PayChannel channel) => channel switch
    {
        PayChannel.WechatJsapi        => "JSAPI",
        PayChannel.WechatApp          => "APP",
        PayChannel.WechatH5           => "H5",
        PayChannel.WechatMiniProgram  => "MINIPROGRAM",
        _                             => "NATIVE"
    };

    /// <summary>将支付宝子渠道转换为内部 PayMethod 字符串</summary>
    internal static string ToAlipayPayMethod(this PayChannel channel) => channel switch
    {
        PayChannel.AlipayFaceToFace => "FACE_TO_FACE",
        PayChannel.AlipayPrecreate  => "PRECREATE",
        PayChannel.AlipayJsapi      => "JSAPI",
        PayChannel.AlipayApp        => "APP",
        PayChannel.AlipayWap        => "WAP",
        _                           => "PAGE"
    };

    /// <summary>将银联子渠道转换为内部产品类型字符串</summary>
    internal static string ToUnionPayProductType(this PayChannel channel) => channel switch
    {
        PayChannel.UnionPayGateway    => "GATEWAY",
        PayChannel.UnionPayNoRedirect => "NO_REDIRECT",
        PayChannel.UnionPayWap        => "WAP",
        PayChannel.UnionPayQrCode     => "QR_CODE",
        PayChannel.UnionPayContract   => "CONTRACT",
        PayChannel.UnionPayQuickPass  => "QUICK_PASS",
        PayChannel.UnionPayApplePay   => "APPLE_PAY",
        _                             => "GATEWAY"
    };
}
