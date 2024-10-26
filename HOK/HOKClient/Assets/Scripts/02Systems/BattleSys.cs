using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HOKProtocol;
using PEPhysx;
using PEMath;

/// <summary>
/// 战斗系统
/// </summary>
public class BattleSys : SysRoot
{
    public static BattleSys Instance;
    public LoadWnd loadWnd;
    public PlayWnd playWnd;
    public HPWnd hpWnd;
    public ResultWnd resultWnd;

    public float SkillDisMultipler;
    
    public bool isTickFight;
    private int mapID;

    private List<BattleHeroData> heroLst = null;
    private GameObject fightGO;
    private FightMgr fightMgr;
    private AudioSource battleAudio;
    uint keyID = 0;
    public uint KeyID
    {
        get
        {
            return ++keyID;
        }
    }
    public override void InitSys()
    {
        base.InitSys();

        Instance = this;
        this.Log("Init BattleSys Done");
    }
    public void EnterBattle()
    {
        audioSvc.StopBGMusic();
        loadWnd.SetWndState();


        mapID = root.MapID;
        heroLst = root.HeroLst;

        resSvc.AsyncLoadScene("map_" + mapID, SceneLoadProgress, SceneLoadDone);
    }
    public void EndBattle(bool isSucc)
    {
        playWnd.SetWndState(false);
        StartCoroutine(ShowResult(isSucc));
    }

    IEnumerator ShowResult(bool isSucc)
    {
        yield return new WaitForSeconds(0.5f);
        resultWnd.SetWndState();
        resultWnd.SetBattleResult(isSucc);
    }
    int lastPercent = 0;
    /// <summary>
    /// 加载的进度
    /// </summary>
    /// <param name="val"></param>
    void SceneLoadProgress(float val)
    {
        int percent = (int)(val * 100);
        //优化加载进度 
        //每帧进行发送数据到服务器加载进度，有可能一两帧的数据是相同的没必要发送
        //所以记录下加载进度如果加载进度不同在进行发送
        if(lastPercent!=percent)
        {
            HOKMsg msg = new HOKMsg
            {
                cmd = CMD.SndLoadPrg,
                sndLoadPrg = new SndLoadPrg
                {
                    roomID = root.RoomID,
                    percent = percent
                }
            };
            netSvc.SendMsg(msg);
            lastPercent = percent;
        }
        
    }
    /// <summary>
    /// 加载完的回调
    /// 告诉服务器战斗开始
    /// </summary>
    void SceneLoadDone()
    {
        //初始化UI
        playWnd.SetWndState();
        hpWnd.SetWndState();
        //加载角色及资源
        //初始化战斗
        fightGO = new GameObject
        {
            name = "fight"
        };
        fightMgr = fightGO.AddComponent<FightMgr>();
        battleAudio = fightGO.AddComponent<AudioSource>();
        MapCfg mapCfg = resSvc.GetMapConfigByID(mapID);
        fightMgr.Init(heroLst, mapCfg);

        HOKMsg msg = new HOKMsg
        {
            cmd = CMD.ReqBattleStart,
            reqBattleStart = new ReqBattleStart
            {
                roomID = root.RoomID
            }
        };
        netSvc.SendMsg(msg);
    }
    public void NtfLoadPrg(HOKMsg msg)
    {
        loadWnd.RefreshPrgData(msg.ntfLoadPrg.percentLst);
    }
    public void RspBattleStart(HOKMsg msg)
    {
        fightMgr.InitCamFollowTrans(root.SelfIndex);
        playWnd.InitSkillInfo();
        loadWnd.SetWndState(false);

        audioSvc.PlayBGMusic(NameDefine.BattleBGMusic);

        isTickFight = true;
        PlayBattleFieldAudio("welcombattle");

    }
    public void NtfOpKey(HOKMsg msg)
    {
        
        if(isTickFight)
        {
            fightMgr.InputKey(msg.ntfOpKey.keyList);
            fightMgr.Tick();
        }
    }
    public void NtfChat(HOKMsg msg)
    {
        string chatMsg = msg.ntfChat.chatMsg;
        playWnd.AddChatMsg(chatMsg);
    }

