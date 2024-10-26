using PEMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class HouyiScatterArrowBuffCfg : BuffCfg
{
    public int scatterCount;//散射个数
    public TargetCfg targetCfg;//散射目标查找规则
    public int damagePct;//散射子弹伤害百分比
}
/// <summary>
/// 后羿1技能散射buff
/// </summary>
public class HouyiScatterArrowBuff : Buff
{
    int scatterCount;//散射个数
    TargetCfg targetCfg;//散射目标查找规则
    PEInt damagePct;//散射子弹伤害百分比
    MainLogicUnit lockTarget;

    public HouyiScatterArrowBuff(MainLogicUnit source, MainLogicUnit owner, Skill skill, int buffID, object[] args = null)
        : base(source, owner, skill, buffID, args)
    {
    }

    public override void LogicInit()
    {
        base.LogicInit();

        HouyiScatterArrowBuffCfg sbc = cfg as HouyiScatterArrowBuffCfg;
        scatterCount = sbc.scatterCount;
        targetCfg = sbc.targetCfg;
        damagePct = sbc.damagePct;
        targetLst = new List<MainLogicUnit>();
        lockTarget = skill.lockTarget;

        var findLst = CalcRule.FindMulipleTargetByRule(owner, targetCfg, PEVector3.zero);
        int count = 0;
        for (int i = 0; i < findLst.Count; i++)
        {
            if (count < scatterCount)
            {
                if (findLst[i].Equals(lockTarget))
                {
                    continue;
                }
                else
                {
                    targetLst.Add(findLst[i]);
                    count += 1;
                }
            }
        }

        for (int i = 0; i < targetLst.Count; i++)
        {
            TargetBullet bullet = source.CreateSkillBullet(source, targetLst[i], skill) as TargetBullet;
            bullet.HitTargetCB = (MainLogicUnit target, object[] args) => {
                this.Log("scatter target name:" + target.unitName);
                target.GetDamageByBuff(skill.cfg.damage * damagePct / 100, this);
            };
        }
    }
}