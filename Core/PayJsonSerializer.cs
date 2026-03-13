using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GaoXinLibrary.PaySDK.Core;

/// <summary>
/// 支付 SDK 统一 JSON 序列化工具
/// <para>
/// 提供预配置的 <see cref="JsonSerializerOptions"/>，使用 snake_case 命名策略、
/// 忽略 null 字段、并启用 <see cref="JavaScriptEncoder.UnsafeRelaxedJsonEscaping"/> 以避免中文 Unicode 转义。
/// </para>
/// <para>
/// 用法示例：
/// <code>
/// // 序列化（中文不会被转义为 \uXXXX）
/// var json = PayJsonSerializer.Serialize(myObject);
///
/// // 反序列化
/// var obj = PayJsonSerializer.Deserialize&lt;MyModel&gt;(json);
///
/// // 直接获取 JsonSerializerOptions 用于自定义场景
/// var options = PayJsonSerializer.Options;
/// </code>
/// </para>
/// </summary>
public static class PayJsonSerializer
{
    /// <summary>
    /// 预配置的 JSON 序列化选项
    /// <para>
    /// • 命名策略：<see cref="JsonNamingPolicy.SnakeCaseLower"/>（自动 snake_case）<br/>
    /// • 空值处理：<see cref="JsonIgnoreCondition.WhenWritingNull"/>（忽略 null 字段）<br/>
    /// • 编码器：<see cref="JavaScriptEncoder.UnsafeRelaxedJsonEscaping"/>（中文直接输出，不做 Unicode 转义）
    /// </para>
    /// </summary>
    public static readonly JsonSerializerOptions Options = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    /// <summary>
    /// 使用 SDK 预配置选项序列化对象为 JSON 字符串
    /// </summary>
    /// <param name="value">要序列化的对象</param>
    /// <returns>JSON 字符串（中文不转义）</returns>
    public static string Serialize<T>(T value) =>
        JsonSerializer.Serialize(value, Options);

    /// <summary>
    /// 使用 SDK 预配置选项将 JSON 字符串反序列化为指定类型
    /// </summary>
    /// <typeparam name="T">目标类型</typeparam>
    /// <param name="json">JSON 字符串</param>
    /// <returns>反序列化后的对象</returns>
    public static T? Deserialize<T>(string json) =>
        JsonSerializer.Deserialize<T>(json, Options);
}
