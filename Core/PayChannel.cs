using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GaoXinLibrary.PaySDK.Core;

/// <summary>
/// 支付渠道（精确到子渠道）
/// </summary>
public enum PayChannel : byte
{
    //0-10由外部系统定义，11-20微信支付，21-30支付宝支付，31-40银联支付，41-50其他支付

    /// <summary>
    /// 微信 JSAPI 支付（公众号内）
    /// </summary>
    [Description("微信JSAPI")]
    [Display(Name = "微信JSAPI支付")]
    WechatJsapi = 11,

    /// <summary>
    /// 微信 APP 支付
    /// </summary>
    [Description("微信app")]
    [Display(Name = "微信app支付")]
    WechatApp,

    /// <summary>
    /// 微信 H5 支付（手机浏览器）
    /// </summary>
    [Description("微信h5")]
    [Display(Name = "微信h5支付")]
    WechatH5,

    /// <summary>
    /// 微信 Native 扫码支付
    /// </summary>
    [Description("微信支付")]
    [Display(Name = "微信支付(Native)")]
    WechatNative,

    /// <summary>
    /// 微信小程序支付
    /// </summary>
    [Description("微信小程序")]
    [Display(Name = "微信小程序支付")]
    WechatMiniProgram,

    /// <summary>
    /// 支付宝当面付（商家扫用户码）
    /// </summary>
    [Description("支付宝当面付")]
    [Display(Name = "支付宝当面付")]
    AlipayFaceToFace = 21,

    /// <summary>
    /// 支付宝订单码支付（用户扫商家码 / Precreate）
    /// </summary>
    [Description("支付宝订单码")]
    [Display(Name = "支付宝订单码支付")]
    AlipayPrecreate,

    /// <summary>
    /// 支付宝 JSAPI 支付（生活号 / 小程序内调用）
    /// </summary>
    [Description("支付宝jsapi")]
    [Display(Name = "支付宝jsapi")]
    AlipayJsapi,

    /// <summary>
    /// 支付宝 App 支付
    /// </summary>
    [Description("支付宝app")]
    [Display(Name = "支付宝app")]
    AlipayApp,

    /// <summary>
    /// 支付宝手机网站支付（WAP）
    /// </summary>
    [Description("支付宝h5")]
    [Display(Name = "支付宝h5支付")]
    AlipayWap,

    /// <summary>
    /// 支付宝电脑网站支付（PC PAGE）
    /// </summary>
    [Description("支付宝pc支付")]
    [Display(Name = "支付宝pc支付")]
    AlipayPage,


    /// <summary>
    /// 银联在线网关支付（PC 前台跳转）
    /// <para>bizType=000201，channelType=07</para>
    /// </summary>
    [Description("银联网关支付")]
    [Display(Name = "银联在线网关支付")]
    UnionPayGateway = 31,

    /// <summary>
    /// 银联无跳转支付（后台消费，无须跳转银联网关）
    /// <para>bizType=000301</para>
    /// </summary>
    [Description("银联无跳转支付")]
    [Display(Name = "银联无跳转支付")]
    UnionPayNoRedirect,

    /// <summary>
    /// 银联 WAP 支付（手机网页支付）
    /// <para>bizType=000201，channelType=08</para>
    /// </summary>
    [Description("银联WAP支付")]
    [Display(Name = "银联WAP支付")]
    UnionPayWap,

    /// <summary>
    /// 银联二维码支付（主扫/被扫）
    /// <para>bizType=000000</para>
    /// </summary>
    [Description("银联二维码支付")]
    [Display(Name = "银联二维码支付")]
    UnionPayQrCode,

    /// <summary>
    /// 银联签约支付（签约后免密扣款）
    /// </summary>
    [Description("银联签约支付")]
    [Display(Name = "银联签约支付")]
    UnionPayContract,

    /// <summary>
    /// 云闪付无感支付（签约后后台扣款）
    /// </summary>
    [Description("云闪付无感支付")]
    [Display(Name = "云闪付无感支付")]
    UnionPayQuickPass,

    /// <summary>
    /// 银联 Apple Pay（基于 Token 的移动端支付）
    /// </summary>
    [Description("银联Apple Pay")]
    [Display(Name = "银联Apple Pay")]
    UnionPayApplePay,
}
