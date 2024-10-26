using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Buff配置
/// </summary>
public class BuffCfg
{
    public int buffID;
    public string buffName;
    /// <summary>
    /// Buff类型
    /// </summary>
    public BuffTypeEnum buffType;
    /// <summary>
    /// Buff附着目标
    /// </summary>
    public AttachTypeEnum attacher;
    /// <summary>
    /// buff作用目标,为null默认影响附着对象
    /// </summary>
    public TargetCfg impacter;
    /// <summary>
    /// 生效延时
    /// </summary>
    public int buffDelay;
    /// <summary>
    /// 生效间隔
    /// </summary>
    public int buffInterval;
    /// <summary>
    /// (不包含delay) 0：生效1次，-1：永久生效
    /// </summary>
    public int buffDuration;

    public StaticPosTypeEnum staticPosType;

    /// <summary>
    /// buff音效
    /// </summary>
    public string buffAudio;
    /// <summary>
    /// buff持续时间的音效
    /// </summary>
    public string buffEffect;
    /// <summary>
    /// buff命中音效
    /// </summary>
    public string hitTickAudio;

}
/// <summary>
/// Buff附着类型
/// </summary>
public enum AttachTypeEnum
{
    None,
    /// <summary>
    /// 附着自己
    /// 比如亚瑟的1技能加速buff
    /// </summary>
    Caster,
    /// <summary>
    /// 给予敌方沉默
    /// 比如亚瑟的1技能沉默buff
    /// </summary>
    Target,


    /// <summary>
    /// 独立
    /// 比如亚瑟大招产生的持续范围伤害
    /// </summary>
    Indie,
    /// <summary>
    /// 弹道技能
    /// 比如后羿大招命中目标产生的范围伤害
    /// </summary>
    Bullet,
}
/// <summary>
/// 位置类型
/// </summary>
public enum StaticPosTypeEnum
{
    None,
    /// <summary>
    /// buff所属技能施放者的位置
    /// </summary>
    SkillCasterPos,
    /// <summary>
    /// Buff所属技能锁定目标的位置
    /// </summary>
    SkillLockTargetPos,
    /// <summary>
    /// 子弹命中目标的位置 
    /// </summary>
    BulletHitTargetPos,
    /// <summary>
    /// UI输入位置信息
    /// </summary>
    UIInputPos,
}
/// <summary>
/// buff类型
/// </summary>
public enum BuffTypeEnum
{
    None,
    /// <summary>
    /// 修改技能
    /// </summary>
    ModifySkill,
    /// <summary>
    /// 修改移速，修改单体移速
    /// </summary>
    MoveSpeed_Single,
    /// <summary>
    /// 沉默
    /// </summary>
    Silense,
    /// <summary>
    /// 基于目标闪现跳跃
    /// </summary>
    TargetFlashMove,
    DirectionFlashMove,//TODO
    /// <summary>
    /// 百分比斩杀
    /// </summary>
    ExecuteDamage,
    /// <summary>
    /// 群体击飞
    /// </summary>
    Knockup_Group,
    /// <summary>
    /// 亚瑟一技能标记buff
    /// </summary>
    ArthurMark,
    /// <summary>
    /// 治疗
    /// </summary>
    HPCure,


    //houyi专区buff
    /// <summary>
    /// Houyi主动技能修改buff
    /// </summary>
    HouyiActiveSkillModify,
    Scatter,
    /// <summary>
    /// Houyi被动攻速加成buff
    /// </summary>
    HouyiPasvAttackSpeed,
    /// <summary>
    /// Houyi被动技能修改Buff
    /// </summary>
    HouyiPasvSkillModify,
    /// <summary>
    /// Houyi被动技能多重射击Buff
    /// </summary>
    HouyiPasvMultiArrow,
    /// <summary>
    /// 混合多重射击与散射
    /// </summary>
    HouyiMixedMultiScatter,


    Stun_Single_DynamicTime,

    /// <summary>
    /// 动态修改群体移速
    /// </summary>
    MoveSpeed_DynamicGroup,
    /// <summary>
    /// 静态群体移速Buff
    /// </summary>
    MoveSpeed_StaticGroup,
    /// <summary>
    /// 动态群体伤害
    /// </summary>
    Damage_DynamicGroup,
    /// <summary>
    /// 静态群体伤害(固定位置范围伤害)
    /// </summary>
    Damage_StaticGroup,
    /// <summary>
    /// 移动攻击
    /// </summary>
    MoveAttack,
}
