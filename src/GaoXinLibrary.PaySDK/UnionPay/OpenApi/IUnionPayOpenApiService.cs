namespace GaoXinLibrary.PaySDK.UnionPay.OpenApi;

/// <summary>
/// 银联 OpenAPI 通用访问接口（独立于收单交易接口）
/// </summary>
public interface IUnionPayOpenApiService
{
    /// <summary>
    /// 调用银联 OpenAPI
    /// </summary>
    /// <param name="bizMethod">OpenAPI 业务方法名</param>
    /// <param name="payload">业务请求体对象</param>
    /// <param name="ct">取消令牌</param>
    /// <returns>原始响应文本</returns>
    Task<string> PostAsync(string bizMethod, object payload, CancellationToken ct = default);
}
