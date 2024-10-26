using HOKProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;


public partial class PlayWnd
{
    public SkillItem skaItem;
    public SkillItem sk1Item;
    public SkillItem sk2Item;
    public SkillItem sk3Item;

    public Transform imgInfoRoot;
    public Image imgInfoCD;

    private bool isForbidReleaseSkill;
    public void InitSkillInfo()
    {
        BattleHeroData self = root.HeroLst[root.SelfIndex];
        UnitCfg heroCfg = resSvc.GetUnitConfigByID(self.heroID);
        int[] skillArr = heroCfg.skillArr;

        skaItem.InitSkillItem(resSvc.GetSkillConfigByID(skillArr[0]), 0);
        sk1Item.InitSkillItem(resSvc.GetSkillConfigByID(skillArr[1]),1);
        sk2Item.InitSkillItem(resSvc.GetSkillConfigByID(skillArr[2]),2);
        sk3Item.InitSkillItem(resSvc.GetSkillConfigByID(skillArr[3]),3);

        SetForbidState(false);
        SetActive(imgInfoRoot, false);
    }
    /// <summary>
    /// 禁止操作
    /// </summary>
    public void SetForbidState()
    {
        SetForbidState(true);
        isForbidReleaseSkill = true;
    }
    void UpdateSkillWnd()
    {
        if(isForbidReleaseSkill)
        {
            if(BattleSys.Instance.IsForbidSelfPlayerReleaseSkill()==false)
            {
                SetForbidState(false);
                isForbidReleaseSkill = false;
            }
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            skaItem.ClickSkillItem();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            sk1Item.ClickSkillItem();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            sk2Item.ClickSkillItem();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            sk3Item.ClickSkillItem();
        }



        if (Input.GetKeyDown(KeyCode.F1))
        {
            root.SelfIndex = 0;
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            root.SelfIndex = 1;
        }
        else if (Input.GetKeyDown(KeyCode.F3))
        {
            root.SelfIndex = 2;
        }
        else if (Input.GetKeyDown(KeyCode.F4))
        {
            root.SelfIndex = 3;
        }
        else if (Input.GetKeyDown(KeyCode.F5))
        {
            root.SelfIndex = 4;
        }
        else if (Input.GetKeyDown(KeyCode.F6))
        {
            root.SelfIndex = 5;
        }
    }
    /// <summary>
    /// 禁止操作
    /// </summary>
    void SetForbidState(bool state)
    {
        sk1Item.SetForbidState(state);
        sk2Item.SetForbidState(state);
        sk3Item.SetForbidState(state);
    }
    public void SetImgInfo(int cdTime)
    {
        SetActive(imgInfoRoot);
        showImgInfo = true;
        showTimeCounter = 0;
        showTime = cdTime * 1.0F / 1000;
    }
    bool showImgInfo;
    float showTimeCounter;
    float showTime;
    void UpdateImgInfo(float delta)
    {
        if (showImgInfo)
        {
            showTimeCounter += delta;
            if (showTimeCounter >= showTime)
            {
                showTimeCounter = 0;
                SetActive(imgInfoRoot, false);
                showImgInfo = false;
            }
            else
            {
                imgInfoCD.fillAmount = 1 - showTimeCounter / showTime;
            }
        }
    }
}

