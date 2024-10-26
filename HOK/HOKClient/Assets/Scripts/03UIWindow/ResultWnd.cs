using HOKProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;

/// <summary>
/// 战斗结算界面
/// </summary>
public class ResultWnd : WindowRoot
{
    public Image imgResult;
    public Image imgPrg;
    public Text txtTime;

    protected override void InitWnd()
    {
        base.InitWnd();
    }

    public void SetBattleResult(bool isSucc)
    {
        if (isSucc)
        {
            audioSvc.PlayUIAudio("victory");
            SetSprite(imgResult, "ResImages/ResultWnd/win");
        }
        else
        {
            audioSvc.PlayUIAudio("defeat");
            SetSprite(imgResult, "ResImages/ResultWnd/lose");
        }
        imgResult.SetNativeSize();

        CreateMonoTimer(
            (loopCount) => {
                SetText(txtTime, 5 - loopCount);
            },
            1000,
            5,
            (isDelay, loopPrg, allPrg) => {
                imgPrg.fillAmount = allPrg;
            },
            ClickContinueBtn,
            1000);
    }

    public void ClickContinueBtn()
    {
        if (gameObject.activeSelf)
        {
            HOKMsg msg = new HOKMsg
            {
                cmd = CMD.ReqBattleEnd,
                reqBattleEnd = new ReqBattleEnd
                {
                    roomID = root.RoomID
                }
            };
            netSvc.SendMsg(msg);
        }
    }
}

