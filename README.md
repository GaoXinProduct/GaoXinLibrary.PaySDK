# GaoXinLibrary.PaySDK

统一支付 SDK，支持**微信支付 v3**、**支付宝**、**银联**三大渠道，兼容 **.NET 8 / 9 / 10**，提供 DI 注入支持。

[![NuGet](https://img.shields.io/nuget/v/GaoXinLibrary.PaySDK.svg)](https://www.nuget.org/packages/GaoXinLibrary.PaySDK)

---

## 目录

- [功能矩阵](#功能矩阵)
- [快速开始](#快速开始)
  - [安装](#安装)
  - [DI 注入（推荐）](#di-注入推荐)
  - [不使用 DI（直接创建客户端）](#不使用-di直接创建客户端)
- [统一接口用法](#统一接口用法)
  - [创建支付订单](#创建支付订单)
  - [查询订单](#查询订单)
  - [申请退款](#申请退款)
  - [查询退款](#查询退款)
  - [关闭订单](#关闭订单)
  - [下载账单](#下载账单)
  - [解析回调通知](#解析回调通知)
- [渠道独立接口](#渠道独立接口)
  - [微信支付](#微信支付独立接口)
  - [支付宝](#支付宝独立接口)
  - [银联](#银联独立接口)
- [微信支付高级功能](#微信支付高级功能)
  - [异常退款](#异常退款)
  - [敏感信息加解密](#敏感信息加解密)
  - [平台证书管理](#平台证书管理)
- [银联跨境电商海关申报](#银联跨境电商海关申报)
- [配置选项参考](#配置选项参考)
- [错误处理](#错误处理)
- [项目结构](#项目结构)

---

## 功能矩阵

| 功能 | 微信支付 | 支付宝 | 银联 |
|------|:-------:|:------:|:----:|
| JSAPI / 公众号支付 | ✅ | ✅ | — |
| APP 支付 | ✅ | ✅ | — |
| H5 / 手机网站支付 | ✅ | ✅ | ✅（WAP） |
| Native / 扫码支付 | ✅ | ✅（订单码） | ✅（二维码主扫） |
| 小程序支付 | ✅ | — | — |
| 当面付（B 扫 C 条码） | — | ✅ | ✅（二维码被扫） |
| 电脑网站支付 | — | ✅ | — |
| 在线网关支付 | — | — | ✅ |
| WAP 支付 | — | — | ✅ |
| 无跳转支付 | — | — | ✅ |
| 二维码支付（主扫/被扫） | — | — | ✅ |
| 签约支付 | — | — | ✅ |
| 云闪付（无感支付） | — | — | ✅ |
| Apple Pay | — | — | ✅ |
| 订单查询 | ✅ | ✅ | ✅ |
| 关闭/撤销订单 | ✅ | ✅（含 cancel） | ✅* |
| 申请退款 | ✅ | ✅ | ✅ |
| 退款查询 | ✅ | ✅ | ✅ |
| 异常退款 | ✅ | — | — |
| 账单下载 | ✅ | ✅ | ✅ |
| 支付回调解析/验签 | ✅ | ✅ | ✅ |
| 退款回调解析 | ✅ | — | — |
| 敏感字段加密 | ✅ | — | — |
| 敏感字段解密 | ✅ | — | — |
| 平台证书下载/管理 | ✅ | — | — |
| 跨境电商海关申报 | — | — | ✅ |
| 加密公钥更新查询 | — | — | ✅ |
| 实名认证 | — | — | ✅ |
| 文件传输（对账文件下载） | — | — | ✅ |

> *银联关闭订单：银联网关支付未支付订单自动超时关闭，SDK 的统一接口返回成功以保持一致性。

---

## 快速开始

### 安装

```bash
dotnet add package GaoXinLibrary.PaySDK
```

### DI 注入（推荐）

**方式一：统一注册（推荐）**

```csharp
// Program.cs
builder.Services.AddPaySDK(sdk =>
{
    // 微信支付
    sdk.AddWechatPay(opt =>
    {
        opt.AppId            = "wx_your_appid";
        opt.MchId            = "1600000000";
        opt.ApiV3Key         = "your_32char_api_v3_key";
        opt.PrivateKey       = "-----BEGIN PRIVATE KEY-----\n...\n-----END PRIVATE KEY-----";
        opt.CertSerialNo     = "your_cert_serial_no";
        opt.NotifyUrl        = "https://your-site.com/pay/wechat/notify"; // 支付异步回调地址（全局默认）
        opt.RefundNotifyUrl  = "https://your-site.com/pay/wechat/refund-notify"; // 退款异步回调地址（全局默认，可选）
        // 新版公钥模式（推荐）
        opt.PlatformPublicKey   = "-----BEGIN PUBLIC KEY-----\n...\n-----END PUBLIC KEY-----";
        opt.PlatformPublicKeyId = "PUB_KEY_ID_xxxx";
        // 或旧版平台证书模式（留空 PlatformPublicKeyId，通过 DownloadCertificatesAsync 自动管理）
    });

    // 支付宝
    sdk.AddAlipay(opt =>
    {
        opt.AppId           = "2021000000000000";
        opt.PrivateKey      = "your_rsa2_private_key";
        opt.AlipayPublicKey = "alipay_rsa2_public_key";
        opt.NotifyUrl       = "https://your-site.com/pay/alipay/notify";  // 异步回调地址（全局默认）
        opt.ReturnUrl       = "https://your-site.com/pay/return";         // 同步跳转地址（全局默认）
    });

    // 银联
    sdk.AddUnionPay(opt =>
    {
        opt.MerId             = "your_mer_id";
        opt.CertId            = "your_cert_id";
        opt.PrivateKey        = "your_rsa_private_key_pem";
        opt.UnionPayPublicKey = "unionpay_verify_public_key_pem";
        opt.FrontUrl          = "https://your-site.com/pay/unionpay/front";
        opt.BackUrl           = "https://your-site.com/pay/unionpay/notify";
    });
});

// 注入统一接口
public class OrderService(IPayService pay) { ... }
```

**方式二：按渠道单独注册**

```csharp
builder.Services
    .AddWechatPay(opt => { /* ... */ })
    .AddAlipay(opt => { /* ... */ })
    .AddUnionPay(opt => { /* ... */ })
    .AddPayService();  // 注册统一路由（可选）

// 按渠道注入
public class MyService(IWechatPayService wechat, IAlipayService alipay) { ... }
```

### 不使用 DI（直接创建客户端）

```csharp
// 微信支付
using var wechatClient = WechatPayClient.Create(new WechatPayOptions
{
    AppId        = "wx_your_appid",
    MchId        = "1600000000",
    ApiV3Key     = "your_api_v3_key",
    PrivateKey   = "your_private_key_pem",
    CertSerialNo = "your_cert_serial_no",
    NotifyUrl    = "https://your-site.com/pay/wechat/notify"
});
var resp = await wechatClient.Pay.CreateNativeOrderAsync(request);

// 支付宝
using var alipayClient = AlipayClient.Create(new AlipayOptions
{
    AppId           = "2021000000000000",
    PrivateKey      = "your_private_key",
    AlipayPublicKey = "alipay_public_key",
    NotifyUrl       = "https://your-site.com/pay/alipay/notify",
    ReturnUrl       = "https://your-site.com/pay/return"
});
var preResp = await alipayClient.Pay.PrecreateAsync(content);

// 银联
using var unionPayClient = UnionPayClient.Create(new UnionPayOptions
{
    MerId             = "your_mer_id",
    CertId            = "your_cert_id",
    PrivateKey        = "your_private_key_pem",
    UnionPayPublicKey = "unionpay_public_key_pem",
    FrontUrl          = "https://your-site.com/pay/unionpay/front",
    BackUrl           = "https://your-site.com/pay/unionpay/notify"
});
var formHtml = unionPayClient.Pay.CreateFrontPay(request);

// 银联海关申报（通过 Customs 属性访问）
var declareResp = await unionPayClient.Customs.DeclareAsync(customsRequest);
```

---

## 统一接口用法

通过 `IPayService` 统一接口，使用 `PayChannel` 枚举指定支付渠道，所有金额单位均为**分**。

### PayChannel 枚举值

| 微信支付 | 支付宝 | 银联 |
|---------|--------|------|
| `WechatJsapi` | `AlipayFaceToFace` | `UnionPayGateway`（在线网关支付） |
| `WechatApp` | `AlipayPrecreate` | `UnionPayNoRedirect`（无跳转支付） |
| `WechatH5` | `AlipayJsapi` | `UnionPayWap`（WAP 手机网页支付） |
| `WechatNative` | `AlipayApp` | `UnionPayQrCode`（二维码支付） |
| `WechatMiniProgram` | `AlipayWap` | `UnionPayContract`（签约支付） |
| | `AlipayPage` | `UnionPayQuickPass`（云闪付/无感支付） |
| | | `UnionPayApplePay`（Apple Pay） |

### 创建支付订单

```csharp
public class OrderService(IPayService pay)
{
    // 微信 Native 扫码支付
    public async Task<string> WechatNativePayAsync()
    {
        var resp = await pay.CreateOrderAsync(new CreateOrderRequest
        {
            Channel    = PayChannel.WechatNative,
            OutTradeNo = "order_001",
            Subject    = "商品描述",
            TotalFee   = 100,           // 1 元 = 100 分
            NotifyUrl  = "https://your-site.com/pay/wechat/notify"
        });
        return resp.CodeUrl!;           // 生成二维码给用户扫码
    }

    // 微信 JSAPI 公众号支付（需要 OpenId）
    public async Task<WechatJsPayParams> WechatJsapiPayAsync(string openId)
    {
        var resp = await pay.CreateOrderAsync(new CreateOrderRequest
        {
            Channel    = PayChannel.WechatJsapi,
            OutTradeNo = "order_002",
            Subject    = "商品描述",
            TotalFee   = 100,
            NotifyUrl  = "https://your-site.com/pay/wechat/notify",
            OpenId     = openId
        });
        return resp.JsPayParams!;       // 前端 JS-SDK 调起支付参数
    }

    // 微信 APP 支付
    public async Task<string> WechatAppPayAsync()
    {
        var resp = await pay.CreateOrderAsync(new CreateOrderRequest
        {
            Channel    = PayChannel.WechatApp,
            OutTradeNo = "order_003",
            Subject    = "商品描述",
            TotalFee   = 100,
            NotifyUrl  = "https://your-site.com/pay/wechat/notify"
        });
        return resp.SdkOrderString!;    // APP SDK 调起参数 JSON
    }

    // 微信 H5 支付（手机浏览器）
    public async Task<string> WechatH5PayAsync(string clientIp)
    {
        var resp = await pay.CreateOrderAsync(new CreateOrderRequest
        {
            Channel    = PayChannel.WechatH5,
            OutTradeNo = "order_004",
            Subject    = "商品描述",
            TotalFee   = 100,
            NotifyUrl  = "https://your-site.com/pay/wechat/notify",
            ClientIp   = clientIp,
            SceneType  = "Wap"
        });
        return resp.PayUrl!;            // 跳转链接
    }

    // 微信小程序支付（需要 OpenId）
    public async Task<WechatJsPayParams> WechatMiniProgramPayAsync(string openId)
    {
        var resp = await pay.CreateOrderAsync(new CreateOrderRequest
        {
            Channel    = PayChannel.WechatMiniProgram,
            OutTradeNo = "order_005",
            Subject    = "商品描述",
            TotalFee   = 100,
            NotifyUrl  = "https://your-site.com/pay/wechat/notify",
            OpenId     = openId
        });
        return resp.JsPayParams!;       // 小程序 wx.requestPayment 参数
    }

    // 支付宝当面付（商家扫用户付款码 - B扫C）
    public async Task<string> AlipayFaceToFacePayAsync(string authCode)
    {
        var resp = await pay.CreateOrderAsync(new CreateOrderRequest
        {
            Channel    = PayChannel.AlipayFaceToFace,
            OutTradeNo = "order_006",
            Subject    = "商品描述",
            TotalFee   = 100,
            AuthCode   = authCode       // 用户付款码（25-36位数字）
        });
        return resp.PrepayId!;          // 支付宝交易号
    }

    // 支付宝订单码支付（生成二维码 - C扫B）
    public async Task<string> AlipayPrecreateAsync()
    {
        var resp = await pay.CreateOrderAsync(new CreateOrderRequest
        {
            Channel    = PayChannel.AlipayPrecreate,
            OutTradeNo = "order_007",
            Subject    = "商品描述",
            TotalFee   = 100,
            NotifyUrl  = "https://your-site.com/pay/alipay/notify"
        });
        return resp.CodeUrl!;           // 二维码链接
    }

    // 支付宝 JSAPI 支付（生活号/小程序内）
    public async Task<string> AlipayJsapiPayAsync(string buyerOpenId)
    {
        var resp = await pay.CreateOrderAsync(new CreateOrderRequest
        {
            Channel    = PayChannel.AlipayJsapi,
            OutTradeNo = "order_008",
            Subject    = "商品描述",
            TotalFee   = 100,
            NotifyUrl  = "https://your-site.com/pay/alipay/notify",
            OpenId     = buyerOpenId,   // buyer_open_id
            Extra      = new Dictionary<string, string>
            {
                ["OpAppId"] = "your_op_appid"   // 可选
            }
        });
        return resp.PrepayId!;          // trade_no, 前端 JS-SDK 唤起支付
    }

    // 支付宝 APP 支付
    public async Task<string> AlipayAppPayAsync()
    {
        var resp = await pay.CreateOrderAsync(new CreateOrderRequest
        {
            Channel    = PayChannel.AlipayApp,
            OutTradeNo = "order_009",
            Subject    = "商品描述",
            TotalFee   = 100,
            NotifyUrl  = "https://your-site.com/pay/alipay/notify"
        });
        return resp.SdkOrderString!;    // APP SDK 签名字符串
    }

    // 支付宝手机网站支付（WAP）
    public async Task<string> AlipayWapPayAsync()
    {
        var resp = await pay.CreateOrderAsync(new CreateOrderRequest
        {
            Channel    = PayChannel.AlipayWap,
            OutTradeNo = "order_010",
            Subject    = "商品描述",
            TotalFee   = 100,
            NotifyUrl  = "https://your-site.com/pay/alipay/notify",
            ReturnUrl  = "https://your-site.com/pay/return"
        });
        return resp.PayUrl!;            // 跳转 URL
    }

    // 支付宝电脑网站支付（PC）
    public async Task<string> AlipayPagePayAsync()
    {
        var resp = await pay.CreateOrderAsync(new CreateOrderRequest
        {
            Channel    = PayChannel.AlipayPage,
            OutTradeNo = "order_011",
            Subject    = "商品描述",
            TotalFee   = 100,
            NotifyUrl  = "https://your-site.com/pay/alipay/notify",
            ReturnUrl  = "https://your-site.com/pay/return"
        });
        return resp.PayUrl!;            // 跳转 URL
    }

    // 银联网关支付（返回自动提交 HTML 表单）
    public async Task<string> UnionPayAsync()
    {
        var resp = await pay.CreateOrderAsync(new CreateOrderRequest
        {
            Channel    = PayChannel.UnionPayGateway,
            OutTradeNo = "order_012",
            Subject    = "商品描述",
            TotalFee   = 100,           // 银联金额单位同为分
            NotifyUrl  = "https://your-site.com/pay/unionpay/notify",
            ReturnUrl  = "https://your-site.com/pay/unionpay/front"
        });
        return resp.PayUrl!;            // HTML 自动提交表单
    }

    // 银联 WAP 手机网页支付
    public async Task<string> UnionPayWapAsync()
    {
        var resp = await pay.CreateOrderAsync(new CreateOrderRequest
        {
            Channel    = PayChannel.UnionPayWap,
            OutTradeNo = "order_013",
            Subject    = "商品描述",
            TotalFee   = 100,
            NotifyUrl  = "https://your-site.com/pay/unionpay/notify",
            ReturnUrl  = "https://your-site.com/pay/unionpay/front"
        });
        return resp.PayUrl!;            // HTML 自动提交表单（WAP 页）
    }

    // 银联二维码支付（主扫 — 生成二维码供用户扫码）
    public async Task<string> UnionPayQrCodeAsync()
    {
        var resp = await pay.CreateOrderAsync(new CreateOrderRequest
        {
            Channel    = PayChannel.UnionPayQrCode,
            OutTradeNo = "order_014",
            Subject    = "商品描述",
            TotalFee   = 100,
            NotifyUrl  = "https://your-site.com/pay/unionpay/notify"
        });
        return resp.CodeUrl!;           // 二维码链接
    }

    // 银联无跳转支付（后台消费，需卡号和持卡人信息）
    public async Task<string> UnionPayNoRedirectAsync()
    {
        var resp = await pay.CreateOrderAsync(new CreateOrderRequest
        {
            Channel    = PayChannel.UnionPayNoRedirect,
            OutTradeNo = "order_015",
            Subject    = "商品描述",
            TotalFee   = 100,
            NotifyUrl  = "https://your-site.com/pay/unionpay/notify",
            Extra      = new Dictionary<string, string>
            {
                ["AccNo"]        = "6222021234567890",    // 卡号
                ["CustomerInfo"] = "{...}"                 // 持卡人信息 JSON
            }
        });
        return resp.PrepayId!;          // 交易流水号
    }

    // 银联 Apple Pay（基于 Token 的移动端支付）
    public async Task<string> UnionPayApplePayAsync(string payData)
    {
        var resp = await pay.CreateOrderAsync(new CreateOrderRequest
        {
            Channel    = PayChannel.UnionPayApplePay,
            OutTradeNo = "order_016",
            Subject    = "商品描述",
            TotalFee   = 100,
            NotifyUrl  = "https://your-site.com/pay/unionpay/notify",
            Extra      = new Dictionary<string, string>
            {
                ["PayData"] = payData   // Apple Pay Token 数据
            }
        });
        return resp.PrepayId!;          // 交易流水号
    }
}
```

### 查询订单

```csharp
var result = await pay.QueryOrderAsync(new QueryOrderRequest
{
    Channel    = PayChannel.WechatNative,   // 或任意渠道
    OutTradeNo = "order_001"                // 商户订单号
    // 也可使用: TransactionId = "平台交易号"
});
// result.TradeStatus: SUCCESS / NOTPAY / CLOSED / REFUND ...
// result.TotalFee:    订单金额（分）
// result.SuccessTime: 支付完成时间
```

### 申请退款

```csharp
var result = await pay.RefundAsync(new RefundRequest
{
    Channel     = PayChannel.WechatNative,
    OutTradeNo  = "order_001",
    OutRefundNo = "refund_001",
    RefundFee   = 50,           // 退款金额（分）
    TotalFee    = 100,          // 原订单总额（分）
    Reason      = "用户申请退款",
    NotifyUrl   = "https://your-site.com/pay/wechat/refund-notify"  // 可选
});
// result.RefundStatus: SUCCESS / PROCESSING / CLOSED / ABNORMAL
```

### 查询退款

```csharp
var result = await pay.QueryRefundAsync(new QueryRefundRequest
{
    Channel     = PayChannel.WechatNative,
    OutRefundNo = "refund_001"
});
// result.RefundStatus / result.RefundFee
```

### 关闭订单

```csharp
var result = await pay.CloseOrderAsync(new CloseOrderRequest
{
    Channel    = PayChannel.WechatNative,
    OutTradeNo = "order_001"
});
// result.Success: true
```

### 下载账单

```csharp
// 微信交易账单
byte[] csv = await pay.DownloadBillAsync(new DownloadBillRequest
{
    Channel  = PayChannel.WechatNative,
    BillDate = "20250101",
    BillType = "ALL"            // ALL / SUCCESS / REFUND
});

// 支付宝交易账单
byte[] aliCsv = await pay.DownloadBillAsync(new DownloadBillRequest
{
    Channel  = PayChannel.AlipayPage,
    BillDate = "2025-01-01",
    BillType = "trade"          // trade / signcustomer
});

// 银联对账文件
byte[] unionCsv = await pay.DownloadBillAsync(new DownloadBillRequest
{
    Channel  = PayChannel.UnionPayGateway,
    BillDate = "0119",          // MMdd 格式
    BillType = "00"             // 00 = 普通对账文件
});
```

### 解析回调通知

> **⚠️ JSON 序列化中文显示提示**
>
> 使用 `JsonSerializer.Serialize()` 序列化回调对象时，默认会将中文转义为 `\uXXXX`（如 `\u652F\u4ED8\u6210\u529F`）。
> 若需要日志或响应中正确显示中文，请使用 `WechatPayHttpClient.JsonOptions`（已配置 `JavaScriptEncoder.UnsafeRelaxedJsonEscaping`）：
>
> ```csharp
> // ❌ 中文会显示为 \uXXXX
> var json = JsonSerializer.Serialize(order);
>
> // ✅ 中文正常显示
> using GaoXinLibrary.PaySDK.Wechat.Core;
> var json = JsonSerializer.Serialize(order, WechatPayHttpClient.JsonOptions);
> ```

```csharp
// ── 微信支付回调（JSON Body + HTTP Header 签名验证） ──
[HttpPost("wechat/notify")]
public async Task<IActionResult> WechatNotify([FromServices] IPayService pay)
{
    using var reader = new StreamReader(Request.Body);
    var body = await reader.ReadToEndAsync();

    var headers = new Dictionary<string, string>
    {
        ["Wechatpay-Timestamp"] = Request.Headers["Wechatpay-Timestamp"].ToString(),
        ["Wechatpay-Nonce"]     = Request.Headers["Wechatpay-Nonce"].ToString(),
        ["Wechatpay-Signature"] = Request.Headers["Wechatpay-Signature"].ToString(),
        ["Wechatpay-Serial"]    = Request.Headers["Wechatpay-Serial"].ToString()
    };

    var result = await pay.ParseCallbackAsync(PayChannel.WechatJsapi, body, headers);

    if (!result.IsValid)
        return BadRequest();

    // result.OutTradeNo / result.TransactionId / result.TotalFee / result.TradeStatus
    // 处理业务逻辑...
    return Ok(new { code = "SUCCESS", message = "成功" });
}

// ── 支付宝回调（Form 表单 POST，签名验证） ──
[HttpPost("alipay/notify")]
public async Task<IActionResult> AlipayNotify([FromServices] IPayService pay)
{
    var form = Request.Form.ToDictionary(k => k.Key, v => v.Value.ToString());
    var formString = string.Join("&",
        form.Select(kv => $"{Uri.EscapeDataString(kv.Key)}={Uri.EscapeDataString(kv.Value)}"));

    var result = await pay.ParseCallbackAsync(PayChannel.AlipayPage, formString);

    if (!result.IsValid)
        return BadRequest();

    // result.OutTradeNo / result.TradeStatus ("TRADE_SUCCESS" / "TRADE_FINISHED")
    return Content("success", "text/plain");
}

// ── 银联回调（Form 表单 POST，签名验证） ──
[HttpPost("unionpay/notify")]
public async Task<IActionResult> UnionPayNotify([FromServices] IPayService pay)
{
    var form = Request.Form.ToDictionary(k => k.Key, v => v.Value.ToString());
    var formString = string.Join("&",
        form.Select(kv => $"{Uri.EscapeDataString(kv.Key)}={Uri.EscapeDataString(kv.Value)}"));

    var result = await pay.ParseCallbackAsync(PayChannel.UnionPayGateway, formString);

    if (!result.IsValid)
        return BadRequest();

    // result.OutTradeNo / result.TransactionId / result.TotalFee
    return Content("ok", "text/plain");
}
```

---

## 渠道独立接口

除了统一 `IPayService`，你也可以直接注入各渠道独立接口获得更精细的控制。

### 微信支付独立接口

```csharp
public class WechatController(IWechatPayService wechat)
{
    // JSAPI 下单（公众号/小程序内网页支付）
    public async Task<WechatJsPayParams> JsapiPayAsync(string openId)
    {
        var resp = await wechat.CreateJsapiOrderAsync(new WechatJsapiOrderRequest
        {
            OutTradeNo  = "order_001",
            Description = "商品描述",
            NotifyUrl   = "https://your-site.com/pay/wechat/notify",
            Amount      = new WechatPayAmount { Total = 100 },
            Payer       = new WechatPayPayer { OpenId = openId }
        });
        return wechat.BuildJsPayParams(resp.PrepayId);
    }

    // APP 下单
    public async Task<WechatAppPayParams> AppPayAsync()
    {
        var resp = await wechat.CreateAppOrderAsync(new WechatAppOrderRequest
        {
            OutTradeNo  = "order_002",
            Description = "商品描述",
            NotifyUrl   = "https://your-site.com/pay/wechat/notify",
            Amount      = new WechatPayAmount { Total = 100 }
        });
        return wechat.BuildAppPayParams(resp.PrepayId);
    }

    // H5 下单（SceneInfo 和 H5Info 已预初始化默认值，只需设置 PayerClientIp）
    public async Task<string> H5PayAsync(string clientIp)
    {
        var resp = await wechat.CreateH5OrderAsync(new WechatH5OrderRequest
        {
            OutTradeNo  = "order_003",
            Description = "商品描述",
            NotifyUrl   = "https://your-site.com/pay/wechat/notify",
            Amount      = new WechatPayAmount { Total = 100 },
            SceneInfo   = { PayerClientIp = clientIp }
        });
        return resp.H5Url;
    }

    // Native 下单（二维码支付）
    public async Task<string> NativePayAsync()
    {
        var resp = await wechat.CreateNativeOrderAsync(new WechatNativeOrderRequest
        {
            OutTradeNo  = "order_004",
            Description = "商品描述",
            NotifyUrl   = "https://your-site.com/pay/wechat/notify",
            Amount      = new WechatPayAmount { Total = 100 }
        });
        return resp.CodeUrl;
    }

    // 查询订单
    public async Task<WechatQueryOrderResponse> QueryAsync(string outTradeNo)
        => await wechat.QueryOrderByOutTradeNoAsync(outTradeNo);

    // 退款
    public async Task<WechatRefundResponse> RefundAsync(string outTradeNo, int refundFee, int totalFee)
    {
        return await wechat.RefundAsync(new WechatRefundRequest
        {
            OutTradeNo  = outTradeNo,
            OutRefundNo = $"refund_{outTradeNo}",
            Amount = new WechatRefundAmount
            {
                Refund   = refundFee,
                Total    = totalFee,
                Currency = "CNY"
            }
        });
    }

    // 关闭订单
    public async Task CloseAsync(string outTradeNo)
        => await wechat.CloseOrderAsync(outTradeNo);

    // 下载交易账单
    public async Task<byte[]> DownloadBillAsync(string billDate)
        => await wechat.DownloadTradeBillAsync(billDate, "ALL");

    // 解析支付回调
    public async Task<WechatPayCallbackDecrypted> ParseCallbackAsync(string body, WechatPayCallbackHeaders headers)
        => await wechat.ParsePayCallbackAsync(body, headers);

    // 解析退款回调
    public async Task<WechatRefundCallbackDecrypted> ParseRefundCallbackAsync(string body, WechatPayCallbackHeaders headers)
        => await wechat.ParseRefundCallbackAsync(body, headers);

    // ── 异常退款 ──────────────────────────────────────────────
    // 退款状态为异常（ABNORMAL）时，调用此接口发起异常退款处理
    // 敏感字段（银行卡号、姓名）由 SDK 自动加密，无需手动处理
    public async Task<WechatAbnormalRefundResponse> ApplyAbnormalRefundAsync(string refundId)
    {
        return await wechat.ApplyAbnormalRefundAsync(new WechatAbnormalRefundRequest
        {
            RefundId    = refundId,              // 微信支付退款单号（路径参数）
            OutRefundNo = "refund_001",           // 商户退款单号
            Type        = "USER_BANK_CARD",       // USER_BANK_CARD 或 MERCHANT_BANK_CARD
            BankType    = "ICBC_DEBIT",           // 开户银行（退款至用户时必填）
            BankAccount = "6222021234567890123",   // 银行卡号（明文，SDK 自动加密）
            RealName    = "张三"                   // 用户姓名（明文，SDK 自动加密）
        });
    }

    // ── 敏感字段加解密 ────────────────────────────────────────
    // 手动加密（一般无需使用，SDK 在 ApplyAbnormalRefundAsync 等接口中自动加密）
    public string EncryptField(string plainText)
        => wechat.EncryptSensitiveField(plainText);

    // 手动解密（用于微信支付下行的加密敏感字段）
    public string DecryptField(string cipherText)
        => wechat.DecryptSensitiveField(cipherText);

    // ── 平台证书管理 ─────────────────────────────────────────
    // 下载并自动注册平台证书（旧版平台证书模式下，建议每 12 小时刷新一次）
    public async Task DownloadAndRegisterCertificatesAsync()
    {
        var certs = await wechat.DownloadCertificatesAsync();
        // certs: [(SerialNo, CertificatePem), ...]
        // 已自动注册到验签缓存，无需额外操作
    }

    // 手动注册平台证书（如从本地文件加载）
    public void RegisterCert(string serialNo, string certPem)
        => wechat.RegisterCertificate(serialNo, certPem);
}
```

### 支付宝独立接口

```csharp
public class AlipayController(IAlipayService alipay)
{
    // 当面付（B扫C，扫用户付款码）
    public async Task<AlipayTradePayResponse> FaceToFacePayAsync(string authCode)
    {
        return await alipay.FaceToFacePayAsync(new AlipayTradePayBizContent
        {
            OutTradeNo  = "order_001",
            Subject     = "商品描述",
            TotalAmount = "1.00",
            AuthCode    = authCode  // 用户付款码，25-36位数字
        });
    }

    // 订单码支付（C扫B，生成二维码）
    public async Task<string> PrecreateAsync()
    {
        var resp = await alipay.PrecreateAsync(
            new AlipayTradePrecreateContent
            {
                OutTradeNo  = "order_002",
                Subject     = "商品描述",
                TotalAmount = "9.90"
            });
        return resp.QrCode;
    }

    // JSAPI 支付（生活号/小程序）
    public async Task<string> JsapiPayAsync(string buyerOpenId)
    {
        var resp = await alipay.CreateOrderAsync(
            new AlipayTradeCreateContent
            {
                OutTradeNo   = "order_003",
                Subject      = "商品描述",
                TotalAmount  = "1.00",
                BuyerOpenId  = buyerOpenId,
                ProductCode  = "JSAPI_PAY"
            });
        return resp.TradeNo;
    }

    // APP 支付（返回 SDK 签名字符串）
    public string AppPayAsync()
    {
        return alipay.BuildAppPayString(
            new AlipayTradeAppPayContent
            {
                OutTradeNo  = "order_004",
                Subject     = "商品描述",
                TotalAmount = "9.90"
            });
    }

    // 手机网站支付（WAP）— notifyUrl / returnUrl 已在 AlipayOptions 中配置
    public string WapPay()
    {
        return alipay.BuildWapPayUrl(
            new AlipayTradeWapPayContent
            {
                OutTradeNo  = "order_005",
                Subject     = "商品描述",
                TotalAmount = "9.90",
                ProductCode = "QUICK_WAP_WAY"
            });
    }

    // 电脑网站支付（PC）— notifyUrl / returnUrl 已在 AlipayOptions 中配置
    public string PagePay()
    {
        return alipay.BuildPagePayUrl(
            new AlipayTradePagePayContent
            {
                OutTradeNo  = "order_006",
                Subject     = "商品描述",
                TotalAmount = "9.90",
                ProductCode = "FAST_INSTANT_TRADE_PAY"
            });
    }

    // 撤销订单（当面付场景专用，已支付会自动退款）
    public async Task<AlipayTradeCancelResponse> CancelAsync(string outTradeNo)
    {
        return await alipay.CancelOrderAsync(new AlipayTradeCancelContent
        {
            OutTradeNo = outTradeNo
        });
    }

    // 关闭订单（未支付状态）
    public async Task<AlipayTradeCloseResponse> CloseAsync(string outTradeNo)
    {
        return await alipay.CloseOrderAsync(
            new AlipayTradeCloseContent { OutTradeNo = outTradeNo },
            ignoreNotExist: true);  // App/H5/PC 用户未跳转支付宝时交易可能不存在
    }

    // 查询订单
    public async Task<AlipayTradeQueryResponse> QueryAsync(string outTradeNo)
    {
        return await alipay.QueryOrderAsync(new AlipayTradeQueryContent
        {
            OutTradeNo = outTradeNo
        });
    }

    // 退款
    public async Task<AlipayTradeRefundResponse> RefundAsync(string outTradeNo)
    {
        return await alipay.RefundAsync(new AlipayTradeRefundContent
        {
            OutTradeNo   = outTradeNo,
            RefundAmount = "1.00",
            RefundReason = "用户申请退款",
            OutRequestNo = $"refund_{outTradeNo}"
        });
    }

    // 退款查询
    public async Task<AlipayTradeRefundQueryResponse> QueryRefundAsync(string outTradeNo, string outRequestNo)
    {
        return await alipay.QueryRefundAsync(new AlipayTradeRefundQueryContent
        {
            OutTradeNo   = outTradeNo,
            OutRequestNo = outRequestNo
        });
    }

    // 账单下载
    public async Task<byte[]> DownloadBillAsync(string billDate)
    {
        return await alipay.DownloadBillAsync(new AlipayBillDownloadContent
        {
            BillType = "trade",
            BillDate = billDate
        });
    }

    // 回调验签
    public AlipayCallbackParams ParseCallback(IDictionary<string, string> formParams)
    {
        var result = alipay.ParseCallback(formParams);
        // result.IsValid / result.TradeStatus / result.OutTradeNo / result.TradeNo
        return result;
    }
}
```

### 银联独立接口

```csharp
public class UnionPayController(IUnionPayService unionPay)
{
    // 在线网关支付（PC 前台跳转，返回 HTML 自动提交表单）
    public string FrontPay(string orderId, int fee)
    {
        var resp = unionPay.CreateFrontPay(new UnionPayFrontPayRequest
        {
            OrderId   = orderId,
            TxnTime   = DateTime.Now.ToString("yyyyMMddHHmmss"),
            TxnAmt    = fee.ToString(),
            OrderDesc = "商品描述"
        });
        return resp.FormHtml;   // 注入页面，浏览器自动 POST 到银联
    }

    // WAP 手机网页支付（移动端前台跳转）
    public string WapPay(string orderId, int fee)
    {
        var resp = unionPay.CreateWapPay(new UnionPayWapPayRequest
        {
            OrderId   = orderId,
            TxnTime   = DateTime.Now.ToString("yyyyMMddHHmmss"),
            TxnAmt    = fee.ToString(),
            OrderDesc = "商品描述"
        });
        return resp.FormHtml;   // WAP 页面自动提交表单
    }

    // 二维码支付 — 主扫（商户生成二维码，用户扫码支付）
    public async Task<string> QrCodeApplyAsync(string orderId, int fee)
    {
        var resp = await unionPay.ApplyQrCodeAsync(new UnionPayQrCodeApplyRequest
        {
            OrderId   = orderId,
            TxnTime   = DateTime.Now.ToString("yyyyMMddHHmmss"),
            TxnAmt    = fee.ToString(),
            OrderDesc = "商品描述"
        });
        return resp.QrCode;     // 二维码链接，前端生成二维码图片
    }

    // 二维码支付 — 被扫（商户扫用户付款码，后台扣款）
    public async Task<UnionPayBackPayResponse> QrCodeConsumeAsync(string orderId, int fee, string qrNo)
    {
        return await unionPay.QrCodeConsumeAsync(new UnionPayQrCodeConsumeRequest
        {
            OrderId   = orderId,
            TxnTime   = DateTime.Now.ToString("yyyyMMddHHmmss"),
            TxnAmt    = fee.ToString(),
            OrderDesc = "商品描述",
            QrNo      = qrNo   // 用户付款码
        });
    }

    // 无跳转支付（后台消费，需卡号和持卡人信息）
    public async Task<UnionPayBackPayResponse> NoRedirectPayAsync(string orderId, int fee, string accNo, string customerInfo)
    {
        return await unionPay.CreateBackPayAsync(new UnionPayBackPayRequest
        {
            BizType      = "000301",
            OrderId      = orderId,
            TxnTime      = DateTime.Now.ToString("yyyyMMddHHmmss"),
            TxnAmt       = fee.ToString(),
            AccNo        = accNo,
            CustomerInfo = customerInfo
        });
    }

    // 签约支付（通过签约协议号免密扣款）
    public async Task<UnionPayBackPayResponse> ContractPayAsync(string orderId, int fee, string contractNo)
    {
        return await unionPay.CreateBackPayAsync(new UnionPayBackPayRequest
        {
            BizType    = "000301",
            OrderId    = orderId,
            TxnTime    = DateTime.Now.ToString("yyyyMMddHHmmss"),
            TxnAmt     = fee.ToString(),
            ContractNo = contractNo
        });
    }

    // 云闪付（无感支付，通过 tokenPayData 扣款）
    public async Task<UnionPayBackPayResponse> QuickPassPayAsync(string orderId, int fee, string tokenPayData)
    {
        return await unionPay.CreateBackPayAsync(new UnionPayBackPayRequest
        {
            BizType      = "000902",
            OrderId      = orderId,
            TxnTime      = DateTime.Now.ToString("yyyyMMddHHmmss"),
            TxnAmt       = fee.ToString(),
            TokenPayData = tokenPayData
        });
    }

    // Apple Pay（基于 Token 的移动端支付，bizType=000802）
    public async Task<UnionPayBackPayResponse> ApplePayAsync(string orderId, int fee, string payData)
    {
        return await unionPay.CreateBackPayAsync(new UnionPayBackPayRequest
        {
            BizType      = "000802",
            OrderId      = orderId,
            TxnTime      = DateTime.Now.ToString("yyyyMMddHHmmss"),
            TxnAmt       = fee.ToString(),
            PayData      = payData   // Apple Pay Token
        });
    }

    // 查询订单
    public async Task<UnionPayQueryResponse> QueryAsync(string orderId, string txnTime)
    {
        return await unionPay.QueryOrderAsync(new UnionPayQueryRequest
        {
            OrderId = orderId,
            TxnTime = txnTime
        });
    }

    // 退款
    public async Task<UnionPayRefundResponse> RefundAsync(string origQueryId, int refundAmt)
    {
        return await unionPay.RefundAsync(new UnionPayRefundRequest
        {
            OrderId     = $"refund_{DateTime.Now:yyyyMMddHHmmss}",
            TxnTime     = DateTime.Now.ToString("yyyyMMddHHmmss"),
            TxnAmt      = refundAmt.ToString(),
            OrigQueryId = origQueryId
        });
    }

    // 对账文件下载
    public async Task<byte[]> DownloadBillAsync(string settleDate)
    {
        return await unionPay.DownloadBillAsync(settleDate, "00");
    }

    // 回调验签
    public UnionPayCallbackParams ParseCallback(IDictionary<string, string> formParams)
    {
        var result = unionPay.ParseCallback(formParams);
        // result.IsValid / result.RespCode / result.OrderId / result.QueryId
        return result;
    }
}
```

---

## 微信支付高级功能

### 异常退款

退款状态为异常（`ABNORMAL`）时，可调用 `ApplyAbnormalRefundAsync` 发起异常退款处理。  
支持退款至用户银行卡（`USER_BANK_CARD`）或退款至交易商户银行账户（`MERCHANT_BANK_CARD`）两种方式。

> **💡 敏感字段自动加密**：银行卡号（`BankAccount`）和用户姓名（`RealName`）只需传入明文，SDK 会自动使用微信支付公钥 / 平台证书进行 RSAES-OAEP 加密，并自动携带 `Wechatpay-Serial` 请求头。

```csharp
// 退款至用户银行卡
var resp = await wechat.ApplyAbnormalRefundAsync(new WechatAbnormalRefundRequest
{
    RefundId    = "50000000382019052709732678859",  // 微信支付退款单号（路径参数）
    OutRefundNo = "refund_001",                     // 商户退款单号
    Type        = "USER_BANK_CARD",                 // 退款至用户银行卡
    BankType    = "ICBC_DEBIT",                     // 开户银行
    BankAccount = "6222021234567890123",             // 银行卡号（明文，SDK 自动加密）
    RealName    = "张三"                             // 用户姓名（明文，SDK 自动加密）
});
// resp.Status: SUCCESS / PROCESSING / ABNORMAL / CLOSED

// 退款至商户银行账户
var resp2 = await wechat.ApplyAbnormalRefundAsync(new WechatAbnormalRefundRequest
{
    RefundId    = "50000000382019052709732678859",
    OutRefundNo = "refund_001",
    Type        = "MERCHANT_BANK_CARD"
});
```

### 敏感信息加解密

SDK 同时支持**微信支付公钥模式**和**平台证书模式**两种加密方式，通过配置自动区分：

| 配置 | 模式 | `Wechatpay-Serial` 请求头 |
|------|------|--------------------------|
| `PlatformPublicKeyId` 以 `PUB_KEY_ID_` 开头 | 微信支付公钥模式 | `PUB_KEY_ID_xxx` |
| `PlatformPublicKeyId` 为空或非 `PUB_KEY_ID_` 前缀 | 平台证书模式 | 平台证书序列号 |

```csharp
// ── 上行加密（商户 → 微信支付）──
// 通常无需手动调用，SDK 在 ApplyAbnormalRefundAsync 等接口中自动加密
// 如需手动加密其他场景的敏感字段：
var encrypted = wechat.EncryptSensitiveField("6222021234567890123");
// encrypted: Base64 编码的 RSAES-OAEP 密文

// ── 下行解密（微信支付 → 商户）──
// 微信支付使用商户 API 证书公钥加密下行敏感信息，SDK 使用商户私钥解密：
var decrypted = wechat.DecryptSensitiveField(encryptedBankAccount);
// decrypted: 银行卡号明文
```

> **加密算法**：RSA/ECB/OAEPWithSHA-1AndMGF1Padding（对应 .NET `RSAEncryptionPadding.OaepSHA1`）  
> **参考文档**：  
> - [微信支付公钥加密](https://pay.weixin.qq.com/doc/v3/merchant/4013053257)  
> - [平台证书加密](https://pay.weixin.qq.com/doc/v3/merchant/4013053264)  
> - [API 证书解密](https://pay.weixin.qq.com/doc/v3/merchant/4013053265)

### 平台证书管理

**旧版平台证书模式**下，需要定期下载平台证书用于回调验签。SDK 提供自动下载并注册的接口：

```csharp
// 下载平台证书并自动注册到验签缓存（建议每 12 小时刷新一次）
var certs = await wechat.DownloadCertificatesAsync();
foreach (var (serialNo, certPem) in certs)
{
    Console.WriteLine($"已注册证书: {serialNo}");
}

// 也可手动注册（如从本地文件加载）
wechat.RegisterCertificate("CERT_SERIAL_NO", certPemContent);
```

> **新版公钥模式**下（配置了 `PUB_KEY_ID_` 前缀的 `PlatformPublicKeyId`），验签使用配置的微信支付公钥，无需下载平台证书。

---

## 银联跨境电商海关申报

银联跨境电商海关申报服务是独立于支付流程的非支付接口，用于将银联支付订单的支付信息向海关申报，实现海关对跨境业务支付流、订单流、物流的三单比对核查。

> ℹ️ 海关申报不是支付渠道，因此不通过统一 `IPayService` 路由，而是通过独立的 `IUnionPayCustomsService` 接口或 `UnionPayClient.Customs` 属性访问。

### DI 注入用法

注册银联支付时会自动注册海关申报服务，直接注入即可：

```csharp
public class CustomsController(IUnionPayCustomsService customs)
{
    // 提交海关申报
    public async Task<UnionPayCustomsDeclarationResponse> DeclareAsync(string origQueryId)
    {
        return await customs.DeclareAsync(new UnionPayCustomsDeclarationRequest
        {
            OrderId     = $"customs_{DateTime.Now:yyyyMMddHHmmss}",
            TxnTime     = DateTime.Now.ToString("yyyyMMddHHmmss"),
            TxnAmt      = "10000",          // 金额（分）
            OrigQueryId = origQueryId,       // 原始支付交易的 queryId
            CustomsCode = "GUANGZHOU",      // 海关代码
            MerAbbr     = "商户备案名称",      // 商户在海关备案的名称
            MerCatCode  = "1234"            // 商户在海关备案的编号
        });
    }

    // 查询海关申报结果
    public async Task<UnionPayCustomsQueryResponse> QueryAsync(string orderId, string txnTime)
    {
        return await customs.QueryDeclarationAsync(new UnionPayCustomsQueryRequest
        {
            OrderId = orderId,
            TxnTime = txnTime
        });
        // resp.OrigRespCode: "00" = 申报成功
    }

    // 加密公钥更新查询（建议每天调用 1 次，获取最新加密公钥证书）
    public async Task<UnionPayEncryptKeyQueryResponse> QueryEncryptKeyAsync()
    {
        return await customs.QueryEncryptKeyAsync(new UnionPayEncryptKeyQueryRequest
        {
            OrderId  = $"key_{DateTime.Now:yyyyMMddHHmmss}",
            TxnTime  = DateTime.Now.ToString("yyyyMMddHHmmss"),
            CertType = "01"     // 01 = 敏感信息加密公钥
        });
        // resp.SignPubKeyCert: 最新的加密公钥证书内容，替换本地证书
    }

    // 实名认证（验证银行卡信息与身份信息一致性）
    public async Task<UnionPayRealNameAuthResponse> RealNameAuthAsync(
        string accNo, string customerInfo)
    {
        return await customs.RealNameAuthAsync(new UnionPayRealNameAuthRequest
        {
            OrderId      = $"auth_{DateTime.Now:yyyyMMddHHmmss}",
            TxnTime      = DateTime.Now.ToString("yyyyMMddHHmmss"),
            AccNo        = accNo,           // 银行卡号（需加密上送）
            CustomerInfo = customerInfo     // Base64: certifTp=01&certifId=xxx&customerNm=xxx&phoneNo=xxx
        });
        // resp.RespCode: "00" = 认证通过
    }

    // 文件传输（下载对账文件）
    public async Task<UnionPayFileTransferResponse> DownloadFileAsync(string settleDate)
    {
        return await customs.FileTransferAsync(new UnionPayFileTransferRequest
        {
            OrderId    = $"file_{DateTime.Now:yyyyMMddHHmmss}",
            TxnTime    = DateTime.Now.ToString("yyyyMMddHHmmss"),
            SettleDate = settleDate,    // 清算日期，格式 MMdd
            FileType   = "00"           // 00 = 普通对账文件
        });
        // resp.FileContent: 解压后的对账文件文本内容
        // resp.FileData:    原始字节数据
    }
}
```

### 非 DI 用法

```csharp
using var client = UnionPayClient.Create(new UnionPayOptions { /* ... */ });

var declareResp = await client.Customs.DeclareAsync(new UnionPayCustomsDeclarationRequest
{
    OrderId     = "customs_001",
    TxnTime     = DateTime.Now.ToString("yyyyMMddHHmmss"),
    TxnAmt      = "10000",
    OrigQueryId = "orig_query_id",
    CustomsCode = "HANGZHOU"
});

var queryResp = await client.Customs.QueryDeclarationAsync(new UnionPayCustomsQueryRequest
{
    OrderId = "customs_001",
    TxnTime = declareResp.TxnTime
});

// 加密公钥更新查询
var keyResp = await client.Customs.QueryEncryptKeyAsync(new UnionPayEncryptKeyQueryRequest
{
    OrderId  = "key_001",
    TxnTime  = DateTime.Now.ToString("yyyyMMddHHmmss"),
    CertType = "01"
});
// keyResp.SignPubKeyCert → 替换本地加密公钥证书

// 实名认证
var authResp = await client.Customs.RealNameAuthAsync(new UnionPayRealNameAuthRequest
{
    OrderId      = "auth_001",
    TxnTime      = DateTime.Now.ToString("yyyyMMddHHmmss"),
    AccNo        = "6222021234567890",
    CustomerInfo = "Base64编码的持卡人信息"
});

// 文件传输（对账文件下载）
var fileResp = await client.Customs.FileTransferAsync(new UnionPayFileTransferRequest
{
    OrderId    = "file_001",
    TxnTime    = DateTime.Now.ToString("yyyyMMddHHmmss"),
    SettleDate = "0119",   // MMdd 格式
    FileType   = "00"
});
// fileResp.FileContent → 对账文件文本
```

---

## 配置选项参考

### WechatPayOptions

| 属性 | 类型 | 必填 | 说明 |
|------|------|:----:|------|
| `AppId` | string | ✅ | 应用 ID（公众号 / 小程序 / APP AppID） |
| `MchId` | string | ✅ | 商户号 |
| `ApiV3Key` | string | ✅ | API v3 密钥（32 字节，用于回调解密 AEAD_AES_256_GCM） |
| `PrivateKey` | string | ✅ | 商户私钥（PEM 格式） |
| `CertSerialNo` | string | ✅ | 商户证书序列号 |
| `PlatformPublicKey` | string | ⚠️ | 微信支付平台公钥 / 平台证书公钥（验签用，新版公钥模式必填） |
| `PlatformPublicKeyId` | string | — | 公钥 ID（`PUB_KEY_ID_xxxx`，新版公钥模式必填） |
| `NotifyUrl` | string | — | 支付结果异步通知回调地址（notify_url），配置后所有下单请求自动携带，也可在下单时覆盖 |
| `RefundNotifyUrl` | string | — | 退款结果异步通知回调地址，配置后退款请求自动携带，也可在退款时覆盖 |
| `BaseUrl` | string | — | API 基础地址，默认 `https://api.mch.weixin.qq.com` |
| `HttpTimeout` | TimeSpan | — | HTTP 超时，默认 30 秒 |

### AlipayOptions

| 属性 | 类型 | 必填 | 说明 |
|------|------|:----:|------|
| `AppId` | string | ✅ | 开放平台应用 ID |
| `PrivateKey` | string | ✅ | 商户 RSA2 私钥（PEM 或 Base64） |
| `AlipayPublicKey` | string | ✅ | 支付宝 RSA2 公钥（PEM 或 Base64，用于回调验签） |
| `NotifyUrl` | string | — | 异步通知回调地址（notify_url），配置后所有支付请求自动携带，也可在下单时覆盖 |
| `ReturnUrl` | string | — | 同步跳转地址（return_url），配置后手机网站 / 电脑网站支付自动携带，也可在下单时覆盖 |
| `SignType` | string | — | 签名类型，默认 `RSA2` |
| `GatewayUrl` | string | — | 网关地址，默认 `https://openapi.alipay.com/gateway.do` |
| `HttpTimeout` | TimeSpan | — | HTTP 超时，默认 30 秒 |

### UnionPayOptions

| 属性 | 类型 | 必填 | 说明 |
|------|------|:----:|------|
| `MerId` | string | ✅ | 商户号 |
| `PrivateKey` | string | ✅ | 商户 RSA 私钥（PEM） |
| `CertId` | string | ✅ | 商户证书序列号 |
| `UnionPayPublicKey` | string | ✅ | 银联根证书公钥（PEM，用于回调验签） |
| `FrontUrl` | string | ✅ | 前台通知 / 同步跳转地址 |
| `BackUrl` | string | ✅ | 后台通知 / 异步回调地址 |
| `FrontGatewayUrl` | string | — | 前台网关，默认 `https://gateway.95516.com/gateway/api/frontTransReq.do` |
| `AppGatewayUrl` | string | — | WAP 前台网关，默认 `https://gateway.95516.com/gateway/api/appTransReq.do` |
| `BackGatewayUrl` | string | — | 后台网关，默认 `https://gateway.95516.com/gateway/api/backTransReq.do` |
| `QueryGatewayUrl` | string | — | 查询网关，默认 `https://gateway.95516.com/gateway/api/queryTrans.do` |
| `FileGatewayUrl` | string | — | 文件下载网关，默认 `https://filedownload.95516.com/` |
| `Version` | string | — | 版本号，默认 `5.1.0` |
| `SignMethod` | string | — | 签名方式，`01`=RSA / `11`=SM2，默认 `01` |

---

## 错误处理

SDK 使用异常机制报告错误，所有支付异常都继承自 `PayException`：

```csharp
try
{
    var resp = await pay.CreateOrderAsync(request);
}
catch (PayException ex)
{
    // ex.ErrorCode    - 错误码（如 "PARAM_ERROR"）
    // ex.ErrorMessage - 错误信息
    // ex.Channel      - 发生错误的渠道（可选）
    Console.WriteLine($"[{ex.ErrorCode}] {ex.ErrorMessage}");
}
```

各渠道也有独立异常类型，包含更详细的渠道级错误信息：

| 异常类型 | 说明 |
|---------|------|
| `PayException` | SDK 统一基础异常 |
| `AlipayException` | 支付宝 API 业务错误 |
| `UnionPayException` | 银联 API 响应错误 |

---

## 项目结构

```
GaoXinLibrary.PaySDK/
├── Core/                           # 统一基础类型
│   ├── IPayService.cs              # 统一支付接口
│   ├── PayChannel.cs               # 渠道枚举（17 种子渠道，含 Apple Pay）
│   ├── PayChannelExtensions.cs     # 渠道枚举扩展方法
│   ├── PayException.cs             # 基础异常
│   ├── CreateOrderRequest.cs       # 创建订单请求
│   ├── CreateOrderResponse.cs      # 创建订单响应
│   ├── QueryOrderRequest.cs        # 查询订单请求
│   ├── QueryOrderResponse.cs       # 查询订单响应
│   ├── RefundRequest.cs            # 退款请求
│   ├── RefundResponse.cs           # 退款响应
│   ├── QueryRefundRequest.cs       # 退款查询请求
│   ├── QueryRefundResponse.cs      # 退款查询响应
│   ├── CloseOrderRequest.cs        # 关闭订单请求
│   ├── CloseOrderResponse.cs       # 关闭订单响应
│   ├── DownloadBillRequest.cs      # 账单下载请求
│   ├── PayCallbackResult.cs        # 回调解析结果
│   └── WechatJsPayParams.cs        # 微信 JS 调起支付参数
├── Wechat/                          # 微信支付 v3
│   ├── Core/                        # WechatPayOptions / WechatPaySigner / WechatPayHttpClient
│   ├── Models/                      # 所有微信支付请求/响应模型
│   │   ├── WechatAbnormalRefundRequest.cs    # 异常退款请求
│   │   ├── WechatAbnormalRefundResponse.cs   # 异常退款响应
│   │   └── ...                               # 其他支付/回调模型
│   ├── Services/                    # IWechatPayService / WechatPayService
│   └── WechatPayClient.cs          # 非 DI 场景的客户端入口
├── Alipay/                          # 支付宝
│   ├── Core/                        # AlipayOptions / AlipaySigner / AlipayHttpClient
│   ├── Models/                      # 所有支付宝请求/响应模型
│   ├── Services/                    # IAlipayService / AlipayService
│   └── AlipayClient.cs             # 非 DI 场景的客户端入口
├── UnionPay/                        # 银联
│   ├── Core/                        # UnionPayOptions / UnionPaySigner / UnionPayHttpClient
│   ├── Models/                      # 所有银联请求/响应模型
│   │   ├── UnionPayCustomsDeclarationRequest.cs   # 海关申报请求
│   │   ├── UnionPayCustomsQueryRequest.cs         # 海关申报查询请求
│   │   ├── UnionPayEncryptKeyQueryRequest.cs      # 加密公钥更新查询请求
│   │   ├── UnionPayEncryptKeyQueryResponse.cs     # 加密公钥更新查询响应
│   │   ├── UnionPayRealNameAuthRequest.cs         # 实名认证请求
│   │   ├── UnionPayRealNameAuthResponse.cs        # 实名认证响应
│   │   ├── UnionPayFileTransferRequest.cs         # 文件传输请求
│   │   ├── UnionPayFileTransferResponse.cs        # 文件传输响应
│   │   └── ...                                    # 其他支付/回调模型
│   ├── Services/                    # IUnionPayService / UnionPayService
│   │   ├── IUnionPayCustomsService.cs  # 海关申报接口（非支付）
│   │   └── UnionPayCustomsService.cs   # 海关申报实现
│   └── UnionPayClient.cs           # 非 DI 场景的客户端入口（Pay + Customs）
├── Extensions/                      # DI 注入扩展方法
│   ├── PayServiceCollectionExtensions.cs      # AddPaySDK / AddPayService
│   ├── WechatPayServiceCollectionExtensions.cs # AddWechatPay
│   ├── AlipayServiceCollectionExtensions.cs    # AddAlipay
│   └── UnionPayServiceCollectionExtensions.cs  # AddUnionPay
└── PayService.cs                    # IPayService 统一路由实现
```

---

## 许可证

MIT
