using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using HOKProtocol;

/// <summary>
/// 匹配确认界面
/// </summary>
public class MatchWnd:WindowRoot
{
    public Text txtTime;
    public Text txtConfirm;
    public Transform leftPlayerRoot;
    public Transform rightPlayerRoot;
    public Button btnConfirm;

    private int timeCount;
    protected override void InitWnd()
    {
        base.InitWnd();

        timeCount = ServerConfig.ConfirmCountDown;
        btnConfirm.interactable = true;
        audioSvc.PlayUIAudio(NameDefine.MainCityUIConfirmAudio);

    }
    public void RefreshUI(ConfirmData[] confirmArr)
    {
        int count = confirmArr.Length / 2;
        //left
        for (int i = 0; i < 5; i++)
        {
            Transform player = leftPlayerRoot.GetChild(i);
            if(i<count)
            {
                SetActive(player);
                string iconPath = "ResImages/MatchWnd/icon_" + confirmArr[i].iconIndex;
                string framePath = "ResImages/MatchWnd/frame_" + (confirmArr[i].confirmDone ? "sure" : "normal");
                Image imgicon = GetImage(player);
                SetSprite(imgicon, iconPath);
                Image imgFrame = GetImage(player,"img_state");
                SetSprite(imgFrame, framePath);
                imgFrame.SetNativeSize();
            }
            else
            {
                SetActive(player, false);
            }
        }

        //right
        for (int i = 0; i < 5; i++)
        {
            Transform player = rightPlayerRoot.GetChild(i);
            if (i < count)
            {
                SetActive(player);
                string iconPath = "ResImages/MatchWnd/icon_" + confirmArr[i+count].iconIndex;
                string framePath = "ResImages/MatchWnd/frame_" + (confirmArr[i+ count].confirmDone ? "sure" : "normal");
                Image imgicon = GetImage(player);
                SetSprite(imgicon, iconPath);
                Image imgFrame = GetImage(player, "img_state");
                SetSprite(imgFrame, framePath);
                imgFrame.SetNativeSize();
            }
            else
            {
                SetActive(player, false);
            }
        }


        int cofirmCount = 0;
        for (int i = 0; i < confirmArr.Length; i++)
        {
            if(confirmArr[i].confirmDone)
            {
                ++cofirmCount;
            }
        }

        txtConfirm.text = cofirmCount + "/" + confirmArr.Length + "就绪";

    }

    public void ClickConfirmBtn()
    {
        audioSvc.PlayUIAudio(NameDefine.MainCityUIConfirmSureAudio);

        HOKMsg msg = new HOKMsg
        {
            cmd = CMD.SndConfirm,
            sndConfirm = new SndConfirm
            {
                roomID = root.RoomID
            }
        };
        netSvc.SendMsg(msg);
        btnConfirm.interactable = false;
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
            if (timeCount<0)
            {
                timeCount = 0;
            }
             
            txtTime.text = timeCount.ToString();
        }
    }
}

