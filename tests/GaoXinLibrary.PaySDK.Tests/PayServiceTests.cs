using GaoXinLibrary.PaySDK.Core;
using Xunit;

namespace GaoXinLibrary.PaySDK.Tests;

public class PayServiceTests
{
    [Fact]
    public void Constructor_AllServicesNull_Succeeds()
    {
        var service = new PayService();
        Assert.NotNull(service);
    }

    [Fact]
    public void Constructor_WithLogger_Succeeds()
    {
        var service = new PayService(logger: null);
        Assert.NotNull(service);
    }

    [Fact]
    public async Task CreateOrder_UnsupportedChannel_ThrowsPayException()
    {
        var service = new PayService();
        var request = new CreateOrderRequest
        {
            Channel = (PayChannel)99,
            OutTradeNo = "order_001",
            Subject = "test",
            TotalFee = 100
        };
        var ex = await Assert.ThrowsAsync<PayException>(() => service.CreateOrderAsync(request));
        Assert.Equal("UNSUPPORTED_CHANNEL", ex.ErrorCode);
    }

    [Fact]
    public async Task CreateOrder_WechatChannel_NoServiceConfigured_ThrowsPayException()
    {
        var service = new PayService();
        var request = new CreateOrderRequest
        {
            Channel = PayChannel.WechatNative,
            OutTradeNo = "order_001",
            Subject = "test",
            TotalFee = 100
        };
        var ex = await Assert.ThrowsAsync<PayException>(() => service.CreateOrderAsync(request));
        Assert.Equal("SERVICE_NOT_CONFIGURED", ex.ErrorCode);
        Assert.Contains("微信支付", ex.ErrorMessage);
    }

    [Fact]
    public async Task CreateOrder_AlipayChannel_NoServiceConfigured_ThrowsPayException()
    {
        var service = new PayService();
        var request = new CreateOrderRequest
        {
            Channel = PayChannel.AlipayPage,
            OutTradeNo = "order_001",
            Subject = "test",
            TotalFee = 100
        };
        var ex = await Assert.ThrowsAsync<PayException>(() => service.CreateOrderAsync(request));
        Assert.Equal("SERVICE_NOT_CONFIGURED", ex.ErrorCode);
        Assert.Contains("支付宝", ex.ErrorMessage);
    }

    [Fact]
    public async Task CreateOrder_UnionPayChannel_NoServiceConfigured_ThrowsPayException()
    {
        var service = new PayService();
        var request = new CreateOrderRequest
        {
            Channel = PayChannel.UnionPayGateway,
            OutTradeNo = "order_001",
            Subject = "test",
            TotalFee = 100
        };
        var ex = await Assert.ThrowsAsync<PayException>(() => service.CreateOrderAsync(request));
        Assert.Equal("SERVICE_NOT_CONFIGURED", ex.ErrorCode);
        Assert.Contains("银联", ex.ErrorMessage);
    }

    [Fact]
    public async Task QueryOrder_UnsupportedChannel_ThrowsPayException()
    {
        var service = new PayService();
        var request = new QueryOrderRequest
        {
            Channel = (PayChannel)99,
            OutTradeNo = "order_001"
        };
        var ex = await Assert.ThrowsAsync<PayException>(() => service.QueryOrderAsync(request));
        Assert.Equal("UNSUPPORTED_CHANNEL", ex.ErrorCode);
    }

    [Fact]
    public async Task QueryOrder_WechatChannel_NoService_ThrowsServiceNotConfigured()
    {
        var service = new PayService();
        var request = new QueryOrderRequest
        {
            Channel = PayChannel.WechatJsapi,
            OutTradeNo = "order_001"
        };
        var ex = await Assert.ThrowsAsync<PayException>(() => service.QueryOrderAsync(request));
        Assert.Equal("SERVICE_NOT_CONFIGURED", ex.ErrorCode);
    }

    [Fact]
    public async Task Refund_UnsupportedChannel_ThrowsPayException()
    {
        var service = new PayService();
        var request = new RefundRequest
        {
            Channel = (PayChannel)99,
            OutTradeNo = "order_001",
            OutRefundNo = "refund_001",
            RefundFee = 50,
            TotalFee = 100
        };
        var ex = await Assert.ThrowsAsync<PayException>(() => service.RefundAsync(request));
        Assert.Equal("UNSUPPORTED_CHANNEL", ex.ErrorCode);
    }

