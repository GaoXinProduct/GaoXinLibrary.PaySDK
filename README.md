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
  - [合单支付](#合单支付)
  - [分账](#分账)
  - [商家转账到零钱](#商家转账到零钱)
- [支付宝扩展能力](#支付宝扩展能力分账--转账)
  - [分账查询与关系查询](#分账查询与关系查询)
  - [转账查询](#转账查询)
  - [交易投诉查询与反馈](#交易投诉查询与反馈)
- [银联独立接口](#银联独立接口)
  - [消费撤销](#消费撤销)
  - [预授权](#预授权)
  - [代收代付](#代收代付)
- [银联跨境电商海关申报](#银联跨境电商海关申报)
- [进阶用法](#进阶用法)
  - [瞬态故障自动重试](#瞬态故障自动重试)
  - [幂等重试支持](#幂等重试支持微信支付-v3)
  - [沙箱环境](#沙箱环境)
  - [分布式追踪](#分布式追踪)
  - [健康检查](#健康检查)
  - [JSON 序列化工具](#json-序列化工具)
  - [配置验证](#配置验证)
- [配置选项参考](#配置选项参考)
- [错误处理](#错误处理)
- [能力边界](#能力边界支持--不支持--规划中)
- [项目结构](#项目结构)
- [单元测试](#单元测试)

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
| 商家分账（关系绑定/订单分账） | — | ✅ | — |
| 商家转账 | — | ✅ | — |
| 合单支付 | ✅ | — | — |
| 分账申请/查询/退回 | ✅ | — | — |
| 商家转账到零钱 | ✅ | — | — |
| 分账查询 | — | ✅ | — |
| 分账关系批量查询 | — | ✅ | — |
| 转账查询 | — | ✅ | — |
| 交易投诉查询/反馈 | — | ✅ | — |
| 消费撤销 | — | — | ✅ |
| 预授权申请/撤销/完成/完成撤销 | — | — | ✅ |
| 代收 | — | — | ✅ |
| 代付/付款到银行卡 | — | — | ✅ |
| OpenAPI 独立模块（OAuth2/非对称） | — | — | ✅ |
| 健康检查（IHealthCheck） | ✅ | ✅ | ✅ |
| 分布式追踪（OpenTelemetry） | ✅ | ✅ | ✅ |
| 沙箱环境（Sandbox） | ✅ | ✅ | ✅ |

> *银联关闭订单：银联网关支付未支付订单自动超时关闭，SDK 的统一接口返回 `Success=true` + `IsSimulated=true`，并且 `OperationMode=Simulated` 以保持一致性，调用方可通过该字段识别是否真实调用了平台接口。

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
// result.IsSimulated: false（微信/支付宝为实际关闭）

// 银联网关支付不提供关闭订单 API，未支付订单会自动超时关闭
// SDK 返回 Success=true + IsSimulated=true 以保持统一接口一致性
var unionResult = await pay.CloseOrderAsync(new CloseOrderRequest
{
    Channel    = PayChannel.UnionPayGateway,
    OutTradeNo = "order_012"
});
// unionResult.Success:     true
// unionResult.IsSimulated: true（未实际调用银联 API）
// unionResult.OperationMode: Simulated（可机读识别模拟语义）
```

> **`IsSimulated` / `OperationMode` 标记说明**：当 `IsSimulated == true` 或 `OperationMode == Simulated` 时，表示 SDK 为保持接口一致性而返回的模拟成功，实际并未向支付平台发送关闭请求。调用方可根据该字段决定是否记录警告日志或做额外处理。

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
> 推荐使用 SDK 内置的 `PayJsonSerializer` 工具类（已配置 `JavaScriptEncoder.UnsafeRelaxedJsonEscaping` + snake_case）：
>
> ```csharp
> // ❌ 中文会显示为 \uXXXX
> var json = JsonSerializer.Serialize(order);
>
> // ✅ 推荐：使用 PayJsonSerializer（跨渠道通用）
> using GaoXinLibrary.PaySDK.Core;
> var json = PayJsonSerializer.Serialize(order);
>
> // ✅ 也可使用 WechatPayHttpClient.JsonOptions（仅微信支付场景）
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

## 回调防重放最佳实践

支付平台均使用异步通知机制推送回调，但**网络不可靠**：通知可能重复投递或延迟到达。业务系统必须在回调处理层实现**幂等处理**，避免重复发货、重复退款。

### 通用原则

1. **先验签，再幂等判断** — 签名无效直接拒绝，不进入业务逻辑。
2. **以平台单号为主键去重** — 每个支付订单的 `transaction_id` / `notify_id` / `queryId` 在支付周期内唯一。
3. **去重存储必须原子化** — 使用数据库唯一索引或分布式锁（Redis `SETNX`），避免竞态条件。
4. **老通知直接丢弃** — 通知时间戳明显过期（超过合理窗口），丢弃后主动查询最新状态。
5. **处理成功返回对应格式** — 微信返回 JSON、支付宝返回 `success`、银联返回 `ok`；返回了错误格式可能导致平台持续重发。

### 微信支付：时间戳窗口 + 签名唯一性

微信支付 v3 回调的三个签名头 `Wechatpay-Timestamp` / `Wechatpay-Nonce` / `Wechatpay-Signature` 本身就是防重放机制：nonce 在 5 分钟内有效，配合签名确保请求未被篡改。

```csharp
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

    // 步骤 1: 验签（SDK 内置，验证时间戳窗口 + 签名）
    var result = await pay.ParseCallbackAsync(PayChannel.WechatJsapi, body, headers);
    if (!result.IsValid)
        return BadRequest();

    // 步骤 2: 以 transaction_id 为主键做幂等处理
    var transactionId = result.TransactionId!;
    var idempotencyKey = $"wxpay_cb_{transactionId}";

    // 使用数据库唯一索引或 Redis SETNX 确保只处理一次
    if (!await TryAcquireIdempotencyLockAsync(idempotencyKey, TimeSpan.FromDays(7)))
        return Ok(new { code = "SUCCESS", message = "已处理" }); // 重复通知，直接返回成功

    try
    {
        // 步骤 3: 业务处理（发货、更新订单状态等）
        await ProcessOrderAsync(result.OutTradeNo, result.TransactionId, result.TradeStatus);
    }
    finally
    {
        // 步骤 4: 成功后释放锁（或保持锁防止后续重复通知重新处理）
    }

    // 步骤 5: 必须返回此格式，否则微信会持续重发
    return Ok(new { code = "SUCCESS", message = "成功" });
}
```

> **微信支付通知重试策略**：若商户未在 5 秒内返回 `{"code":"SUCCESS","message":"成功"}`，微信支付会按 15s/15s/30s/3m/10m/20m/30m/30m/30m/60m/3h/3h/6h/6h 间隔重试，最长持续 24 小时。因此幂等处理必须可靠。

### 支付宝：notify_id 去重

支付宝异步通知使用 `notify_id` 作为去重标识，每个通知的 `notify_id` 全局唯一。

```csharp
[HttpPost("alipay/notify")]
public async Task<IActionResult> AlipayNotify([FromServices] IPayService pay)
{
    var form = Request.Form.ToDictionary(k => k.Key, v => v.Value.ToString());
    var formString = string.Join("&",
        form.Select(kv => $"{Uri.EscapeDataString(kv.Key)}={Uri.EscapeDataString(kv.Value)}"));

    // 步骤 1: 验签
    var result = await pay.ParseCallbackAsync(PayChannel.AlipayPage, formString);
    if (!result.IsValid)
        return BadRequest();

    // 步骤 2: 获取 notify_id 并去重
    if (!form.TryGetValue("notify_id", out var notifyId) || string.IsNullOrEmpty(notifyId))
        return BadRequest();

    if (!await TryAcquireIdempotencyLockAsync($"alipay_cb_{notifyId}", TimeSpan.FromDays(7)))
        return Content("success", "text/plain"); // 重复通知

    try
    {
        await ProcessOrderAsync(result.OutTradeNo, result.TransactionId, result.TradeStatus);
    }
    finally { }

    return Content("success", "text/plain");
}
```

> **支付宝 async_req 主动查询**：支付宝异步通知不携带 `biz_content`，只包含 `notify_type=fund_auth_freeze` 等提示信息。此时应使用 `notify_type` 识别事件，主动调用查询接口 (`alipay.trade.query`) 确认订单状态，详见 [支付宝异步通知说明](https://opendocs.alipay.com/open/270/105902)。

### 银联：orderId + queryId 联合去重

银联回调采用 `queryId`（交易流水号）作为主去重键，每次交易的 `queryId` 唯一。`orderId`（商户订单号）可作为辅助校验。

```csharp
[HttpPost("unionpay/notify")]
public async Task<IActionResult> UnionPayNotify([FromServices] IPayService pay)
{
    var form = Request.Form.ToDictionary(k => k.Key, v => v.Value.ToString());
    var formString = string.Join("&",
        form.Select(kv => $"{Uri.EscapeDataString(kv.Key)}={Uri.EscapeDataString(kv.Value)}"));

    // 步骤 1: 验签
    var result = await pay.ParseCallbackAsync(PayChannel.UnionPayGateway, formString);
    if (!result.IsValid)
        return BadRequest();

    // 步骤 2: 以 queryId 为主键做幂等处理
    var queryId = result.TransactionId!;
    if (!await TryAcquireIdempotencyLockAsync($"unionpay_cb_{queryId}", TimeSpan.FromDays(7)))
        return Content("ok", "text/plain"); // 重复通知

    try
    {
        await ProcessOrderAsync(result.OutTradeNo, queryId, result.TradeStatus);
    }
    finally { }

    return Content("ok", "text/plain");
}
```

### 建议的去重存储实现

| 存储方案 | 实现方式 | 适用场景 |
|---------|---------|---------|
| 数据库唯一索引 | `INSERT INTO callback_log (notify_id, status, created_at) VALUES (@id, 'processing', NOW())` — 依赖唯一索引抛异常回滚 | 小型项目，已有数据库 |
| Redis `SETNX` | `SET alipay_cb_{notify_id} 1 NX EX 604800` — 原子操作，自动过期 | 高并发，建议 7 天过期 |
| ConcurrentDictionary | 仅适合单体应用、单实例、重启后丢失的场景 | 开发/测试环境 |

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

    // 手动解密（用于自定义场景的下行加密敏感字段）
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

> **自动解密说明**：`RefundAsync`、`QueryRefundAsync`、`ApplyAbnormalRefundAsync` 返回的 `UserReceivedAccount` 已自动尝试解密。若平台返回明文或掩码，SDK 会保留原值并继续返回，不会抛异常。

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

### 支付宝扩展能力（分账 / 转账）

```csharp
public class AlipayAdvancedController(IAlipayService alipay)
{
    // 1) 绑定分账关系（一次绑定，多次分账）
    public async Task BindRoyaltyAsync()
    {
        await alipay.BindRoyaltyRelationAsync(new AlipayTradeRoyaltyRelationBindContent
        {
            OutRequestNo = "royalty_bind_001",
            TransOut = "2088xxxxxx_out",
            TransIn = "2088xxxxxx_in",
            TransInType = "userId",
            Type = "transfer",
            Desc = "平台分账关系"
        });
    }

    // 2) 对已支付订单发起分账
    public async Task SettleAsync(string outTradeNo)
    {
        await alipay.SettleOrderAsync(new AlipayTradeOrderSettleContent
        {
            OutTradeNo = outTradeNo,
            OutRequestNo = $"settle_{outTradeNo}",
            RoyaltyParameters =
            [
                new AlipayRoyaltyDetail
                {
                    TransIn = "2088xxxxxx_in",
                    TransInType = "userId",
                    Amount = "0.30",
                    Desc = "平台服务费"
                }
            ]
        });
    }

    // 3) 商家转账
    public async Task TransferAsync()
    {
        await alipay.TransferAsync(new AlipayFundTransUniTransferContent
        {
            OutBizNo = "transfer_001",
            TransAmount = "1.00",
            PayeeInfoIdentity = "2088xxxxxx_user",
            PayeeInfoIdentityType = "ALIPAY_USER_ID",
            BizScene = "DIRECT_TRANSFER",
            Remark = "活动补贴"
        });
    }
}
```

### 分账查询与关系查询

```csharp
public class AlipaySettleQueryController(IAlipayService alipay)
{
    // 分账查询（通过分账请求号或交易号查询分账结果）
    public async Task<AlipayTradeOrderSettleQueryResponse> QuerySettleOrderAsync()
    {
        return await alipay.QuerySettleOrderAsync(new AlipayTradeOrderSettleQueryContent
        {
            OutRequestNo = "settle_order_001"  // 分账请求号
            // 或 TradeNo = "2021xxxxxx"       // 支付宝交易号
        });
        // resp.RoyaltyDetailList: 分账明细列表
    }

    // 分账关系批量查询（查询已绑定的分账接收方列表）
    public async Task<AlipayTradeRoyaltyRelationBatchQueryResponse> QueryRoyaltyRelationAsync()
    {
        return await alipay.QueryRoyaltyRelationAsync(new AlipayTradeRoyaltyRelationBatchQueryContent
        {
            PageNum  = 1,      // 页码
            PageSize = 20      // 每页条数
        });
        // resp.ReceiverList: 分账接收方列表
        // resp.TotalPageNum / resp.CurrentPageNum: 分页信息
    }
}
```

### 转账查询

```csharp
public class AlipayTransferQueryController(IAlipayService alipay)
{
    // 查询转账结果（alipay.fund.trans.common.query）
    public async Task<AlipayFundTransCommonQueryResponse> QueryTransferAsync()
    {
        return await alipay.QueryTransferAsync(new AlipayFundTransCommonQueryContent
        {
            OutBizNo    = "transfer_001",       // 商户转账单号
            // 或 OrderId = "2021xxxxxx"        // 支付宝转账单据号
            BizScene    = "DIRECT_TRANSFER"     // 业务场景
        });
        // resp.Status: SUCCESS / FAIL / PROCESSING
        // resp.Amount / resp.PayeeInfo / resp.OrderFee
    }
}
```

### 交易投诉查询与反馈

```csharp
public class AlipayComplainController(IAlipayService alipay)
{
    // 批量查询交易投诉列表
    public async Task<AlipayTradeComplainQueryResponse> QueryComplaintsAsync()
    {
        return await alipay.QueryComplaintsAsync(new AlipayTradeComplainQueryContent
        {
            Status    = "WAIT_FEEDBACK",          // 投诉状态：WAIT_FEEDBACK / FEEDBACKED
            PageNum   = 1,
            PageSize  = 20,
            StartTime = "2025-01-01 00:00:00",
            EndTime   = "2025-06-01 23:59:59"
        });
        // resp.ComplainList: 投诉列表
        // resp.TotalNum / resp.PageNum: 分页信息
    }

    // 提交投诉反馈
    public async Task<AlipayTradeComplainFeedbackResponse> FeedbackComplaintAsync(
        string complainEventId, string content)
    {
        return await alipay.FeedbackComplaintAsync(new AlipayTradeComplainFeedbackContent
        {
            ComplainEventId = complainEventId,            // 支付宝侧投诉单号
            Content         = content,                     // 反馈内容（最多 200 字）
            Images          = "img_id_1,img_id_2"          // 凭证图片 ID（可选，逗号分隔）
        });
        // resp.ResultCode: "SUCCESS" = 反馈提交成功
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

    // ── 消费撤销 ──────────────────────────────────────────────
    // 撤销已完成的消费交易（仅限当日交易），发起前需确认订单状态
    public async Task<UnionPayConsumeUndoResponse> ConsumeUndoAsync(
        string orgQueryId, int txnAmt)
    {
        return await unionPay.ConsumeUndoAsync(new UnionPayConsumeUndoRequest
        {
            OrderId     = $"undo_{DateTime.Now:yyyyMMddHHmmss}",
            TxnTime     = DateTime.Now.ToString("yyyyMMddHHmmss"),
            TxnAmt      = txnAmt.ToString(),
            OrigQueryId = orgQueryId        // 原始消费交易的 queryId
        });
        // resp.OrigRespCode: "00" = 撤销成功
    }

    // ── 预授权 ────────────────────────────────────────────────
    // 预授权申请：冻结用户卡内资金，不实际扣款
    public async Task<UnionPayPreAuthResponse> PreAuthAsync(
        string orderId, string accNo, string customerInfo, int txnAmt)
    {
        return await unionPay.PreAuthAsync(new UnionPayPreAuthRequest
        {
            OrderId      = orderId,
            TxnTime      = DateTime.Now.ToString("yyyyMMddHHmmss"),
            TxnAmt       = txnAmt.ToString(),
            OrderDesc    = "酒店预授权",
            AccNo        = accNo,           // 卡号（需加密上送）
            CustomerInfo = customerInfo,     // 持卡人信息（需加密）
            BackUrl      = "https://your-site.com/pay/unionpay/notify"
        });
    }

    // 预授权撤销：撤销未完成的预授权交易
    public async Task<UnionPayPreAuthUndoResponse> PreAuthUndoAsync(
        string orderId, string origQueryId, int txnAmt)
    {
        return await unionPay.PreAuthUndoAsync(new UnionPayPreAuthUndoRequest
        {
            OrderId     = orderId,
            TxnTime     = DateTime.Now.ToString("yyyyMMddHHmmss"),
            TxnAmt      = txnAmt.ToString(),
            OrigQueryId = origQueryId       // 原始预授权 queryId
        });
    }

    // 预授权完成：对已预授权的订单发起实际扣款
    public async Task<UnionPayPreAuthCompleteResponse> PreAuthCompleteAsync(
        string orderId, string origQueryId, int txnAmt)
    {
        return await unionPay.PreAuthCompleteAsync(new UnionPayPreAuthCompleteRequest
        {
            OrderId     = orderId,
            TxnTime     = DateTime.Now.ToString("yyyyMMddHHmmss"),
            TxnAmt      = txnAmt.ToString(),    // 完成金额可小于等于原预授权金额
            OrigQueryId = origQueryId
        });
    }

    // 预授权完成撤销：撤销已完成的预授权扣款
    public async Task<UnionPayPreAuthCompleteUndoResponse> PreAuthCompleteUndoAsync(
        string orderId, string origQueryId, int txnAmt)
    {
        return await unionPay.PreAuthCompleteUndoAsync(new UnionPayPreAuthCompleteUndoRequest
        {
            OrderId     = orderId,
            TxnTime     = DateTime.Now.ToString("yyyyMMddHHmmss"),
            TxnAmt      = txnAmt.ToString(),
            OrigQueryId = origQueryId       // 原始预授权完成的 queryId
        });
    }

    // ── 代收 ─────────────────────────────────────────────────
    // 从用户银行卡扣款至商户（无跳转后台消费）
    public async Task<UnionPayCollectionResponse> CollectionAsync(
        string orderId, string accNo, string customerInfo, int txnAmt)
    {
        return await unionPay.CollectionAsync(new UnionPayCollectionRequest
        {
            OrderId      = orderId,
            TxnTime      = DateTime.Now.ToString("yyyyMMddHHmmss"),
            TxnAmt       = txnAmt.ToString(),
            AccNo        = accNo,           // 卡号（需加密上送）
            CustomerInfo = customerInfo,     // 持卡人信息（需加密）
            BackUrl      = "https://your-site.com/pay/unionpay/notify"
        });
    }

    // ── 代付（付款到银行卡）───────────────────────────────────
    // 从商户账户打款至用户银行卡
    public async Task<UnionPayPaymentResponse> PayToBankCardAsync(
        string orderId, string accNo, string customerInfo, int txnAmt)
    {
        return await unionPay.PayToBankCardAsync(new UnionPayPaymentRequest
        {
            OrderId      = orderId,
            TxnTime      = DateTime.Now.ToString("yyyyMMddHHmmss"),
            TxnAmt       = txnAmt.ToString(),
            AccNo        = accNo,           // 收款卡号（需加密上送）
            CustomerInfo = customerInfo,     // 收款人信息（需加密）
            BackUrl      = "https://your-site.com/pay/unionpay/notify"
        });
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

### 合单支付

合单支付用于将多笔子订单合并为一笔向用户收款，适用于购物车多商户、连锁门店等多场景。每笔合单最多支持 10 笔子订单，子订单金额累加为合单总金额，用户支付后资金会自动分配至各子商户。

```csharp
public class WechatCombineController(IWechatPayService wechat)
{
    // 合单 JSAPI 下单（公众号/小程序内）
    public async Task<WechatJsPayParams> CombineJsapiPayAsync(string openId)
    {
        var resp = await wechat.CreateCombineJsapiOrderAsync(new WechatCombineOrderRequest
        {
            CombineAppId       = "wx_your_appid",
            CombineMchId       = "1600000000",
            CombineOutTradeNo  = "combine_order_001",
            NotifyUrl          = "https://your-site.com/pay/wechat/notify",
            SubOrders =
            [
                new WechatCombineSubOrder
                {
                    MchId       = "1600000000",
                    OutTradeNo  = "sub_order_001",
                    Description = "商品A",
                    Amount      = new WechatCombineSubOrderAmount { TotalAmount = 100, Currency = "CNY" }
                },
                new WechatCombineSubOrder
                {
                    MchId       = "1600000001",
                    OutTradeNo  = "sub_order_002",
                    Description = "商品B",
                    Amount      = new WechatCombineSubOrderAmount { TotalAmount = 200, Currency = "CNY" }
                }
            ],
            CombinePayerInfo = new WechatCombinePayerInfo { OpenId = openId }
        });
        return wechat.BuildJsPayParams(resp.PrepayId);
    }

    // 合单 APP 下单
    public async Task<WechatAppPayParams> CombineAppPayAsync()
    {
        var resp = await wechat.CreateCombineAppOrderAsync(new WechatCombineOrderRequest
        {
            CombineAppId       = "wx_your_appid",
            CombineMchId       = "1600000000",
            CombineOutTradeNo  = "combine_order_002",
            NotifyUrl          = "https://your-site.com/pay/wechat/notify",
            SubOrders =
            [
                new WechatCombineSubOrder
                {
                    MchId       = "1600000000",
                    OutTradeNo  = "sub_order_003",
                    Description = "商品A",
                    Amount      = new WechatCombineSubOrderAmount { TotalAmount = 100 }
                }
            ]
        });
        return wechat.BuildAppPayParams(resp.PrepayId);
    }

    // 合单 H5 下单
    public async Task<string> CombineH5PayAsync(string clientIp)
    {
        var resp = await wechat.CreateCombineH5OrderAsync(new WechatCombineOrderRequest
        {
            CombineAppId       = "wx_your_appid",
            CombineMchId       = "1600000000",
            CombineOutTradeNo  = "combine_order_003",
            NotifyUrl          = "https://your-site.com/pay/wechat/notify",
            SubOrders =
            [
                new WechatCombineSubOrder
                {
                    MchId       = "1600000000",
                    OutTradeNo  = "sub_order_004",
                    Description = "商品A",
                    Amount      = new WechatCombineSubOrderAmount { TotalAmount = 100 }
                }
            ],
            SceneInfo = new WechatPaySceneInfo { PayerClientIp = clientIp }
        });
        return resp.H5Url;
    }

    // 合单 Native 下单（扫码支付）
    public async Task<string> CombineNativePayAsync()
    {
        var resp = await wechat.CreateCombineNativeOrderAsync(new WechatCombineOrderRequest
        {
            CombineAppId       = "wx_your_appid",
            CombineMchId       = "1600000000",
            CombineOutTradeNo  = "combine_order_004",
            NotifyUrl          = "https://your-site.com/pay/wechat/notify",
            SubOrders =
            [
                new WechatCombineSubOrder
                {
                    MchId       = "1600000000",
                    OutTradeNo  = "sub_order_005",
                    Description = "商品A",
                    Amount      = new WechatCombineSubOrderAmount { TotalAmount = 300 }
                }
            ]
        });
        return resp.CodeUrl;
    }

    // 合单查询
    public async Task<WechatCombineQueryResponse> QueryCombineAsync(string combineOutTradeNo)
        => await wechat.QueryCombineOrderAsync(combineOutTradeNo);

    // 合单关闭
    public async Task CloseCombineAsync(string combineOutTradeNo)
    {
        await wechat.CloseCombineOrderAsync(combineOutTradeNo, new WechatCombineCloseRequest
        {
            CombineAppId = "wx_your_appid",
            SubOrders =
            [
                new WechatCombineCloseSubOrder
                {
                    MchId      = "1600000000",
                    OutTradeNo = "sub_order_001"
                }
            ]
        });
    }
}
```

> **子订单说明**：每笔子订单的 `mchid` 必须是发起下单的合单商户号或其子商户号。每个子订单独立关联 `out_trade_no`、`description` 和 `amount`，最多 10 笔。

---

### 分账

分账用于将已支付的订单金额按比例分配给多个接收方（如平台方、服务商、子商户等），支持冻结后解冻、分账回退等操作。

```csharp
public class WechatProfitSharingController(IWechatPayService wechat)
{
    // 请求分账
    public async Task<WechatProfitSharingResponse> CreateProfitSharingAsync(
        string transactionId, string outOrderNo)
    {
        return await wechat.CreateProfitSharingAsync(new WechatProfitSharingRequest
        {
            AppId         = "wx_your_appid",
            TransactionId = transactionId,       // 微信支付订单号
            OutOrderNo    = outOrderNo,           // 商户分账单号
            Receivers =
            [
                new WechatProfitSharingReceiver
                {
                    Type        = "MERCHANT_ID",
                    Account     = "1600000002",    // 分账接收方商户号
                    Amount      = 10,               // 分账金额（分）
                    Description = "平台服务费"
                },
                new WechatProfitSharingReceiver
                {
                    Type        = "MERCHANT_ID",
                    Account     = "1600000003",
                    Amount      = 30,
                    Description = "分店分账"
                }
            ],
            UnfreezeUnsplit = false   // true = 解冻剩余未分账资金
        });
    }

    // 查询分账结果
    public async Task<WechatProfitSharingQueryResponse> QueryProfitSharingAsync(
        string outOrderNo, string transactionId)
        => await wechat.QueryProfitSharingAsync(outOrderNo, transactionId);

    // 请求分账回退
    public async Task<WechatProfitSharingReturnResponse> ReturnProfitSharingAsync(
        string outOrderNo, string outReturnNo)
    {
        return await wechat.ReturnProfitSharingAsync(new WechatProfitSharingReturnRequest
        {
            OutOrderNo     = outOrderNo,
            OutReturnNo    = outReturnNo,
            ReturnMchId    = "1600000002",     // 回退方商户号
            Amount         = 5,                 // 回退金额（分）
            Description    = "分账回退",
            OrderId        = outOrderNo         // 原分账单号
        });
    }

    // 查询分账回退结果
    public async Task<WechatProfitSharingReturnQueryResponse> QueryReturnAsync(
        string outReturnNo, string outOrderNo)
        => await wechat.QueryProfitSharingReturnAsync(outReturnNo, outOrderNo);
}
```

> **资金解冻周期**：用户支付成功后资金默认冻结 30 天（可通过 `settle_info.profit_sharing` 参数延长），在冻结期内需完成分账。若设置 `UnfreezeUnsplit = true`，SDK 会在分账同时解冻剩余资金。

---

### 商家转账到零钱

商家转账到零钱用于向指定用户（通过 OpenId 识别）的微信零钱账户发起转账，适用于企业付款、红包发放、佣金结算等场景。

> **💡 UserName 加密说明**：如果提供收款用户姓名（`UserName`），需要先使用 `EncryptSensitiveField` 加密后再传入。SDK 在内部已预留加密字段，实际传输时需自行调用加密方法获得密文。

```csharp
public class WechatTransferController(IWechatPayService wechat)
{
    // 发起商家转账到零钱
    public async Task<WechatTransferBillsResponse> TransferToWalletAsync(
        string openId, int amount)
    {
        return await wechat.TransferBillsAsync(new WechatTransferBillsRequest
        {
            AppId           = "wx_your_appid",
            OutBillNo       = $"transfer_{DateTime.Now:yyyyMMddHHmmssfff}",
            TransferSceneId = "1000",                   // 转账场景 ID
            OpenId          = openId,                    // 收款用户 openid
            UserName        = wechat.EncryptSensitiveField("张三"),  // 收款用户姓名（需加密）
            TransferAmount  = amount,                    // 转账金额（分）
            TransferRemark  = "活动补贴",
            NotifyUrl       = "https://your-site.com/pay/wechat/transfer-notify"
        });
    }

    // 查询转账结果
    public async Task<WechatTransferBillQueryResponse> QueryTransferAsync(string outBillNo)
        => await wechat.QueryTransferBillAsync(outBillNo);
}
```

> **转账场景 ID**：不同业务场景对应不同的 `TransferSceneId`（如 `1000` = 营销活动），需向微信支付申请开通。若不传 `UserName`，SDK 将仅凭 openid 进行校验转账。

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

## 银联 OpenAPI 独立模块（OAuth2 / 非对称）

银联 OpenAPI 与收单交易接口不是同一能力域。SDK 已提供独立的 `IUnionPayOpenApiService` 与 `AddUnionPayOpenApi`，避免把 OpenAPI 认证流程（OAuth2/非对称）与支付下单接口耦合在一起。

```csharp
// Program.cs
builder.Services.AddUnionPayOpenApi(opt =>
{
    opt.BaseUrl = "https://openapi.unionpay.com";
    opt.AppId = "your_openapi_appid";

    // 二选一：
    // 1) OAuth2
    // opt.AuthMode = UnionPayOpenApiAuthMode.OAuth2;
    // opt.OAuthToken = "your_access_token";

    // 2) 非对称验签（RSA2）
    opt.AuthMode = UnionPayOpenApiAuthMode.Asymmetric;
    opt.PrivateKey = "-----BEGIN PRIVATE KEY-----\n...\n-----END PRIVATE KEY-----";
});

// 使用
public class UnionPayOpenApiController(IUnionPayOpenApiService openApi)
{
    public async Task<string> QueryDemoAsync()
    {
        return await openApi.PostAsync("your.biz.method", new { foo = "bar" });
    }
}
```

> 该模块定位为 OpenAPI 认证与请求骨架，不改变现有收单支付接口行为。你可以在其上按产品文档逐步扩展具体 OpenAPI 能力。

---

## 进阶用法

### 瞬态故障自动重试

SDK 内置了瞬态故障自动重试机制，当遇到网络抖动、连接超时、服务端 5xx 等临时性故障时，会按指数退避策略自动重试。此机制适用于所有三个支付渠道（微信支付、支付宝、银联）。

**可重试的故障类型：**
- 网络层错误（连接失败、DNS 解析失败等 `HttpRequestException`）
- HTTP 请求超时（`TaskCanceledException`，非用户主动取消）
- 服务端错误（HTTP 5xx 状态码）

**配置参数 — `PayRetryOptions`：**

| 属性 | 类型 | 默认值 | 说明 |
|------|------|--------|------|
| `MaxRetries` | `int` | `2` | 最大重试次数（不含首次请求），设为 0 则不重试 |
| `InitialDelay` | `TimeSpan` | 500ms | 首次重试前的等待时间，后续按指数退避递增 |
| `MaxDelay` | `TimeSpan` | 5s | 单次重试等待时间上限 |

```csharp
// 使用默认重试配置（2 次重试，500ms 起始延迟，指数退避）
builder.Services.AddWechatPay(opt =>
{
    opt.AppId        = "wx_your_appid";
    opt.MchId        = "1600000000";
    opt.ApiV3Key     = "your_api_v3_key";
    opt.PrivateKey   = "your_private_key";
    opt.CertSerialNo = "your_serial_no";
    // RetryOptions 默认已启用，无需额外配置
});

// 自定义重试策略
builder.Services.AddAlipay(opt =>
{
    opt.AppId           = "2021000000000000";
    opt.PrivateKey      = "your_private_key";
    opt.AlipayPublicKey = "alipay_public_key";
    opt.RetryOptions = new PayRetryOptions
    {
        MaxRetries   = 3,                              // 最多重试 3 次
        InitialDelay = TimeSpan.FromMilliseconds(200),  // 首次重试等待 200ms
        MaxDelay     = TimeSpan.FromSeconds(10)          // 单次最多等待 10s
    };
});

// 禁用重试
builder.Services.AddUnionPay(opt =>
{
    opt.MerId             = "your_mer_id";
    opt.CertId            = "your_cert_id";
    opt.PrivateKey        = "your_private_key";
    opt.UnionPayPublicKey = "unionpay_public_key";
    opt.RetryOptions = new PayRetryOptions { MaxRetries = 0 };
});
```

### 幂等重试支持（微信支付 v3）

微信支付 v3 接口支持 `Idempotency-Key` 请求头，用于防止因网络重试导致的重复扣款。SDK 在所有 POST 方法中暴露了可选的 `idempotencyKey` 参数：

```csharp
// 通过独立接口使用幂等键
public class WechatController(IWechatPayService wechat)
{
    public async Task<WechatNativeOrderResponse> SafeCreateOrder()
    {
        // 使用商户订单号作为幂等键，确保同一订单不会重复创建
        var orderId = "order_001";
        var resp = await wechat.CreateNativeOrderAsync(new WechatNativeOrderRequest
        {
            OutTradeNo  = orderId,
            Description = "商品描述",
            Amount      = new WechatPayAmount { Total = 100 }
        });
        return resp;
    }
}
```

> **💡 说明**：`Idempotency-Key` 在 `WechatPayHttpClient` 的 `PostAsync`、`PostNoContentAsync`、`PostWithEncryptionAsync` 方法中均可通过参数传入。瞬态重试机制独立于幂等键工作 — 瞬态重试针对的是网络层故障（在请求未到达微信服务器时安全重试），而幂等键则保护已到达服务器的请求不被重复处理。

### 沙箱环境（Sandbox Mode）

SDK 支持通过 `Environment` 属性切换生产环境与沙箱环境，方便开发测试。所有三个渠道的 Options 类均提供 `Environment` 配置项：

```csharp
// 微信支付：使用测试商户号（微信支付无公开沙箱，但 SDK 预留了切换能力）
builder.Services.AddWechatPay(opt =>
{
    opt.AppId       = "wx_your_appid";
    opt.MchId       = "1600000000";
    opt.ApiV3Key    = "your_api_v3_key";
    opt.PrivateKey  = "your_private_key";
    opt.CertSerialNo = "your_serial_no";
    opt.Environment = PayEnvironment.Sandbox;  // 设置为沙箱模式
});

// 支付宝：自动切换到沙箱网关 https://openapi-sandbox.dl.alipaydev.com/gateway.do
builder.Services.AddAlipay(opt =>
{
    opt.AppId           = "2021000000000000";
    opt.PrivateKey      = "your_private_key";
    opt.AlipayPublicKey = "alipay_public_key";
    opt.Environment     = PayEnvironment.Sandbox;
});

// 银联：Production / Sandbox 通过 Environment 切换
builder.Services.AddUnionPay(opt =>
{
    opt.MerId             = "your_test_mer_id";
    opt.Environment       = PayEnvironment.Sandbox;
    // 其余配置保持不变
});
```

> **沙箱说明**：支付宝沙箱网关为 `openapi-sandbox.dl.alipaydev.com`，SDK 在 `PayEnvironment.Sandbox` 下自动切换。微信支付和银联的沙箱环境需向对应平台申请测试商户号，SDK 通过 `Environment` 标记业务意图，具体网关地址由平台方提供。

### 分布式追踪（OpenTelemetry）

SDK 内置了 OpenTelemetry 兼容的 `ActivitySource`，所有支付渠道的 API 调用均会创建 `Activity` Span，便于在 APM 系统（如 Jaeger、Zipkin、Azure Monitor 等）中追踪支付链路：

```csharp
using GaoXinLibrary.PaySDK.Core;
using OpenTelemetry.Trace;

// Program.cs 中注册 OpenTelemetry
builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddSource(PayActivitySource.Source.Name)  // "GaoXinLibrary.PaySDK"
        .AddJaegerExporter());                     // 或其他 Exporter

// 业务代码中无需额外处理，SDK 自动为每次 API 调用创建 Span
// 包含 channel、method、out_trade_no 等标签
```

> **ActivitySource 名称**：`GaoXinLibrary.PaySDK`（可在类 `PayActivitySource.Source.Name` 中获取）。每个支付 API 调用都会自动创建对应的 Span，标签包括 `channel`、`method`、`out_trade_no` 等核心字段。

### 健康检查（Health Check）

SDK 提供了标准的 ASP.NET Core 健康检查端点，用于监控各支付渠道的配置状态：

```csharp
// Program.cs
builder.Services
    .AddHealthChecks()
    .AddPayHealthChecks();   // 添加 PaySDK 健康检查（tag: "payment"）

// app.MapHealthChecks 或自定义端点...
app.MapHealthChecks("/health");

// 返回示例：
// {
//   "status": "Healthy",
//   "results": {
//     "pay_sdk": {
//       "status": "Healthy",
//       "description": "PaySDK is operational",
//       "data": {
//         "channels.wechat": "configured",
//         "channels.alipay": "configured",
//         "channels.unionpay": "configured",
//         "sdk.version": "1.0.0"
//       }
//     }
//   }
// }
```

> **依赖前提**：`AddPayHealthChecks` 需在 `IPayService` 已注册的前提下使用（先调用 `AddPaySDK` 或 `AddPayService`）。未注册 `IPayService` 时健康检查仍返回 Healthy，但各渠道状态为 `not_configured`。

### 统一日志追踪（推荐）

`PayService` 已补充统一日志字段，建议在业务层补齐 `requestId` 并在落单/回调时记录以下字段：`channel`、`outTradeNo`、`requestId`、`tradeStatus`、`errorCode`。

```csharp
// Program.cs
app.Use(async (ctx, next) =>
{
    var requestId = ctx.Request.Headers.TryGetValue("X-Request-Id", out var v) && !string.IsNullOrWhiteSpace(v)
        ? v.ToString()
        : Guid.NewGuid().ToString("N");

    ctx.Items["requestId"] = requestId;
    ctx.Response.Headers["X-Request-Id"] = requestId;
    await next();
});
```

> 建议在 Controller 中将 `channel/outTradeNo/tradeStatus/errorCode` 与 `requestId` 一并写入结构化日志，便于对账和问题排查。

### JSON 序列化工具

`PayJsonSerializer` 提供 SDK 预配置的 JSON 序列化选项，适用于所有支付渠道的日志记录、调试输出等场景：

```csharp
using GaoXinLibrary.PaySDK.Core;

// 序列化（中文不会被转义为 \uXXXX，使用 snake_case 命名）
var json = PayJsonSerializer.Serialize(myObject);

// 反序列化
var obj = PayJsonSerializer.Deserialize<MyModel>(json);

// 直接获取 JsonSerializerOptions 用于自定义场景
var options = PayJsonSerializer.Options;
```

**预配置选项：**
- 命名策略：`JsonNamingPolicy.SnakeCaseLower`（自动 snake_case）
- 空值处理：`JsonIgnoreCondition.WhenWritingNull`（忽略 null 字段）
- 编码器：`JavaScriptEncoder.UnsafeRelaxedJsonEscaping`（中文直接输出，不做 Unicode 转义）

### 配置验证

SDK 在 DI 注册时会自动校验必填配置项。所有 Options 类的必填属性均标注了 `[Required]` 特性，注册时通过 `Validator.ValidateObject` 提前验证，而不是等到第一次 API 调用时才报错：

```csharp
// ❌ 缺少必填项，注册时立即抛出 ValidationException
builder.Services.AddWechatPay(opt =>
{
    opt.AppId = "wx_appid";
    // 未设置 MchId、ApiV3Key、PrivateKey、CertSerialNo
});
// 抛出: ValidationException: "微信支付 MchId 不能为空"

// ✅ 所有必填项已配置
builder.Services.AddWechatPay(opt =>
{
    opt.AppId        = "wx_appid";
    opt.MchId        = "1600000000";
    opt.ApiV3Key     = "your_api_v3_key";
    opt.PrivateKey   = "your_private_key";
    opt.CertSerialNo = "your_serial_no";
});
```

各渠道必填项：

| 渠道 | 必填属性 |
|------|----------|
| 微信支付 | `AppId`、`MchId`、`ApiV3Key`、`PrivateKey`、`CertSerialNo` |
| 支付宝 | `AppId`、`PrivateKey`、`AlipayPublicKey` |
| 银联 | `MerId`、`PrivateKey`、`CertId`、`UnionPayPublicKey` |

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
| `RetryOptions` | `PayRetryOptions` | — | 瞬态故障重试配置（默认 2 次重试，500ms 起始延迟），详见[瞬态故障自动重试](#瞬态故障自动重试) |

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
| `RetryOptions` | `PayRetryOptions` | — | 瞬态故障重试配置（默认 2 次重试，500ms 起始延迟），详见[瞬态故障自动重试](#瞬态故障自动重试) |

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
| `RetryOptions` | `PayRetryOptions` | — | 瞬态故障重试配置（默认 2 次重试，500ms 起始延迟），详见[瞬态故障自动重试](#瞬态故障自动重试) |

> **💡 配置验证**：所有标注为 ✅ 必填的属性均在 DI 注册时通过 `[Required]` + `Validator.ValidateObject` 自动校验，缺失时立即抛出 `ValidationException`，而非等到首次 API 调用时才报错。

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

## 能力边界（支持 / 不支持 / 规划中）

| 分类 | 能力 | 状态 | 说明 |
|------|------|------|------|
| 微信支付 | JSAPI/APP/H5/Native/小程序、退款、退款查询、回调验签解密 | ✅ | 已支持 |
| 微信支付 | 异常退款、平台证书下载注册、敏感字段加解密 | ✅ | 已支持 |
| 微信支付 | 合单支付（JSAPI/APP/H5/Native 下单、查询、关闭） | ✅ | 本版本新增 |
| 微信支付 | 分账（请求分账、分账查询、分账回退、回退查询） | ✅ | 本版本新增 |
| 微信支付 | 商家转账到零钱、转账查询 | ✅ | 本版本新增 |
| 微信支付 | 花呗分期、支付券 | 🚧 | 规划中，按业务优先级扩展 |
| 微信支付 | 微信支付分 | 🚧 | 规划中 |
| 支付宝 | 收单核心链路（当面付/预下单/JSAPI/App/WAP/Page） | ✅ | 已支持 |
| 支付宝 | 分账关系绑定、订单分账、商家转账 | ✅ | 已支持 |
| 支付宝 | 分账查询、分账关系批量查询、转账查询 | ✅ | 本版本新增 |
| 支付宝 | 交易投诉查询与反馈 | ✅ | 本版本新增 |
| 支付宝 | 花呗分期、RiskGO | 🚧 | 规划中 |
| 银联 | 收单交易、退款、查询、回调、海关申报 | ✅ | 已支持 |
| 银联 | 消费撤销、预授权申请/撤销/完成/完成撤销 | ✅ | 本版本新增 |
| 银联 | 代收、代付（付款到银行卡） | ✅ | 本版本新增 |
| 银联 | OpenAPI 独立模块（OAuth2/非对称认证骨架） | ✅ | 已支持 |
| 银联 | OpenAPI 具体产品 API（按产品逐项封装） | 🚧 | 规划中，建议按业务申请逐步接入 |
| 全渠道 | 健康检查（IHealthCheck）、分布式追踪（OpenTelemetry）、沙箱环境 | ✅ | 本版本新增 |

---

## 项目结构

```
GaoXinLibrary.PaySDK/
├── Core/                           # 统一基础类型
│   ├── IPayService.cs              # 统一支付接口
│   ├── PayChannel.cs               # 渠道枚举（17 种子渠道，含 Apple Pay）
│   ├── PayChannelExtensions.cs     # 渠道枚举扩展方法
│   ├── PayException.cs             # 基础异常
│   ├── PayRetryOptions.cs          # 瞬态故障重试配置（指数退避）
│   ├── PayJsonSerializer.cs        # 统一 JSON 序列化工具
│   ├── PayActivitySource.cs        # OpenTelemetry ActivitySource
│   ├── PayHealthCheck.cs           # ASP.NET Core 健康检查实现
│   ├── PayConstants.cs             # 全局常量（沙箱网关地址等）
│   ├── CreateOrderRequest.cs       # 创建订单请求
│   ├── CreateOrderResponse.cs      # 创建订单响应
│   ├── QueryOrderRequest.cs        # 查询订单请求
│   ├── QueryOrderResponse.cs       # 查询订单响应
│   ├── RefundRequest.cs            # 退款请求
│   ├── RefundResponse.cs           # 退款响应
│   ├── QueryRefundRequest.cs       # 退款查询请求
│   ├── QueryRefundResponse.cs      # 退款查询响应
│   ├── CloseOrderRequest.cs        # 关闭订单请求
│   ├── CloseOrderResponse.cs       # 关闭订单响应（含 IsSimulated 标记）
│   ├── DownloadBillRequest.cs      # 账单下载请求
│   ├── PayCallbackResult.cs        # 回调解析结果
│   └── WechatJsPayParams.cs        # 微信 JS 调起支付参数
├── Wechat/                          # 微信支付 v3
│   ├── Core/                        # WechatPayOptions / WechatPaySigner / WechatPayHttpClient
│   ├── Models/                      # 所有微信支付请求/响应模型
│   │   ├── WechatAbnormalRefundRequest.cs    # 异常退款请求
│   │   ├── WechatAbnormalRefundResponse.cs   # 异常退款响应
│   │   ├── WechatCombineOrderRequest.cs      # 合单下单请求（含子订单结构）
│   │   ├── WechatCombineOrderResponse.cs     # 合单下单响应
│   │   ├── WechatCombineQueryResponse.cs     # 合单查询响应
│   │   ├── WechatCombineCloseRequest.cs      # 合单关闭请求
│   │   ├── WechatProfitSharingRequest.cs     # 请求分账
│   │   ├── WechatProfitSharingQueryResponse.cs    # 分账查询响应
│   │   ├── WechatProfitSharingReturnRequest.cs    # 分账回退请求
│   │   ├── WechatProfitSharingReturnQueryResponse.cs # 分账回退查询响应
│   │   ├── WechatTransferBillsRequest.cs     # 商家转账到零钱请求
│   │   ├── WechatTransferBillsResponse.cs    # 商家转账到零钱响应
│   │   ├── WechatTransferBillQueryResponse.cs # 转账查询响应
│   │   └── ...                               # 其他支付/回调模型
│   ├── Services/                    # IWechatPayService / WechatPayService
│   └── WechatPayClient.cs          # 非 DI 场景的客户端入口
├── Alipay/                          # 支付宝
│   ├── Core/                        # AlipayOptions / AlipaySigner / AlipayHttpClient
│   ├── Models/                      # 所有支付宝请求/响应模型
│   │   ├── AlipayTradeRoyaltyRelationBindContent.cs      # 分账关系绑定
│   │   ├── AlipayTradeOrderSettleContent.cs              # 订单分账
│   │   ├── AlipayTradeOrderSettleQueryContent.cs         # 分账查询请求
│   │   ├── AlipayTradeOrderSettleQueryResponse.cs        # 分账查询响应
│   │   ├── AlipayTradeRoyaltyRelationBatchQueryContent.cs # 分账关系批量查询请求
│   │   ├── AlipayTradeRoyaltyRelationBatchQueryResponse.cs # 分账关系批量查询响应
│   │   ├── AlipayFundTransUniTransferContent.cs          # 商家转账
│   │   ├── AlipayFundTransCommonQueryContent.cs          # 转账查询请求
│   │   ├── AlipayFundTransCommonQueryResponse.cs         # 转账查询响应
│   │   ├── AlipayTradeComplainQueryContent.cs            # 交易投诉查询请求
│   │   ├── AlipayTradeComplainQueryResponse.cs           # 交易投诉查询响应
│   │   ├── AlipayTradeComplainFeedbackContent.cs         # 交易投诉反馈请求
│   │   └── ...                                           # 其他支付/回调模型
│   ├── Services/                    # IAlipayService / AlipayService
│   └── AlipayClient.cs             # 非 DI 场景的客户端入口
├── UnionPay/                        # 银联
│   ├── Core/                        # UnionPayOptions / UnionPaySigner / UnionPayHttpClient
│   ├── Models/                      # 所有银联请求/响应模型
│   │   ├── UnionPayConsumeUndoRequest.cs            # 消费撤销请求
│   │   ├── UnionPayConsumeUndoResponse.cs           # 消费撤销响应
│   │   ├── UnionPayPreAuthRequest.cs                # 预授权请求
│   │   ├── UnionPayPreAuthResponse.cs               # 预授权响应
│   │   ├── UnionPayPreAuthUndoRequest.cs            # 预授权撤销请求
│   │   ├── UnionPayPreAuthUndoResponse.cs           # 预授权撤销响应
│   │   ├── UnionPayPreAuthCompleteRequest.cs        # 预授权完成请求
│   │   ├── UnionPayPreAuthCompleteResponse.cs       # 预授权完成响应
│   │   ├── UnionPayPreAuthCompleteUndoRequest.cs    # 预授权完成撤销请求
│   │   ├── UnionPayPreAuthCompleteUndoResponse.cs   # 预授权完成撤销响应
│   │   ├── UnionPayCollectionRequest.cs             # 代收请求
│   │   ├── UnionPayCollectionResponse.cs            # 代收响应
│   │   ├── UnionPayPaymentRequest.cs                # 代付请求
│   │   ├── UnionPayPaymentResponse.cs               # 代付响应
│   │   ├── UnionPayCustomsDeclarationRequest.cs     # 海关申报请求
│   │   ├── UnionPayCustomsQueryRequest.cs           # 海关申报查询请求
│   │   ├── UnionPayEncryptKeyQueryRequest.cs        # 加密公钥更新查询请求
│   │   ├── UnionPayEncryptKeyQueryResponse.cs       # 加密公钥更新查询响应
│   │   ├── UnionPayRealNameAuthRequest.cs           # 实名认证请求
│   │   ├── UnionPayRealNameAuthResponse.cs          # 实名认证响应
│   │   ├── UnionPayFileTransferRequest.cs           # 文件传输请求
│   │   ├── UnionPayFileTransferResponse.cs          # 文件传输响应
│   │   └── ...                                      # 其他支付/回调模型
│   ├── Services/                    # IUnionPayService / UnionPayService
│   │   ├── IUnionPayCustomsService.cs  # 海关申报接口（非支付）
│   │   └── UnionPayCustomsService.cs   # 海关申报实现
│   ├── OpenApi/                     # 银联 OpenAPI（OAuth2/非对称）独立模块
│   │   ├── UnionPayOpenApiOptions.cs
│   │   ├── IUnionPayOpenApiService.cs
│   │   └── UnionPayOpenApiService.cs
│   └── UnionPayClient.cs           # 非 DI 场景的客户端入口（Pay + Customs）
├── Extensions/                      # DI 注入扩展方法
│   ├── PayServiceCollectionExtensions.cs      # AddPaySDK / AddPayService
│   ├── WechatPayServiceCollectionExtensions.cs # AddWechatPay
│   ├── AlipayServiceCollectionExtensions.cs    # AddAlipay
│   ├── UnionPayServiceCollectionExtensions.cs  # AddUnionPay
│   ├── PayHealthCheckExtensions.cs            # AddPayHealthChecks
│   └── UnionPayOpenApiServiceCollectionExtensions.cs  # AddUnionPayOpenApi
├── PayService.cs                    # IPayService 统一路由实现
├── PayService.Wechat.cs             # 微信支付统一路由实现
├── PayService.Alipay.cs             # 支付宝统一路由实现
└── PayService.UnionPay.cs           # 银联统一路由实现
```

---

## 单元测试

项目包含完整的单元测试套件，位于 `GaoXinLibrary.PaySDK.Tests/` 测试项目中，共计 **159 个测试用例**，全部通过。

### 运行测试

```bash
# 运行全部测试
dotnet test GaoXinLibrary.PaySDK.Tests/

# 运行指定测试分类
dotnet test GaoXinLibrary.PaySDK.Tests/ --filter "Category=SignerTests"
```

### 测试分类

| 测试文件 | 覆盖范围 |
|---------|---------|
| `SignerTests.cs` | 微信支付 v3 签名、支付宝 RSA2 签名、银联签名算法 |
| `AmountConversionTests.cs` | 金额单位转换（元 → 分 / 分 → 元）边界值 |
| `PayChannelExtensionsTests.cs` | PayChannel 枚举扩展方法（渠道名称、描述等） |
| `PayRetryOptionsTests.cs` | 重试配置默认值、自定义参数、禁用场景 |
| `PayJsonSerializerTests.cs` | snake_case 序列化/反序列化、Unicode 转义控制 |
| `PayServiceTests.cs` | 统一接口路由（IPayService）集成测试 |
| `EdgeCaseTests.cs` | 边界场景（空值处理、特殊字符、超长字符串等） |

---

## 许可证

MIT
