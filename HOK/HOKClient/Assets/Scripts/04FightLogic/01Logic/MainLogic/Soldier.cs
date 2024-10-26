using PEMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 小兵
/// </summary>
public class Soldier : MainLogicUnit
{
    public int soldierID;
    public int waveIndex;
    public int orderIndex;
    public string soldierName;

    PEInt sqrSearchDis;
    TargetCfg cfg;

    public Soldier(SoldierData sd) : base(sd)
    {
        soldierID = sd.soldierID;
        waveIndex = sd.waveIndex;
        orderIndex = sd.orderIndex;

        unitType = UnitTypeEnum.Soldier;
        unitName = sd.unitCfg.unitName + " w:" + waveIndex + " o:" + orderIndex;

        pathPrefix = "ResChars";
    }

    public override void LogicInit()
    {
        base.LogicInit();
        sqrSearchDis = (PEInt)skillArr[0].cfg.targetCfg.searchDis * (PEInt)skillArr[0].cfg.targetCfg.searchDis;
        cfg = skillArr[0].cfg.targetCfg;
        InputMoveForwardKey();
    }

    private int AITickInterval = 5;
    private int AITickIntervalCounter = 0;
    public override void LogicTick()
    {
        base.LogicTick();
        if (AITickIntervalCounter < AITickInterval)
        {
            AITickIntervalCounter += 1;
            return;
        }
        else
        {
            AITickIntervalCounter = 0;
        }

        if (CanReleaseSkill(ud.unitCfg.skillArr[0]))
        {
            MainLogicUnit lockTarget = CalcRule.FindSingleTargetByRule(this, cfg, PEVector3.zero);
            if (lockTarget != null)
            {
                skillArr[0].ReleaseSkill(PEVector3.zero);
            }
            else
            {
                lockTarget = CalcRule.FindMinDisEnemyTarget(this, cfg);
                if (lockTarget != null)
                {
                    PEVector3 offsetDir = lockTarget.LogicPos - LogicPos;
                    PEInt sqrDis = offsetDir.sqrMagnitude;
                    if (sqrDis < sqrSearchDis)
                    {
                        InputFakeMoveKey(offsetDir.normalized);
                    }
                    else
                    {
                        InputMoveForwardKey();
                    }
                }
            }
        }
    }

    void InputMoveForwardKey()
    {
        if (IsTeam(TeamEnum.Blue))
        {
            InputFakeMoveKey(PEVector3.right);
        }
        else
        {
            InputFakeMoveKey(PEVector3.left);
        }
    }
}
