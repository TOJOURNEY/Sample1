using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEMath;


public class HPCureBuffCfg:BuffCfg
{
    public int cureHPpct;
}
/// <summary>
/// 亚瑟被动血量回复buff
/// </summary>
public class HPCureBuff_Single : Buff
{
    public PEInt cureHPpct;
    public HPCureBuff_Single(MainLogicUnit source, MainLogicUnit owner, Skill skill, int buffID, object[] args = null) : base(source, owner, skill, buffID, args)
    {
    }

    public override void LogicInit()
    {
        base.LogicInit();

        HPCureBuffCfg hcbc = cfg as HPCureBuffCfg;
        cureHPpct = hcbc.cureHPpct;
    }
    protected override void Tick()
    {
        base.Tick();
        if(owner.unitState==UnitStateEnum.Alive)
        {
            owner.GetCureByBuff(owner.ud.unitCfg.hp*cureHPpct/100,this);
        }
    }

}
