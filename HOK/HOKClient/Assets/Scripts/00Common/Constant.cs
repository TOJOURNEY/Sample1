using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 常量定义
/// </summary>
public static class NameDefine
{
    //背景音效
    public const string MainCityBGMusic = "main"; 
    //登录按钮
    public const string MainCityUIAudio = "loginBtnClick";
    //开始界面按钮
    public const string MainCityUIStartBtnAudio = "com_click1";
    //通用点击
    public const string MainCityUISureBtnAudio = "com_click2";
    //1v1，3v3，5v5音效
    public const string MainCityUIMatchAudio = "matchBtnClick";
    public const string MainCityUIRankAudio = "matchBtnClick";
    public const string MainCityUISetingsAudio = "matchBtnClick"; 
    //匹配界面确认按钮
    public const string MainCityUIConfirmAudio = "matchReminder"; 
    public const string MainCityUIConfirmSureAudio = "matchSureClick";
     
    //选择界面
    public const string MainCityUISelectHeroClickAudio = "SelectHeroClick";

    /// <summary>
    ///加载界面音效
    /// </summary>
    public const string MainCityUILoadAudio = "Load";

    public const string BattleCDAudio = "com_cd_ok";


    public const string BattleBGMusic = "battle";
}
public static class ClientConfig
{
    public const int ScreenStandardWidth = 2160;
    public const int ScreenStandardHeight = 1080;

    public const int ScreenOPDis = 135;
    //通用的移动攻击buffID
    public const int CommonMoveAttackBuffID = 90000;

    public const int SkillOPDis = 125;

    public const int SkillCancelDis = 500;

}

