using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 小兵视图显示控制
/// </summary>
public class SoldierView : MainViewUnit
{
    Soldier soldier;
    public override void Init(LogicUnit logicUnit)
    {
        base.Init(logicUnit);
        soldier = logicUnit as Soldier;
    }

    protected override void Update()
    {
        base.Update();

        if (soldier.unitState == UnitStateEnum.Dead)
        {
            DestroySoldier();
            RmvUIItemInfo();
        }
    }

    void DestroySoldier()
    {
        Destroy(gameObject, 3f);
    }
}