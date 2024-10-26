using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/// <summary>
/// 技能配置
/// </summary>
public class SkillCfg
{
    /// <summary>
    /// 技能ID 
    /// </summary>
    public int skillID;
    /// <summary>
    /// 技能图标名字
    /// </summary>
    public string iconName;
    /// <summary>
    /// 施法动画名称
    /// </summary>
    public string aniName;
    /// <summary>
    /// 技能施放方式
    /// </summary>
    public ReleaseModeEnum releaseMode;
    /// <summary>
    /// 目标选择配置
    /// </summary>
    public TargetCfg targetCfg;
    /// <summary>
    /// 弹道配置
    /// </summary>
    public BulletCfg bulletCfg;
    /// <summary>
    /// CD时间:毫秒
    /// </summary>
    public int cdTime;
    /// <summary>
    /// 施法时间(技能前摇)：毫秒
    /// </summary>
    public int spellTime;

    /// <summary>
    /// 是否为普通攻击
    /// </summary>
    public bool isNormalAttack;
    /// <summary>
    /// 技能全长时间，包含前摇，后摇
    /// 后摇动作均可被移动中断，但技能总时间不能变短
    /// </summary>
    public int skillTime;
    /// <summary>
    /// 基础伤害数值
    /// </summary>
    public int damage;
    /// <summary>
    /// 附加buff
    /// </summary>
    public int[] buffIDArr;

    /// <summary>
    /// 施法开始
    /// </summary>
    public string audio_start;
    /// <summary>
    /// 施法成功
    /// </summary>
    public string audio_work;
    /// <summary>
    /// 施法命中
    /// </summary>
    public string audio_hit;
}
public enum ReleaseModeEnum
{
    None,
    /// <summary>
    /// 点击施放
    /// </summary>
    Click,
    /// <summary>
    /// 基于某一指定位置施放
    /// </summary>
    Postion,
    /// <summary>
    /// 指定方向施放
    /// </summary>
    Direction,


}

