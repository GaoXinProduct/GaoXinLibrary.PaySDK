# Copilot Instructions

## Project Guidelines
- 在 .NET 10 项目中避免使用 Buffer.BlockCopy，优先使用 Span/AsSpan + CopyTo 或其他现代0 GC拷贝方式。
- 编写的model必须每一个class对应一个文件，不要在一个文件中写多个class
- 每个class的字段必须按照官方文档所述进行编写
- 需要支持微信，支付宝，银联三种支付方式（暂时，后续可能会增加Stripe/paypal等），提供单个或者统一对外的接口进行支付/查询/退款等
- 兼容.net 8/9/10，并且提供di注入
- 银联的开发文档为：https://open.unionpay.com/tjweb/support/doc/online/3/125
- 支付宝的开发文档为：https://opendocs.alipay.com/open/00a0ut?pathHash=b19b288a
- 微信的开发文档为：https://pay.weixin.qq.com/doc/v3/merchant/4012062524
- 支付宝需要实现的支付产品有当面付，订单码支付，App 支付，手机网站支付，电脑网站支付【下载账单，订单退款】，支付回调需要统一
- 微信需要实现的支付产品有JSAPI支付，APP支付，H5支付，Native支付，小程序支付【下载账单，订单退款】，支付回调需要统一
- 微信支付的回调文档：https://pay.weixin.qq.com/doc/v3/merchant/4012791902
- 支付宝支付的回调文档：https://opendocs.alipay.com/open/270/105902?pathHash=d5cd617e