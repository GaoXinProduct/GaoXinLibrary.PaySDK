namespace GaoXinLibrary.PaySDK.UnionPay.Models;

/// <summary>
/// 银联文件传输响应参数
/// </summary>
public class UnionPayFileTransferResponse
{
    /// <summary>应答码：00 成功，其他失败</summary>
    public string RespCode { get; set; } = string.Empty;

    /// <summary>应答信息</summary>
    public string? RespMsg { get; set; }

    /// <summary>
    /// 对账文件内容（已解压的文本内容）
    /// <para>如果银联返回 fileContent 字段，此处为解码后的字符串</para>
    /// </summary>
    public string? FileContent { get; set; }

    /// <summary>
    /// 对账文件原始字节（未解码的原始数据）
    /// </summary>
    public byte[]? FileData { get; set; }

    /// <summary>原始键值对</summary>
    public Dictionary<string, string> RawParams { get; set; } = new();
}
