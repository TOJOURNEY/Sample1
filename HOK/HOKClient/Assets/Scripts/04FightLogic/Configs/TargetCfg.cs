using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 目标配置
/// </summary>
public class TargetCfg
{
    /// <summary>
    /// 技能可施放目标类型
    /// </summary>
    public TargetTeamEnum targetTeam;
    /// <summary>
    /// 技能目标选择规则
    /// </summary>
    public SelectRuleEnum selectRule;
    /// <summary>
    /// 目标类型数组
    /// 可以是多类目标
    /// </summary>
    public UnitTypeEnum[] targetTypeArr;

    //----------辅助参数-----------//
    /// <summary>
    /// 查找目标范围距离
    /// </summary>
    public float selectRange;
    /// <summary>
    /// 移动攻击搜索距离：单位：米
    /// </summary>
    public float searchDis;
}
/// <summary>
/// 技能目标选择规则
/// </summary>
public enum SelectRuleEnum
{
    None,
    /// <summary>
    /// 单个目标选择规则
    /// 总血量最少
    /// </summary>
    MinHPValue,
    /// <summary>
    /// 单个目标选择规则
    /// 百分比血量最少
    /// </summary>
    MinHPPercent,
    /// <summary>
    /// 单个目标选择规则
    /// 靠近目标角色最近的单个选择
    /// </summary>
    TargetClosestSingle,
    /// <summary>
    /// 单个目标选择规则
    /// 靠近某个位置的单个选择
    /// </summary>
    PositionClosestSingle,


    /// <summary>
    /// 多个目标选择规则
    /// 范围选择
    /// </summary>
    TargetClosetMultiple,

    /// <summary>
    /// 靠近某个位置的多个选择
    /// 范围选择
    /// </summary>
    PositionClosestMultiple,

    Hero,//所有英雄单位，全图技能
}
/// <summary>
/// 目标队伍
/// </summary>
public enum TargetTeamEnum
{
    /// <summary>
    /// 用于动态选择目标，通常是方向指向或位置指向技能，在施法成功后通过buff来选择目标
    /// </summary>
    Dynamic,
    /// <summary>
    /// 队友
    /// </summary>
    Friend,
    /// <summary>
    /// 敌人
    /// </summary>
    Enemy,
}

