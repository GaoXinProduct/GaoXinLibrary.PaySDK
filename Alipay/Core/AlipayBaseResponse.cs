using System.Text.Json.Serialization;

namespace GaoXinLibrary.PaySDK.Alipay.Core;

/// <summary>
/// 支付宝 API 响应基类
/// </summary>
public class AlipayBaseResponse
{
    /// <summary>网关返回码：10000 表示成功</summary>
    [JsonPropertyName("code")]
    public string Code { get; set; } = string.Empty;

    /// <summary>网关返回码描述</summary>
    [JsonPropertyName("msg")]
    public string? Msg { get; set; }

    /// <summary>业务返回码，参见具体的 API 接口文档</summary>
    [JsonPropertyName("sub_code")]
    public string? SubCode { get; set; }

    /// <summary>业务返回码描述</summary>
    [JsonPropertyName("sub_msg")]
    public string? SubMsg { get; set; }
}
