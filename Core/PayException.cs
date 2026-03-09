namespace GaoXinLibrary.PaySDK.Core;

/// <summary>
/// 支付 SDK 调用异常
/// </summary>
public class PayException : Exception
{
    /// <summary>错误码</summary>
    public string ErrorCode { get; }

    /// <summary>错误信息</summary>
    public string ErrorMessage { get; }

    /// <summary>支付渠道</summary>
    public PayChannel? Channel { get; }

    /// <summary>
    /// 使用错误码和错误信息创建异常
    /// </summary>
    public PayException(string errorCode, string errorMessage, PayChannel? channel = null)
        : base(channel.HasValue
            ? $"{channel} 支付失败：[{errorCode}] {errorMessage}"
            : $"操作失败：[{errorCode}] {errorMessage}")
    {
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
        Channel = channel;
    }

    /// <summary>
    /// 使用消息字符串创建异常
    /// </summary>
    public PayException(string message, Exception? inner = null)
        : base(message, inner)
    {
        ErrorCode = "UNKNOWN";
        ErrorMessage = message;
    }
}
