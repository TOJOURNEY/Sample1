using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 子弹配置
/// </summary>
public class BulletCfg
{
    public BulletTypeEnum bulletType;
    public string bulletName;
    public string resPath;
    public float bulletSpeed;
    public float bulletSize;
    public float bulletHeight;
    public float bulletOffset;

    public int bulletDelay;//ms
    /// <summary>
    /// 是否被阻挡
    /// </summary>
    public bool canBlock;

    public TargetCfg impacter;
    public int bulletDuration;
}
/// <summary>
/// 子弹类型
/// </summary>
public enum BulletTypeEnum
{
    /// <summary>
    /// 基于ui方向
    /// </summary>
    UIDirection,
    /// <summary>
    /// 基于ui指定位置
    /// </summary>
    UIPosition,
    /// <summary>
    /// 当前技能目标
    /// </summary>
    SkillTarget,
    /// <summary>
    /// 放置技能
    /// </summary>
    BuffSearch,
}

