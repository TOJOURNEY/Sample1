using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 防御塔，水晶 血条显示
/// </summary>
public class ItemHPTower:ItemHP
{
    public override void InitItem(MainLogicUnit unit, Transform root, int hp)
    {
        base.InitItem(unit, root, hp);
        if(isFriend)
        {
            SetSprite(imgPrg, "ResImages/PlayWnd/selftowerhpfg");
        }
        else
        {
            SetSprite(imgPrg, "ResImages/PlayWnd/enemytowerhpfg");
        }
    }
}
