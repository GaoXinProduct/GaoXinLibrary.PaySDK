namespace GaoXinLibrary.PaySDK.Core;

/// <summary>
/// 瞬态故障重试配置
/// <para>
/// 适用于网络抖动、连接超时、服务端 5xx 错误等临时性故障的自动重试。<br/>
/// 重试使用指数退避策略，每次重试等待时间翻倍，但不超过 <see cref="MaxDelay"/>。
/// </para>
/// <para>
/// 可重试的故障类型：
/// <list type="bullet">
///   <item>网络层错误（连接失败、DNS 解析失败等 <see cref="HttpRequestException"/>）</item>
///   <item>HTTP 请求超时（<see cref="TaskCanceledException"/>，非用户主动取消）</item>
///   <item>服务端错误（HTTP 5xx 状态码）</item>
/// </list>
/// </para>
/// </summary>
public class PayRetryOptions
{
    /// <summary>
    /// 最大重试次数（不含首次请求），默认 2 次
    /// <para>设置为 0 表示不进行重试。</para>
    /// </summary>
    public int MaxRetries { get; set; } = 2;

    /// <summary>
    /// 首次重试前的等待时间，默认 500 毫秒
    /// <para>后续重试将按指数退避递增（每次翻倍）。</para>
    /// </summary>
    public TimeSpan InitialDelay { get; set; } = TimeSpan.FromMilliseconds(500);

    /// <summary>
    /// 单次重试等待时间上限，默认 5 秒
    /// <para>指数退避不会超过此值。</para>
    /// </summary>
    public TimeSpan MaxDelay { get; set; } = TimeSpan.FromSeconds(5);
}
