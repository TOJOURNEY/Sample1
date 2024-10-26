using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HOKProtocol;


/// <summary>
/// 开始界面
/// </summary>
public class StartWnd : WindowRoot
{
    public Text txtName;
    private UserData ud = null;
    protected override void InitWnd()
    {
        base.InitWnd();

        ud = root.UserData;
        txtName.text = ud.name;
    }

    public void ClickStarBth()
    {
        audioSvc.PlayUIAudio(NameDefine.MainCityUIStartBtnAudio);
        LoginSys.Instance.EnterLobby();
    }
    public void ClickAreaBth()
    {
        root.ShowTips("正在开发中，尽情期待");
    }
    public void ClickExitBth()
    {
        root.ShowTips("正在开发中，尽情期待");
    }
}