    [Fact]
    public async Task CloseOrder_WechatChannel_NoService_ThrowsServiceNotConfigured()
    {
        var service = new PayService();
        var request = new CloseOrderRequest
        {
            Channel = PayChannel.WechatNative,
            OutTradeNo = "order_001"
        };
        var ex = await Assert.ThrowsAsync<PayException>(() => service.CloseOrderAsync(request));
        Assert.Equal("SERVICE_NOT_CONFIGURED", ex.ErrorCode);
    }

    [Fact]
    public async Task CloseOrder_UnionPayChannel_NoService_ReturnsSimulatedSuccess()
    {
        var service = new PayService();
        var request = new CloseOrderRequest
        {
            Channel = PayChannel.UnionPayGateway,
            OutTradeNo = "order_001"
        };
        var result = await service.CloseOrderAsync(request);
        Assert.True(result.Success);
        Assert.True(result.IsSimulated);
        Assert.Equal(CloseOrderOperationMode.Simulated, result.OperationMode);
        Assert.Equal(PayChannel.UnionPayGateway, result.Channel);
    }

    [Fact]
    public async Task CloseOrder_UnionPayWap_ReturnsSimulatedSuccess()
    {
        var service = new PayService();
        var request = new CloseOrderRequest
        {
            Channel = PayChannel.UnionPayWap,
            OutTradeNo = "order_001"
        };
        var result = await service.CloseOrderAsync(request);
        Assert.True(result.Success);
        Assert.True(result.IsSimulated);
    }

    [Fact]
    public async Task CloseOrder_UnionPayApplePay_ReturnsSimulatedSuccess()
    {
        var service = new PayService();
        var request = new CloseOrderRequest
        {
            Channel = PayChannel.UnionPayApplePay,
            OutTradeNo = "order_001"
        };
        var result = await service.CloseOrderAsync(request);
        Assert.True(result.Success);
        Assert.True(result.IsSimulated);
    }

    [Fact]
    public async Task DownloadBill_UnsupportedChannel_ThrowsPayException()
    {
        var service = new PayService();
        var request = new DownloadBillRequest
        {
            Channel = (PayChannel)99,
            BillDate = "20250101",
            BillType = "ALL"
        };
        var ex = await Assert.ThrowsAsync<PayException>(() => service.DownloadBillAsync(request));
        Assert.Equal("UNSUPPORTED_CHANNEL", ex.ErrorCode);
    }

    [Fact]
    public async Task ParseCallback_UnsupportedChannel_ThrowsPayException()
    {
        var service = new PayService();
        var ex = await Assert.ThrowsAsync<PayException>(() =>
            service.ParseCallbackAsync((PayChannel)99, "body", null));
        Assert.Equal("UNSUPPORTED_CHANNEL", ex.ErrorCode);
    }

    [Fact]
    public async Task ParseCallback_WechatChannel_NoService_ThrowsServiceNotConfigured()
    {
        var service = new PayService();
        var ex = await Assert.ThrowsAsync<PayException>(() =>
            service.ParseCallbackAsync(PayChannel.WechatJsapi, "body", new Dictionary<string, string>()));
        Assert.Equal("SERVICE_NOT_CONFIGURED", ex.ErrorCode);
    }

    [Fact]
    public async Task ParseCallback_AlipayChannel_NoService_ThrowsServiceNotConfigured()
    {
        var service = new PayService();
        var ex = await Assert.ThrowsAsync<PayException>(() =>
            service.ParseCallbackAsync(PayChannel.AlipayPage, "body", null));
        Assert.Equal("SERVICE_NOT_CONFIGURED", ex.ErrorCode);
    }

    [Fact]
    public async Task ParseCallback_UnionPayChannel_NoService_ThrowsServiceNotConfigured()
    {
        var service = new PayService();
        var ex = await Assert.ThrowsAsync<PayException>(() =>
            service.ParseCallbackAsync(PayChannel.UnionPayGateway, "body", null));
        Assert.Equal("SERVICE_NOT_CONFIGURED", ex.ErrorCode);
    }
}
