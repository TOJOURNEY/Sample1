using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEMath;

/// <summary>
/// 动态查找目标修改移速
/// </summary>
public class MoveSpeedBuff_DynamicGroup : Buff
{
    PEInt speedOffset;
    public MoveSpeedBuff_DynamicGroup(MainLogicUnit source, MainLogicUnit owner, Skill skill, int buffID, object[] args = null) : base(source, owner, skill, buffID, args)
    {
    }

    public override void LogicInit()
    {
        base.LogicInit();

        targetLst = new List<MainLogicUnit>();
        targetLst.AddRange(CalcRule.FindMulipleTargetByRule(owner,cfg.impacter,skill.skillArgs));
        MoveSpeedBuffCfg msbc = cfg as MoveSpeedBuffCfg;
        speedOffset = msbc.amount;

    }
    protected override void Start()
    {
        base.Start();

        ModifyMoveSpeed(speedOffset,true);
    }
    protected override void Tick()
    {
        base.Tick();

        ModifyMoveSpeed(-speedOffset);

        targetLst.Clear();
        targetLst.AddRange(CalcRule.FindMulipleTargetByRule(owner, cfg.impacter, PEVector3.zero));
        ModifyMoveSpeed(speedOffset);

    }
    protected override void End()
    {
        base.End();

        ModifyMoveSpeed(-speedOffset);
        targetLst.Clear();
        targetLst = null;
    }
    void ModifyMoveSpeed(PEInt value,bool showJump=false)
    {
        for (int i = 0; i < targetLst.Count; i++)
        {
            PEInt offset = targetLst[i].moveSpeedBase * (value / 100);
            targetLst[i].ModifyMoveSpeed(offset, this, showJump);
        }
    }
}
