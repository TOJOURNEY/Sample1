using HOKProtocol;
using PEMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class HouyiPasvAttackSpeedBuffCfg : BuffCfg
{
    public int overCount;//叠加层数
    public int speedAddtion;//加成百分比
    public int resetTime;
}
/// <summary>
/// houyi被动加攻速buff
/// </summary>
public class HouyiPasvAttackSpeedBuff : Buff
{
    int currOverCount;//叠加层数
    int maxOverCount;//最大叠加层数
    int resetTime;

    PEInt speedAddtion;
    PEInt speedOffset;

    public HouyiPasvAttackSpeedBuff(MainLogicUnit source, MainLogicUnit owner, Skill skill, int buffID, object[] args = null)
        : base(source, owner, skill, buffID, args)
    {
    }

    public override void LogicInit()
    {
        base.LogicInit();

        currOverCount = 0;
        HouyiPasvAttackSpeedBuffCfg hpasbc = cfg as HouyiPasvAttackSpeedBuffCfg;
        maxOverCount = hpasbc.overCount;
        resetTime = hpasbc.resetTime;
        speedAddtion = hpasbc.speedAddtion;
        speedOffset = PEInt.zero;

        Skill[] skillArr = source.GetAllSkill();
        for (int i = 0; i < skillArr.Length; i++)
        {
            skillArr[i].SpellSuccCallback += OnSpellSkillSucc;
        }
    }

    void OnSpellSkillSucc(Skill skillReleased)
    {
        if (skillReleased.cfg.isNormalAttack)
        {
            timeCount = 0;
            if (currOverCount >= maxOverCount)
            {
                return;
            }
            else
            {
                ++currOverCount;
                isCounter = true;
                PEInt addition = owner.AttackSpeedRateBase * (speedAddtion / 100);
                speedOffset += addition;
                owner.ModifyAttackSpeed(addition);
            }
        }
        else
        {
            if (skillReleased.skillID != 1021)
            {
                ResetSpeed();
            }
        }
    }

    int timeCount;
    bool isCounter;
    protected override void Tick()
    {
        base.Tick();
        if (isCounter)
        {
            timeCount += ServerConfig.ServerLogicFrameIntervelMs;
            if (timeCount >= resetTime)
            {
                ResetSpeed();
                timeCount = 0;
                isCounter = false;
            }
        }
    }

    void ResetSpeed()
    {
        owner.ModifyAttackSpeed(-speedOffset);
        speedOffset = PEInt.zero;
        currOverCount = 0;
    }
}