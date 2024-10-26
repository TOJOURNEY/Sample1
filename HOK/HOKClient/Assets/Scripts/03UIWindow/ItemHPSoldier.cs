using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 小兵血条
/// </summary>
public class ItemHPSoldier : ItemHP
{
    public Image iconState;

    public override void InitItem(MainLogicUnit unit, Transform root, int hp)
    {
        base.InitItem(unit, root, hp);

        SetActive(iconState, false);
        if(isFriend)
        {
            SetSprite(imgPrg, "ResImages/PlayWnd/selfteamhpfg");
        }
        else
        {
            SetSprite(imgPrg, "ResImages/PlayWnd/enemyteamhpfg");
        }
    }
    public override void SetStateInfo(StateEnum state, bool show)
    {
        base.SetStateInfo(state, show);

        if(!show)
        {
            SetActive(iconState, false);
        }
        else
        {
            //血条下方图标显示
            switch (state)
            {
                case StateEnum.Silenced:
                    SetSprite(iconState, "ResImages/PlayWnd/silenceIcon");
                    break;
                case StateEnum.Knockup:
                    SetSprite(iconState, "ResImages/PlayWnd/stunIcon");
                    break;
                case StateEnum.Stunned:
                    SetSprite(iconState, "ResImages/PlayWnd/stunIcon");
                    break;


                case StateEnum.Invincible:
                case StateEnum.Restricted:
                case StateEnum.None:
                default:
                    break;
            }

            SetActive(iconState);
            iconState.SetNativeSize();
        }
    }
}

