using PEMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ExecuteDamageBuffCfg : BuffCfg
{
    public int damagePct;
}

/// <summary>
/// 百分比生命值斩杀
/// </summary>
public class ExecuteDamageBuff : Buff
{
    PEInt damagePct;

    public ExecuteDamageBuff(MainLogicUnit source, MainLogicUnit owner, Skill skill, int buffID, object[] args = null)
        : base(source, owner, skill, buffID, args)
    {
    }

    public override void LogicInit()
    {
        base.LogicInit();

        ExecuteDamageBuffCfg edbc = cfg as ExecuteDamageBuffCfg;
        damagePct = edbc.damagePct;
    }

    protected override void Start()
    {
        base.Start();

        PEInt damage = (damagePct / 100) * owner.ud.unitCfg.hp;
        owner.GetDamageByBuff(damage, this);
    }
}
