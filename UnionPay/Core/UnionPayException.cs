using GaoXinLibrary.PaySDK.Core;

namespace GaoXinLibrary.PaySDK.UnionPay.Core;

/// <summary>
/// 银联支付 API 调用异常
/// </summary>
public class UnionPayException : PayException
{
    /// <summary>
    /// 使用错误码和错误信息创建异常
    /// </summary>
    public UnionPayException(string errorCode, string errorMessage)
        : base(errorCode, errorMessage, PayChannel.UnionPayGateway)
    {
    }

    /// <summary>
    /// 使用消息字符串创建异常
    /// </summary>
    public UnionPayException(string message, Exception? inner = null)
        : base(message, inner)
    {
    }
}
