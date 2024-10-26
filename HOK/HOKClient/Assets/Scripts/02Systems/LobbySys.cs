using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HOKProtocol;

/// <summary>
/// 大厅的业务系统
/// </summary>
public class LobbySys : SysRoot
{
    public static LobbySys Instance;
    public LobbyWnd lobbyWnd;
    public MatchWnd matchWnd;
    public SelectWnd selectWnd;
    public override  void InitSys()
    {
        base.InitSys();

        Instance = this;
        this.Log("Init LobbySys Done");
    }
    public void EnterLobby()
    {
        resSvc.AsyncLoadScene("MainCity",null,null);
        lobbyWnd.SetWndState();
    }
    public void RspMatch(HOKMsg msg)
    {
        int predictTime = msg.rspMatch.predictTime;
        lobbyWnd.ShowMatchInfo(true, predictTime);
    }
    public void NtfConfirm(HOKMsg msg)
    {
        NtfConfirm ntf = msg.ntfConfirm;

        if(ntf.dissmiss)
        {
            matchWnd.SetWndState(false);
            lobbyWnd.SetWndState();
        }
        else
        {
            root.RoomID = ntf.roomID;
            lobbyWnd.SetWndState(false);
            if (matchWnd.gameObject.activeSelf==false)
            {
                matchWnd.SetWndState();
            }
            matchWnd.RefreshUI(ntf.confirmArr);
        }
    }
    public void NtfSelect(HOKMsg msg)
    {
        matchWnd.SetWndState(false);
        selectWnd.SetWndState();

    }
    public void NtfLoadRes(HOKMsg msg)
    {
        root.MapID = msg.ntfLoadRes.mapID;
        root.HeroLst = msg.ntfLoadRes.heroList;
        root.SelfIndex = msg.ntfLoadRes.posIndex;
        selectWnd.SetWndState(false);
        BattleSys.Instance.EnterBattle();
    }
}
