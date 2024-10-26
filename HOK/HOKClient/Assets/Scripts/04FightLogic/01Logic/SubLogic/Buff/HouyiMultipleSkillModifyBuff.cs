using HOKProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class HouyiMultipleSkillModifyBuffCfg : BuffCfg
{
    public int originalID;
    public int powerID;
    public int superPowerID;
    public int triggerOverCount;
    public int resetTime;
}
/// <summary>
/// 后羿被动强化普攻为多重射击，一次射出三支箭。（同一个目标）
/// </summary>
public class HouyiMultipleSkillModifyBuff : Buff
{
    int originalID;
    int powerID;
    int superPowerID;

    Skill modifySkill;

    int currOverCount;
    int triggerOverCount;
    int resetTime;

    public HouyiMultipleSkillModifyBuff(MainLogicUnit source, MainLogicUnit owner, Skill skill, int buffID, object[] args = null)
        : base(source, owner, skill, buffID, args)
    {
    }

    public override void LogicInit()
    {
        base.LogicInit();

        HouyiMultipleSkillModifyBuffCfg hpsmbc = cfg as HouyiMultipleSkillModifyBuffCfg;
        triggerOverCount = hpsmbc.triggerOverCount;
        originalID = hpsmbc.originalID;
        powerID = hpsmbc.powerID;
        superPowerID = hpsmbc.superPowerID;
        resetTime = hpsmbc.resetTime;
        modifySkill = owner.GetSkillByID(originalID);

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
            if (currOverCount >= triggerOverCount)
            {
                owner.mainViewUnit.SetImgInfo(resetTime);
                return;
            }
            else
            {
                ++currOverCount;
                if (currOverCount == triggerOverCount)
                {
                    isCounter = true;
                    owner.mainViewUnit.SetImgInfo(resetTime);
                    if (modifySkill.TempSkillID == 0)
                    {
                        modifySkill.ReplaceSkillCfg(powerID);
                    }
                    else
                    {
                        modifySkill.ReplaceSkillCfg(superPowerID);
                    }
                }
            }
        }
        else
        {
            if (skillReleased.skillID != 1021)
            {
                ResetSkill();
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
                ResetSkill();
                timeCount = 0;
                isCounter = false;
            }
        }
    }

    void ResetSkill()
    {
        currOverCount = 0;
        if (modifySkill.TempSkillID == powerID)
        {
            modifySkill.ReplaceSkillCfg(originalID);
        }
        else if (modifySkill.TempSkillID == superPowerID)
        {
            modifySkill.ReplaceSkillCfg(1024);
        }
        else
        {
            this.Log("reset skill alread done.");
        }

    }
}