using GaoXinLibrary.PaySDK.Core;

namespace GaoXinLibrary.PaySDK.Alipay.Core;

/// <summary>
/// 支付宝 API 调用异常
/// </summary>
public class AlipayException : PayException
{
    /// <summary>
    /// 使用错误码和错误信息创建异常
    /// </summary>
    public AlipayException(string errorCode, string errorMessage)
        : base(errorCode, errorMessage, null)
    {
    }

    /// <summary>
    /// 使用消息字符串创建异常
    /// </summary>
    public AlipayException(string message, Exception? inner = null)
        : base(message, inner)
    {
    }
}