    public void RspBattleEnd(HOKMsg msg)
    {
        playWnd.SetWndState(false);
        hpWnd.SetWndState(false);
        resultWnd.SetWndState(false);
        if (fightMgr != null)
        {
            fightMgr.UnInit();
        }
        DestroyImmediate(fightGO);
        LobbySys.Instance.EnterLobby();
        audioSvc.PlayBGMusic(NameDefine.MainCityBGMusic);
    }
    public void RefreshIncome(int income)
    {
        playWnd.RefreshIncome(income);
    }
    public void SetKillData(TeamEnum killHeroTeam)
    {
        playWnd.SetKillData(killHeroTeam);
    }
    public void SetReviveState(bool isRevive, int reviveSec = 0)
    {
        playWnd.SetReviveTime(isRevive, reviveSec);
    }
    public bool CheckUIInput()
    {
        return playWnd.IsUIInput();
    }
    /// <summary>
    /// 技能cd
    /// </summary>
    /// <param name="skillID">技能</param>
    /// <param name="cdTime">cd时间</param>
    public void EnterCDState(int skillID,int cdTime)
    {
        playWnd.EnterCDState(skillID, cdTime);
    }
    public MainLogicUnit GetSelfHero()
    {
        return fightMgr.GetSelfHero(root.SelfIndex);
    }
    //public void SendSkillKey(int skillID)
    //{
    //    SendSkillKey(skillID, Vector3.zero);
    //}

    public TeamEnum GetCurrentUserTeam()
    {
        int sep = heroLst.Count / 2;
        if(root.SelfIndex<sep)
        {
            return TeamEnum.Blue;
        }
        else
        {
            return TeamEnum.Red;
        }
    }

    public List<PEColliderBase> GetEnvColliders()
    {
        return fightMgr.GetEnvColliders();
    }
    public void AddBullet(Bullet bullet)
    {
        fightMgr.AddBullet(bullet);
    }
    public void PlayBattleFieldAudio(string name)
    {
        audioSvc.PlayEntityAudio(name, battleAudio);
    }
    #region API  Func
    /// <summary>
    /// 发送移动帧操作到服务器
    /// </summary>
    /// <param name="logicDir"></param>
    /// <returns></returns> 
    public bool SendMoveKey(PEVector3 logicDir)
    {
        //if(CanMove())
        //{
            HOKMsg msg = new HOKMsg
            {
                cmd = CMD.SndOpkey,
                sndOpKey = new SndOpKey
                {
                    roomID = root.RoomID,
                    opKey = new OpKey
                    {
                        opIndex = root.SelfIndex,
                        keyType = KeyType.Move,
                        moveKey = new MoveKey(),
                    }
                }
            };

            msg.sndOpKey.opKey.moveKey.x = logicDir.x.ScaledValue;
            msg.sndOpKey.opKey.moveKey.z = logicDir.z.ScaledValue;
            msg.sndOpKey.opKey.moveKey.keyID = KeyID;
            NetSvc.Instance.SendMsg(msg);
            return true;
        //}
        //else
        //{
        //    return false;
        //}

    }
    public void SendSkillKey(int skillID)
    {
        SendSkillKey(skillID, Vector3.zero);
    }
    /// <summary>
    /// 发送技能释放指令
    /// </summary>
    /// <param name="skillID"></param>
    /// <param name="vec"></param>
    public void SendSkillKey(int skillID, Vector3 vec)
    {
        if (CanReleaseSkill(skillID))
        {
            //this.Log($"Rls Skill:{skillID} with Data:{vec}");
            HOKMsg netSkillMsg = new HOKMsg
            {
                cmd = CMD.SndOpkey,
                sndOpKey = new SndOpKey
                {
                    roomID = root.RoomID,
                    opKey = new OpKey
                    {
                        opIndex = root.SelfIndex,
                        keyType = KeyType.Skill,
                        skillKey = new SkillKey
                        {
                            skillID = (uint)skillID,
                            x_value = ((PEInt)vec.x).ScaledValue,
                            z_value = ((PEInt)vec.z).ScaledValue,

                        }
                    }
                }
            };
            netSvc.SendMsg(netSkillMsg);
        }
        else
        {
            this.Log("Skill can not release.");
        }
    }
    bool CanReleaseSkill(int skillID)
    {
        return fightMgr.CanReleaseSkill(root.SelfIndex, skillID);
    }
    public bool IsForbidSelfPlayerReleaseSkill()
    {
        return fightMgr.IsForbidReleaseSkill(root.SelfIndex);
    }
    bool CanMove()
    {
        return fightMgr.CanMove(root.SelfIndex);
    }
    #endregion
}
