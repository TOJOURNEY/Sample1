using PEMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class HouyiMultipleArrowBuffCfg : BuffCfg
{
    public int arrowCount;
    public int arrowDelay;
    public float posOffset;
}
/// <summary>
/// 被动多重射击buff
/// </summary>
public class HouyiMultipleArrowBuff : Buff
{
    int arrowCount;
    int arrowDelay;
    PEInt posOffset;

    public HouyiMultipleArrowBuff(MainLogicUnit source, MainLogicUnit owner, Skill skill, int buffID, object[] args = null)
        : base(source, owner, skill, buffID, args)
    {
    }

    MainLogicUnit targetHero;
    public override void LogicInit()
    {
        base.LogicInit();

        HouyiMultipleArrowBuffCfg hpmabc = cfg as HouyiMultipleArrowBuffCfg;
        arrowCount = hpmabc.arrowCount;
        arrowDelay = hpmabc.arrowDelay;
        posOffset = (PEInt)hpmabc.posOffset;

        targetHero = skill.lockTarget;

        for (int i = 0; i < arrowCount; i++)
        {
            TargetBullet bullet = source.CreateSkillBullet(source, targetHero, skill) as TargetBullet;
            bullet.SetDelayData((i + 1) * arrowDelay);

            if (i % 2 == 0)
            {
                bullet.SetOffsetPos(PEVector3.up * posOffset);
            }
            else
            {
                bullet.SetOffsetPos(PEVector3.up * -posOffset);
            }

            bullet.HitTargetCB = (MainLogicUnit target, object[] args) => {
                target.GetDamageByBuff(skill.cfg.damage, this);
            };
        }
    }
}