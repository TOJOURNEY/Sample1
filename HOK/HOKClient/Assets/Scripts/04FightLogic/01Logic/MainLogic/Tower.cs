using HOKProtocol;
using PEMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 塔和水晶
/// </summary>
public class Tower : MainLogicUnit
{
    public int towerID;
    public int towerIndex;

    public Tower(TowerData ud) : base(ud)
    {
        towerID = ud.towerID;
        towerIndex = ud.towerIndex;

        unitType = UnitTypeEnum.Tower;
        pathPrefix = "ResTower";
    }

    public override void LogicTick()
    {
        base.LogicTick();

        TickAI();
    }

    public override void LogicUnInit()
    {
        base.LogicUnInit();

        if (unitState == UnitStateEnum.Dead)
        {
            if (towerID == 1002)
            {
                this.Log("红方胜");
                if (BattleSys.Instance.GetSelfHero().IsTeam(TeamEnum.Blue))
                {
                    BattleSys.Instance.EndBattle(false);
                }
                else
                {
                    BattleSys.Instance.EndBattle(true);
                }
                BattleSys.Instance.isTickFight = false;
            }
            else if (towerID == 2002)
            {
                this.Log("蓝方胜");
                if (BattleSys.Instance.GetSelfHero().IsTeam(TeamEnum.Red))
                {
                    BattleSys.Instance.EndBattle(false);
                }
                else
                {
                    BattleSys.Instance.EndBattle(true);
                }
                BattleSys.Instance.isTickFight = false;
            }

            TowerView tv = mainViewUnit as TowerView;
            tv.DestroyTower();
        }
    }


    int aiIntervel = 200;
    int aiIntervelCounter = 0;
    void TickAI()
    {
        aiIntervelCounter += ServerConfig.ServerLogicFrameIntervelMs;
        if (aiIntervelCounter >= aiIntervel)
        {
            aiIntervelCounter -= aiIntervel;

            MainLogicUnit unit = SearchTarget();
            if (unit != null)
            {
                mainViewUnit.SetAtkSkillRange(true, skillArr[0].cfg.targetCfg.selectRange);
                if (CanReleaseSkill(ud.unitCfg.skillArr[0]))
                {
                    skillArr[0].ReleaseSkill(PEVector3.zero);
                }
            }
            else
            {
                mainViewUnit.SetAtkSkillRange(false);
            }
        }
    }

    MainLogicUnit SearchTarget()
    {
        return CalcRule.FindSingleTargetByRule(this, skillArr[0].cfg.targetCfg, PEVector3.zero);
    }
}

