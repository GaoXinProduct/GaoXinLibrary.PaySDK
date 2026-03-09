namespace GaoXinLibrary.PaySDK.Wechat.Models;

/// <summary>
/// 微信支付 H5 下单请求
/// <para>https://pay.weixin.qq.com/doc/v3/merchant/4012062524</para>
/// <para>
/// <see cref="WechatCreateOrderRequestBase.SceneInfo"/> 和 <see cref="WechatPaySceneInfo.H5Info"/>
/// 已预初始化默认值（H5Info.Type = "Wap"），只需设置 <see cref="WechatPaySceneInfo.PayerClientIp"/> 即可。
/// </para>
/// </summary>
public class WechatH5OrderRequest : WechatCreateOrderRequestBase
{
    /// <summary>
    /// 初始化 H5 下单请求，自动创建默认场景信息
    /// </summary>
    public WechatH5OrderRequest()
    {
        SceneInfo = new WechatPaySceneInfo
        {
            H5Info = new WechatPayH5Info { Type = "Wap" }
        };
    }

    /// <summary>
    /// 校验 H5 下单必填的场景信息
    /// </summary>
    internal void ValidateH5Fields()
    {
        if (SceneInfo is null)
            throw new ArgumentException("H5 下单必须设置 SceneInfo（场景信息）", nameof(SceneInfo));

        if (string.IsNullOrEmpty(SceneInfo.PayerClientIp))
            throw new ArgumentException("H5 下单必须设置 SceneInfo.PayerClientIp（用户终端 IP）", nameof(SceneInfo.PayerClientIp));

        SceneInfo.H5Info ??= new WechatPayH5Info { Type = "Wap" };
    }
}
