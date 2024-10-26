using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HOKProtocol;
using PEUtils; 

/// <summary>
/// 登录界面 
/// </summary>
public class LoginWnd : WindowRoot
{
    public InputField iptAcct;
    public InputField iptPass;
    public Toggle togSrv;

    public Image imgPrgLoop;
    public Image imgPrgAll;
    public Text txtTime;

    protected override void InitWnd()
    {
        base.InitWnd();

        System.Random rd = new System.Random();
        iptAcct.text = rd.Next(100, 999).ToString();
        iptPass.text = rd.Next(100, 999).ToString();
    }
    public void ClickloginBth()
    {
        audioSvc.PlayUIAudio(NameDefine.MainCityUIAudio);
        if(iptAcct.text.Length>=3&&iptPass.text.Length>=3)
        {
            HOKMsg msg = new HOKMsg
            {
                cmd = CMD.ReqLogin,
                reqLogin = new ReqLogin
                {
                    acct = iptAcct.text,
                    pass = iptPass.text,
                }
            };

            netSvc.SendMsg(msg, (bool result) =>
             {
                 if (result == false)
                 {
                     netSvc.InitSvc();
                 }
             });
            //root.AddTips("请求登录服务器");
        }
        else
        {
            //POP Tips
            root.AddTips("账号或密码不符合规范");
        }
    }
    public void ClickGMBattleBtn()
    {
        SetWndState(false);
        GMSystem.Instance.StartSimulate();
    }
    MonoTimer testTimer;
    /// <summary>
    /// 点击按钮触发倒计时
    /// </summary>
    public void ClickTestBtn()
    {
        SetText(txtTime, 5);
        testTimer?.DisableTimer();
        testTimer = CreateMonoTimer(
            (loopCount) =>{
            this.ColorLog(LogColorEnum.Green, "Loop:" + loopCount);
            SetText(txtTime, 5 - loopCount);
            },
            1000,
            5,
            (isDelay, looPrg, allPrg) =>
            {
                SetActive(imgPrgLoop);
                if(isDelay)
                {
                    SetActive(txtTime, false);
                }
                else
                {
                    SetActive(txtTime);
                }
                imgPrgLoop.fillAmount = 1 - looPrg;
                imgPrgAll.fillAmount = allPrg;
            },
            () =>
            {
                SetActive(imgPrgLoop, false);
                imgPrgAll.fillAmount = 0;
                this.ColorLog(LogColorEnum.Green, "Loop End");
            },
            3000
            );
    }
}
