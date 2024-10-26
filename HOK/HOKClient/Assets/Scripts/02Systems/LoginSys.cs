using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HOKProtocol;


/// <summary>
/// 登录的业务系统
/// </summary>
public class LoginSys : SysRoot
{
    public static LoginSys Instance;

    public LoginWnd loginWnd;
    public StartWnd startWnd;
    public override void InitSys()
    {
        base.InitSys();

        Instance = this;
        this.Log("Init LoginSys Done");
    }
    /// <summary>
    /// 进入界面 
    /// </summary>
    public void EnterLogin()
    {
        loginWnd.SetWndState();
        audioSvc.PlayBGMusic(NameDefine.MainCityBGMusic);
    }

    public void RspLogin(HOKMsg msg)
    {
        root.ShowTips("登录成功");
        root.UserData = msg.rspLogin.userData;

        startWnd.SetWndState();
        loginWnd.SetWndState(false);
    }
    public void EnterLobby()
    {
        startWnd.SetWndState(false);
        LobbySys.Instance.EnterLobby();
    }
}
