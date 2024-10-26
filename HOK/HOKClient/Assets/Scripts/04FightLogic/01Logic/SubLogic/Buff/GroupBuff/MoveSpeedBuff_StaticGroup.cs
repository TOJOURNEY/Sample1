using PEMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 静态群体移速buff
/// </summary>
public class MoveSpeedBuff_StaticGroup : Buff
{
    PEInt speedOffset;

    public MoveSpeedBuff_StaticGroup(MainLogicUnit source, MainLogicUnit owner, Skill skill, int buffID, object[] args = null)
        : base(source, owner, skill, buffID, args)
    {
    }

    public override void LogicInit()
    {
        base.LogicInit();

        MoveSpeedBuffCfg msbc = cfg as MoveSpeedBuffCfg;
        speedOffset = msbc.amount;

        targetLst = new List<MainLogicUnit>();

        switch (msbc.staticPosType)
        {
            case StaticPosTypeEnum.SkillCasterPos:
                LogicPos = source.LogicPos;
                break;
            case StaticPosTypeEnum.SkillLockTargetPos:
                LogicPos = skill.lockTarget.LogicPos;
                break;
            case StaticPosTypeEnum.BulletHitTargetPos:
                LogicPos = (PEVector3)args[1];
                break;
            case StaticPosTypeEnum.UIInputPos:
                LogicPos = source.LogicPos + skill.skillArgs;
                break;
            case StaticPosTypeEnum.None:
            default:
                this.Error("static buff pos error.");
                break;
        }
    }

    protected override void Start()
    {
        base.Start();

        targetLst.AddRange(CalcRule.FindMulipleTargetByRule(source, cfg.impacter, LogicPos));
        ModifyGroupMoveSpeed(speedOffset, true);
    }

    protected override void Tick()
    {
        base.Tick();
        ModifyGroupMoveSpeed(-speedOffset);

        targetLst.Clear();
        targetLst.AddRange(CalcRule.FindMulipleTargetByRule(source, cfg.impacter, LogicPos));
        ModifyGroupMoveSpeed(speedOffset, false);
    }

    protected override void End()
    {
        base.End();

        ModifyGroupMoveSpeed(-speedOffset);
    }


    void ModifyGroupMoveSpeed(PEInt offset, bool showJump = false)
    {
        for (int i = 0; i < targetLst.Count; i++)
        {
            PEInt value = targetLst[i].moveSpeedBase * (offset / 100);
            targetLst[i].ModifyMoveSpeed(value, this, showJump);
        }
    }
}
