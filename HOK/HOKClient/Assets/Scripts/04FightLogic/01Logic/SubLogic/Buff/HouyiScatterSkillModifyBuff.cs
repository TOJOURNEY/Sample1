using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class HouyiScatterSkillModifyBuffCfg : BuffCfg
{
    public int originalID;
    public int powerID;
    public int superPowerID;
}

/// <summary>
/// 后羿1技能修改buff
/// </summary>
public class HouyiScatterSkillModifyBuff : Buff
{
    int originalID;
    int powerID;
    int superPowerID;
    Skill modifySkill;

    public HouyiScatterSkillModifyBuff(MainLogicUnit source, MainLogicUnit owner, Skill skill, int buffID, object[] args = null)
        : base(source, owner, skill, buffID, args)
    {
    }

    public override void LogicInit()
    {
        base.LogicInit();

        HouyiScatterSkillModifyBuffCfg hssmbc = cfg as HouyiScatterSkillModifyBuffCfg;
        originalID = hssmbc.originalID;
        powerID = hssmbc.powerID;
        superPowerID = hssmbc.superPowerID;
        modifySkill = owner.GetSkillByID(originalID);
    }

    protected override void Start()
    {
        base.Start();

        if (modifySkill.TempSkillID == 0)
        {
            modifySkill.ReplaceSkillCfg(powerID);
        }
        else
        {
            modifySkill.ReplaceSkillCfg(superPowerID);
        }
    }

    protected override void End()
    {
        base.End();
        if (modifySkill.TempSkillID == powerID)
        {
            modifySkill.ReplaceSkillCfg(originalID);
        }
        else
        {
            modifySkill.ReplaceSkillCfg(1025);
        }
    }
}