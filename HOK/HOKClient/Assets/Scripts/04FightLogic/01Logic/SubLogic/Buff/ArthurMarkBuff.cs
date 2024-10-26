using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEMath;

public class ArthurMarkBuffCfg : BuffCfg
{
    public int damagePct;
}
/// <summary>
/// 亚瑟一技能标记buff
/// </summary>
public class ArthurMarkBuff : Buff
{
    PEInt damagePct;
    MainLogicUnit target;
    public ArthurMarkBuff(MainLogicUnit source, MainLogicUnit owner, Skill skill, int buffID, object[] args = null) : base(source, owner, skill, buffID, args)
    {
    }
    public override void LogicInit()
    {
        base.LogicInit();

        ArthurMarkBuffCfg ambc = cfg as ArthurMarkBuffCfg;
        damagePct = ambc.damagePct;
        target = skill.lockTarget;

    }
    protected override void Start()
    {
        base.Start();
        target.OnHurt += GetHurt;
         
    }
    void GetHurt()
    {
        target.GetDamageByBuff(damagePct / 100 * target.ud.unitCfg.hp, this, false);
    }
    protected override void End()
    {
        base.End();
        target.OnHurt -= GetHurt;

    }
}
