using HOKProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 英雄的选择界面
/// </summary>
public class SelectWnd : WindowRoot
{
    public Image imgHeroShow;
    public Text txtCountTime;
    public Transform transScrollRoot;
    public GameObject heroItem;
    public Button btnSure;
    public Transform transSkillIconRoot;

    private int timeCount;
    private List<HeroSelectData> heroSelectLst = null;
    private bool isSelected = false;
    private int selectHeroID;

    protected override void InitWnd()
    {
        base.InitWnd();

        btnSure.interactable = true;
        isSelected = false;
        timeCount = ServerConfig.SelectCountDown;
        heroSelectLst = root.UserData.heroSelectData;

        for (int i = transScrollRoot.childCount - 1; i >= 0; --i)
        {
            DestroyImmediate(transScrollRoot.GetChild(i).gameObject);
        }

        for (int i = 0; i < heroSelectLst.Count; i++)
        {
            int heroID = heroSelectLst[i].heroID;
            GameObject go = Instantiate(heroItem);
            go.name = heroID.ToString();
            RectTransform rect = go.GetComponent<RectTransform>();
            rect.SetParent(transScrollRoot);
            rect.localScale = Vector3.one;
            UnitCfg cfg = resSvc.GetUnitConfigByID(heroID);
            SetSprite(GetImage(go.transform, "imgIcon"), "ResImages/SelectWnd/" + cfg.resName + "_head");
            SetText(GetText(go.transform, "txtName"), cfg.unitName);

            OnClick(go, ClickHeroItem, go, heroID);

            //默认数据
            if (i==0)
            {
                ClickHeroItem(null, new object[] { go, heroID });
            }
        }
    }
    void ClickHeroItem(PointerEventData ped,object[] args)
    {
        audioSvc.PlayUIAudio(NameDefine.MainCityUISelectHeroClickAudio);

        if(isSelected)
        {
            root.ShowTips("已经选定英雄");
            return;
        }
        GameObject go = args[0] as GameObject;

        for (int i = 0; i < transScrollRoot.childCount; i++)
        {
            Transform item = transScrollRoot.GetChild(i);
            Image selectGlow = GetImage(item, "state");
            if (item.gameObject.Equals(go))
            {
                SetSprite(selectGlow, "ResImages/SelectWnd/selectGlow");
            }
            else
            {
                SetSprite(selectGlow, "ResImages/MatchWnd/frame_normal");
            }
        }

        selectHeroID = (int)args[1];

        //选定英雄更换背景
        UnitCfg cfg = resSvc.GetUnitConfigByID(selectHeroID);
        SetSprite(imgHeroShow, "Resimages/SelectWnd/" + cfg.resName + "_show");

        //技能图标显示
        for (int i = 0; i < transSkillIconRoot.childCount; i++)
        {
            Image icon = GetImage(transSkillIconRoot.GetChild(i));
            SetSprite(icon, "ResImages/PlayWnd/" + cfg.resName + "_sk" + i);
        }
    }
    private float deltaCount;
    private void Update()
    {
        float delta = Time.deltaTime;
        deltaCount += delta;
        if(deltaCount>=1)
        {
            deltaCount -= 1;
            timeCount -= 1;
            if(timeCount<0)
            {
                timeCount = 0;
                //倒计时完成,强制默认为当前选择
                ClickSureBtn();
            }
             
            int min = timeCount / 60;
            int sec = timeCount % 60;
            string minStr = min < 10 ? "0" + min + ":" : min.ToString() + ":";
            string secStr = sec < 10 ? "0" + sec : sec.ToString();
            txtCountTime.text = minStr+secStr;
        }
    }
    public void ClickSureBtn()
    {
        audioSvc.PlayUIAudio(NameDefine.MainCityUISureBtnAudio);

        if(isSelected)
        {
            return;
        }

        HOKMsg msg = new HOKMsg
        {
            cmd = CMD.SndSelect,
            sndSelect = new SndSelect
            {
                roomID = root.RoomID,
                heroID=selectHeroID
            }
        };

        netSvc.SendMsg(msg);

        btnSure.interactable = false;
        isSelected = true;
    }
    protected override void UninitWnd()
    {
        base.UninitWnd();
    }
}
