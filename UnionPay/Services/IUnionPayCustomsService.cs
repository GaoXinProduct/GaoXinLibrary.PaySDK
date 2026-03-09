using GaoXinLibrary.PaySDK.UnionPay.Models;

namespace GaoXinLibrary.PaySDK.UnionPay.Services;

/// <summary>
/// 银联跨境电商海关申报服务接口
/// <para>
/// 非支付接口，用于将银联支付订单的支付信息向海关申报，
/// 实现海关对跨境业务支付流、订单流、物流的三单比对核查。
/// </para>
/// </summary>
public interface IUnionPayCustomsService
{
    /// <summary>
    /// 提交海关申报
    /// <para>同步应答仅为受理结果，申报结果需通过 <see cref="QueryDeclarationAsync"/> 查询</para>
    /// </summary>
    Task<UnionPayCustomsDeclarationResponse> DeclareAsync(UnionPayCustomsDeclarationRequest request, CancellationToken ct = default);

    /// <summary>
    /// 查询海关申报结果
    /// </summary>
    Task<UnionPayCustomsQueryResponse> QueryDeclarationAsync(UnionPayCustomsQueryRequest request, CancellationToken ct = default);

    /// <summary>
    /// 加密公钥更新查询
    /// <para>
    /// 商户定期（1天1次）向银联全渠道系统发起获取加密公钥信息交易。
    /// 在加密公钥证书更新期间，全渠道系统支持新老证书的共同使用，新老证书并行期为1个月。
    /// </para>
    /// </summary>
    Task<UnionPayEncryptKeyQueryResponse> QueryEncryptKeyAsync(UnionPayEncryptKeyQueryRequest request, CancellationToken ct = default);

    /// <summary>
    /// 实名认证
    /// <para>
    /// 验证银行卡验证信息及身份信息如证件类型、证件号码、姓名、密码、CVN2、有效期、手机号等与银行卡号的一致性。
    /// </para>
    /// </summary>
    Task<UnionPayRealNameAuthResponse> RealNameAuthAsync(UnionPayRealNameAuthRequest request, CancellationToken ct = default);

    /// <summary>
    /// 文件传输（对账文件下载）
    /// <para>
    /// 商户可开发"文件传输类交易"接口获取对账文件。
    /// </para>
    /// </summary>
    Task<UnionPayFileTransferResponse> FileTransferAsync(UnionPayFileTransferRequest request, CancellationToken ct = default);
}
