using PEMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class HouyiMixedMultiScatterBuffCfg : BuffCfg
{
    public int scatterCount;//散射个数
    public TargetCfg targetCfg;//散射目标查找规则
    public int damagePct;//散射子弹伤害百分比

    public int arrowCount;
    public int arrowDelay;
    public float posOffset;
}
/// <summary>
/// 混合强化Buff
/// </summary>
public class HouyiMixedMultiScatterBuff : Buff
{
    int scatterCount;//散射个数
    TargetCfg targetCfg;//散射目标查找规则
    int damagePct;//散射子弹伤害百分比
    MainLogicUnit targetHero;

    int arrowCount;
    int arrowDelay;
    PEInt posOffset;

    public HouyiMixedMultiScatterBuff(MainLogicUnit source, MainLogicUnit owner, Skill skill, int buffID, object[] args = null)
        : base(source, owner, skill, buffID, args)
    {
    }

    public override void LogicInit()
    {
        base.LogicInit();

        HouyiMixedMultiScatterBuffCfg hmmsbc = cfg as HouyiMixedMultiScatterBuffCfg;
        scatterCount = hmmsbc.scatterCount;
        targetCfg = hmmsbc.targetCfg;
        damagePct = hmmsbc.damagePct;

        targetLst = new List<MainLogicUnit>();

        arrowCount = hmmsbc.arrowCount;
        arrowDelay = hmmsbc.arrowDelay;
        posOffset = (PEInt)hmmsbc.posOffset;

        targetHero = skill.lockTarget;

        //主箭多重射击
        MultiArrow(skill.lockTarget, skill.cfg.damage, false);

        var findLst = CalcRule.FindMulipleTargetByRule(owner, targetCfg, PEVector3.zero);
        int count = 0;
        for (int i = 0; i < findLst.Count; i++)
        {
            if (count < scatterCount)
            {
                if (findLst[i].Equals(targetHero))
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

            MultiArrow(targetLst[i], skill.cfg.damage * damagePct / 100, true);
        }
    }

    void MultiArrow(MainLogicUnit target, PEInt damage, bool isCurve = false)
    {
        for (int i = 0; i < arrowCount; i++)
        {
            TargetBullet bullet = source.CreateSkillBullet(source, target, skill) as TargetBullet;
            if (isCurve)
            {
                bullet.SetCurveDir();
            }
            bullet.SetDelayData((i + 1) * arrowDelay);

            if (i % 2 == 0)
            {
                bullet.SetOffsetPos(PEVector3.up * posOffset);
            }
            else
            {
                bullet.SetOffsetPos(PEVector3.up * -posOffset);
            }

            bullet.HitTargetCB = (MainLogicUnit hitTarget, object[] args) => {
                hitTarget.GetDamageByBuff(damage, this);
            };
        }
    }
}
