namespace GaoXinLibrary.PaySDK.UnionPay.Models;

/// <summary>
/// 银联加密公钥更新查询响应参数
/// <para>
/// 银联返回最新的加密公钥证书，商户需替换本地证书。
/// </para>
/// </summary>
public class UnionPayEncryptKeyQueryResponse
{
    /// <summary>应答码：00 成功，其他失败</summary>
    public string RespCode { get; set; } = string.Empty;

    /// <summary>应答信息</summary>
    public string? RespMsg { get; set; }

    /// <summary>
    /// 加密公钥证书（signPubKeyCert）
    /// <para>银联返回的最新加密公钥证书内容</para>
    /// </summary>
    public string? SignPubKeyCert { get; set; }

    /// <summary>证书类型</summary>
    public string? CertType { get; set; }

    /// <summary>原始键值对</summary>
    public Dictionary<string, string> RawParams { get; set; } = new();
}
