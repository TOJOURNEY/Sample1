using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEMath;



public class DamageBuffCfg_DynamicGroup : BuffCfg
{
    /// <summary>
    /// 每次Tick伤害
    /// </summary>
    public int damage;
}
/// <summary>
/// 范围伤害buff
/// </summary>
public class DamageBuff_DynamicGroup : Buff
{
    PEInt damage;

    public DamageBuff_DynamicGroup(MainLogicUnit source, MainLogicUnit owner, Skill skill, int buffID, object[] args = null)
        : base(source, owner, skill, buffID, args)
    {
    }

    public override void LogicInit()
    {
        base.LogicInit();

        targetLst = new List<MainLogicUnit>();
        DamageBuffCfg_DynamicGroup gdbc = cfg as DamageBuffCfg_DynamicGroup;
        damage = gdbc.damage;
    }

    protected override void Tick()
    {
        base.Tick();
        CalcGroupDamage();
    }

    void CalcGroupDamage()
    {
        targetLst.Clear();
        targetLst.AddRange(CalcRule.FindMulipleTargetByRule(owner, cfg.impacter, PEVector3.zero));
        for (int i = 0; i < targetLst.Count; i++)
        {
            targetLst[i].GetDamageByBuff(damage, this);
        }
    }
}
