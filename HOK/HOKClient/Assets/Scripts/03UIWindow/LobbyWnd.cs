using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HOKProtocol;


/// <summary>
/// 大厅界面
/// </summary>
public class LobbyWnd : WindowRoot
{
    public Text txtName;
    public Text txtLevel;
    public Text txtExp;
    public Image fgExp;

    public Text txtCoin;
    public Text txtDiamond;
    public Text txtTicket;

    public Transform transMatchRoot;
    public Text txtPredictTime; 
    public Text txtCountTime;

    protected override void InitWnd()
    {
        base.InitWnd();

        SetActive(transMatchRoot, false);
        UserData ud = root.UserData;

        txtName.text = ud.name;
        txtLevel.text = "LV."+ud.lv;
        txtExp.text = ud.exp+"/20000";
        fgExp.fillAmount = (ud.exp * 1.0f) / 100000;
        txtDiamond.text = ud.diamond.ToString();
        txtCoin.text = ud.coin.ToString();
        txtTicket.text = ud.ticket.ToString();
        txtCountTime.text = "00:00";

    }
    public void ShowMatchInfo(bool isActive,int predictTime=0)
    {
        if(isActive)
        {
            SetActive(transMatchRoot);
            isMatching = true;
            int min = predictTime / 60;
            int sec = predictTime % 60;
            string minStr = min < 10 ? "0" + min + ":" : min.ToString() + ":";
            string secStr = sec < 10 ? "0" + sec : sec.ToString();
            txtPredictTime.text = "预计匹配时间："+minStr + secStr;
        }
        else
        {
            SetActive(transMatchRoot, false);
            isMatching = false;
            timeCount = 0;
            deltaSum = 0;
        }
    }
    private int timeCount = 0;
    private float deltaSum = 0;
    private bool isMatching = false;
    private void Update()
    {
        if(isMatching)
        {
            float delta = Time.deltaTime;
            deltaSum += delta;
            if(deltaSum>=1)
            {
                deltaSum -= 1;
                timeCount += 1;
                SetCountTime();

            }
        }
    }
    private void SetCountTime()
    {
        int min = timeCount / 60;
        int sec = timeCount % 60;
        string minStr = min < 10 ? "0" + min + ":" : min.ToString() + ":";
        string secStr = sec < 10 ? "0" + sec : sec.ToString();
        txtCountTime.text = minStr + secStr;
    }
    public void ClickMatchBtn()
    {
        audioSvc.PlayUIAudio(NameDefine.MainCityUIMatchAudio);
        HOKMsg msg = new HOKMsg
        {
            cmd = CMD.ReqMatch,
            reqMatch = new ReqMatch
            {
                pvpEnum = PvPEnum._1v1
            }
        };
        netSvc.SendMsg(msg);

    }
    public void ClickRankBtn()
    {
        audioSvc.PlayUIAudio(NameDefine.MainCityUIRankAudio);
        HOKMsg msg = new HOKMsg
        {
            cmd = CMD.ReqMatch,
            reqMatch = new ReqMatch
            {
                pvpEnum = PvPEnum._3v3
            }
        };
        netSvc.SendMsg(msg);

    }
    public void ClickSetings()
    {
        audioSvc.PlayUIAudio(NameDefine.MainCityUISetingsAudio);
        HOKMsg msg = new HOKMsg
        {
            cmd = CMD.ReqMatch,
            reqMatch = new ReqMatch
            {
                pvpEnum = PvPEnum._5v5
            }
        };
        netSvc.SendMsg(msg);
    }
}
