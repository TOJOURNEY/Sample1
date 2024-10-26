using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEMath;


/// <summary>
/// 移动攻击buff
/// </summary>
public class MoveAttackBuff : Buff
{
    MainLogicUnit moveTarget;
    SkillCfg atkSkillCfg;
    PEInt selectRange;
    PEInt searchDis;
    bool activeSkill;
    public MoveAttackBuff(MainLogicUnit source, MainLogicUnit owner, Skill skill, int buffID, object[] args = null) : base(source, owner, skill, buffID, args)
    {
    }
    public override void LogicInit()
    {
        base.LogicInit();

        atkSkillCfg = ResSvc.Instance.GetSkillConfigByID(skill.skillID);
        selectRange = (PEInt)atkSkillCfg.targetCfg.selectRange;
        searchDis = (PEInt)atkSkillCfg.targetCfg.searchDis;
        activeSkill = false;

    }
    protected override void Start()
    {
        base.Start();

        MoveTotarget();
    }
    protected override void Tick()
    {
        base.Tick();
        MoveTotarget();
    }
    void MoveTotarget()
    {
        moveTarget = CalcRule.FindMinDisEnemyTarget(owner, skill.cfg.targetCfg);
        if(moveTarget==null)
        {
            return;
        }
        else
        {
            PEVector3 offsetDir = moveTarget.LogicPos - owner.LogicPos;
            PEInt sqrDis = offsetDir.sqrMagnitude;
            PEInt sumRadius = owner.ud.unitCfg.colliCfg.mRadius + moveTarget.ud.unitCfg.colliCfg.mRadius;
            if(sqrDis<(selectRange+sumRadius)* (selectRange + sumRadius))
            {
                activeSkill = true;
                BattleSys.Instance.SendMoveKey(PEVector3.zero);
                unitState = SubUnitState.End;
            }
            else
            {
                if(sqrDis<(searchDis+sumRadius)*(searchDis+sumRadius))
                {
                    if(BattleSys.Instance.CheckUIInput())
                    {
                        //有ui输入，中断移动攻击
                        unitState = SubUnitState.End;
                    }
                    else
                    {
                        BattleSys.Instance.SendMoveKey(offsetDir.normalized);
                    }
                }
                else
                {
                    this.Log("超出搜索距离");
                    BattleSys.Instance.SendMoveKey(PEVector3.zero);
                    unitState = SubUnitState.End;
                }
            }
        }
    }
    protected override void End()
    {
        base.End();

        if(activeSkill)
        {
            activeSkill = false;
            this.Log("Send ReActive Skill Msg");
            BattleSys.Instance.SendSkillKey(skill.skillID);
        }
    }
}
