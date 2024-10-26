using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 范围击飞Buff
/// </summary>
public class KnockUpBuff_Group : Buff
{
    public KnockUpBuff_Group(MainLogicUnit source, MainLogicUnit owner, Skill skill, int buffID, object[] args = null)
        : base(source, owner, skill, buffID, args)
    {
    }

    protected override void Start()
    {
        base.Start();

        targetLst = new System.Collections.Generic.List<MainLogicUnit>();
        targetLst = CalcRule.FindMulipleTargetByRule(owner, cfg.impacter, skill.skillArgs);
        for (int i = 0; i < targetLst.Count; i++)
        {
            targetLst[i].KnockupCount += 1;
        }
    }

    protected override void End()
    {
        base.End();

        for (int i = 0; i < targetLst.Count; i++)
        {
            targetLst[i].KnockupCount -= 1;
        }
    }
}
