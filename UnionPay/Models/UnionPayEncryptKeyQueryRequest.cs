namespace GaoXinLibrary.PaySDK.UnionPay.Models;

/// <summary>
/// 银联加密公钥更新查询请求参数
/// <para>
/// 商户定期（1天1次）向银联全渠道系统发起获取加密公钥信息交易。
/// 在加密公钥证书更新期间，全渠道系统支持新老证书的共同使用，新老证书并行期为1个月。
/// 全渠道系统向商户返回最新的加密公钥证书，由商户服务器替换本地证书。
/// </para>
/// </summary>
public class UnionPayEncryptKeyQueryRequest
{
    /// <summary>交易类型，固定 95（加密公钥更新查询）</summary>
    public string TxnType { get; set; } = "95";

    /// <summary>交易子类，固定 00</summary>
    public string TxnSubType { get; set; } = "00";

    /// <summary>产品类型，固定 000000</summary>
    public string BizType { get; set; } = "000000";

    /// <summary>商户订单号</summary>
    public string OrderId { get; set; } = string.Empty;

    /// <summary>交易时间，格式 yyyyMMddHHmmss</summary>
    public string TxnTime { get; set; } = string.Empty;

    /// <summary>证书类型，固定 01（敏感信息加密公钥）</summary>
    public string CertType { get; set; } = "01";
}
