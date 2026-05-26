using GaoXinLibrary.PaySDK.UnionPay.Models;

namespace GaoXinLibrary.PaySDK.UnionPay.Services;

public interface IUnionPayService
{
    UnionPayFrontPayResponse CreateFrontPay(UnionPayFrontPayRequest request);

    UnionPayFrontPayResponse CreateWapPay(UnionPayWapPayRequest request);

    Task<UnionPayBackPayResponse> CreateBackPayAsync(UnionPayBackPayRequest request, CancellationToken ct = default);

    Task<UnionPayQrCodeApplyResponse> ApplyQrCodeAsync(UnionPayQrCodeApplyRequest request, CancellationToken ct = default);

    Task<UnionPayBackPayResponse> QrCodeConsumeAsync(UnionPayQrCodeConsumeRequest request, CancellationToken ct = default);

    Task<UnionPayQueryResponse> QueryOrderAsync(UnionPayQueryRequest request, CancellationToken ct = default);

    Task<UnionPayRefundResponse> RefundAsync(UnionPayRefundRequest request, CancellationToken ct = default);

    Task<byte[]> DownloadBillAsync(string settleDate, string fileType, CancellationToken ct = default);

    UnionPayCallbackParams ParseCallback(IDictionary<string, string> formParams);

    Task<UnionPayConsumeUndoResponse> ConsumeUndoAsync(UnionPayConsumeUndoRequest request, CancellationToken ct = default);

    Task<UnionPayPreAuthResponse> PreAuthAsync(UnionPayPreAuthRequest request, CancellationToken ct = default);

    Task<UnionPayPreAuthUndoResponse> PreAuthUndoAsync(UnionPayPreAuthUndoRequest request, CancellationToken ct = default);

    Task<UnionPayPreAuthCompleteResponse> PreAuthCompleteAsync(UnionPayPreAuthCompleteRequest request, CancellationToken ct = default);

    Task<UnionPayPreAuthCompleteUndoResponse> PreAuthCompleteUndoAsync(UnionPayPreAuthCompleteUndoRequest request, CancellationToken ct = default);

    Task<UnionPayCollectionResponse> CollectionAsync(UnionPayCollectionRequest request, CancellationToken ct = default);

    Task<UnionPayPaymentResponse> PayToBankCardAsync(UnionPayPaymentRequest request, CancellationToken ct = default);
}
