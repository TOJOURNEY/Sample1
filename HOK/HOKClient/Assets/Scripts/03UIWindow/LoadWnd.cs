using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HOKProtocol;


/// <summary>
/// 加载窗口
/// </summary>
public class LoadWnd : WindowRoot
{
    public Transform blueTeamRoot;
    public Transform redTeamRoot;

    private List<BattleHeroData> heroLst;
    private List<Text> txtPercentLst;
    protected override void InitWnd()
    {
        base.InitWnd();

        txtPercentLst = new List<Text>();
        audioSvc.PlayUIAudio(NameDefine.MainCityUILoadAudio);

        heroLst = root.HeroLst;

        int count = heroLst.Count / 2;
        //blueteam
        for (int i = 0; i < 5; i++)
        {
            Transform player = blueTeamRoot.GetChild(i);
            if(i<count)
            {
                SetActive(player);
                UnitCfg cfg = resSvc.GetUnitConfigByID(heroLst[i].heroID);
                SetSprite(GetImage(player, "imgHero"), "ResImages/LoadWnd/" + cfg.resName + "_load");
                SetText(GetText(player, "txtHeroName"), cfg.unitName);
                SetText(GetText(player, "bgName/txtPlayerName"),heroLst[i].userName);
                Text txtPrg = GetText(player, "txtProgress");
                txtPercentLst.Add(txtPrg);
                SetText(txtPrg, "0%");
            }
            else
            {
                SetActive(player, false);
            }
        }

        //redteam
        for (int i = 0; i < 5; i++)
        {
            Transform player = redTeamRoot.GetChild(i);
            if (i < count)
            {
                SetActive(player);
                UnitCfg cfg = resSvc.GetUnitConfigByID(heroLst[i+count].heroID);
                SetSprite(GetImage(player, "imgHero"), "ResImages/LoadWnd/" + cfg.resName + "_load");
                SetText(GetText(player, "txtHeroName"), cfg.unitName);
                SetText(GetText(player, "bgName/txtPlayerName"), heroLst[i+count].userName);
                Text txtPrg = GetText(player, "txtProgress");
                txtPercentLst.Add(txtPrg);
                SetText(txtPrg, "0%");
            }
            else
            {
                SetActive(player, false);
            }
        }
    }
    /// <summary>
    /// 刷新进度数据
    /// </summary>
    /// <param name="percentLst">进度数据</param>
    public void RefreshPrgData(List<int> percentLst)
    {
        for (int i = 0; i < percentLst.Count; i++)
        {
            txtPercentLst[i].text = percentLst[i] + "%"; 
        }
    }
}
