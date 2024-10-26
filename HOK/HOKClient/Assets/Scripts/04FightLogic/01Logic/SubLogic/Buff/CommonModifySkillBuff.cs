using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CommonModifySkillBuffCfg:BuffCfg
{
    /// <summary>
    /// 原始id
    /// </summary>
    public int originalID;
    /// <summary>
    /// 替换完的id
    /// </summary>
    public int replaceID;


}
/// <summary>
/// 技能替换buff
/// </summary>
public class CommonModifySkillBuff : Buff
{
    /// <summary>
    /// 原始id
    /// </summary>
    public int originalID;
    /// <summary>
    /// 替换完的id
    /// </summary>
    public int replaceID;
    /// <summary>
    /// 即将被修改的技能
    /// </summary>
    public Skill modifySkill;
    public CommonModifySkillBuff(MainLogicUnit source, MainLogicUnit owner, Skill skill, int buffID, object[] args = null) : base(source, owner, skill, buffID, args)
    {
    }
    public override void LogicInit()
    {
        base.LogicInit();

        CommonModifySkillBuffCfg mabc = cfg as CommonModifySkillBuffCfg;
        originalID = mabc.originalID;
        replaceID = mabc.replaceID;

        modifySkill = owner.GetSkillByID(originalID);

    }
    protected override void Start()
    {
        base.Start();

        modifySkill.ReplaceSkillCfg(replaceID);
        modifySkill.SpellSuccCallback += ReplaceSkillReleaseDone;
    }
    void ReplaceSkillReleaseDone(Skill skillReleased)
    {
        if(skillReleased.cfg.isNormalAttack)
        {
            unitState = SubUnitState.End;
        }
    }
    protected override void End()
    {
        base.End();

        modifySkill.ReplaceSkillCfg(originalID);
        modifySkill.SpellSuccCallback -= ReplaceSkillReleaseDone;

    }
}