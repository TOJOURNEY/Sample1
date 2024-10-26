using PEUtils;
using UnityEngine;
using HOKProtocol;
using System.Collections.Generic;

public class GameRoot : MonoBehaviour
{
    public Transform uiRoot;
    public static GameRoot Instance;
    public TipsWnd tipsWnd;

    List<MonoTimer> tempTimerLst;
    List<MonoTimer> timerLst;
    void Start()
    {
        Instance = this;

        ///初始化配置日志文件
        LogConfig cfg = new LogConfig
        {
            enableLog = true,
            logPrefix = "",
            enableTime = true,
            logSeparate = ">",
            enableThreadID = true,
            enableTrace = true,
            enableSave = true,
            enableCover = true,
            saveName = "HOKClientPELog.txt",
            loggerType = LoggerType.Unity
        };
        PELog.InitSetting(cfg);

        PELog.ColorLog(LogColorEnum.Green, "InitLog");
        //随机数种子(TODO：服务器统一下发种子数据)
        RandomUtils.InitRandom(666);
        DontDestroyOnLoad(this);
        InitRoot();
        PELog.Log("Init Root");
        Init();
        PELog.Log("Init Done");
    }

    // Update is called once per frame
    void Update()
    {
        //View Timer Tick 
        //视图层驱动
        if(tempTimerLst.Count>=0)
        {
            timerLst.AddRange(tempTimerLst);
            tempTimerLst.Clear();
        }

        for (int i =  timerLst.Count-1;i>=0; --i)
        {
            MonoTimer timer = timerLst[i];
            if(timer.IsActive)
            {
                timer.TickTimer(Time.deltaTime * 1000);
            }
            else
            {
                timerLst.RemoveAt(i);
            }
        }
    }
    void InitRoot()
    {
        for (int i = 0; i < uiRoot.childCount; i++)
        {
            Transform trans = uiRoot.GetChild(i);
            trans.gameObject.SetActive(false);
        }
        tipsWnd.SetWndState();
    }
    private NetSvc netSvc;
    private ResSvc resSvc;
    private AudioSvc audioSvc;
    /// <summary>
    /// 初始化挂载各类管理服务脚本
    /// </summary>
    void Init()
    {
        //计时器
        timerLst = new List<MonoTimer>();
        tempTimerLst = new List<MonoTimer>();

        netSvc = GetComponent<NetSvc>();
        netSvc.InitSvc();
        resSvc = GetComponent<ResSvc>();
        resSvc.InitSvc();
        audioSvc = GetComponent<AudioSvc>();
        audioSvc.InitSvc();


        GMSystem gm = GetComponent<GMSystem>();
        gm.InitSys();


        LoginSys login = GetComponent<LoginSys>();
        login.InitSys();
        LobbySys lobby = GetComponent<LobbySys>();
        lobby.InitSys();
        BattleSys battle = GetComponent<BattleSys>();
        battle.InitSys();

        //login
        PELog.Log("EnterLogin");
        login.EnterLogin();
        
    }

    public void AddTips(string tips)
    {
        tipsWnd.AddTips(tips);
    }
    public void ShowTips(string tips)
    {
        tipsWnd.AddTips(tips);
    }
    public void AddMonoTimer(MonoTimer timer)
    {
        tempTimerLst.Add(timer);
    }
    #region
    UserData userData;
    public UserData UserData
    {
        set { userData = value; }
        get { return userData; }
    }
    private uint roomID;
    public uint RoomID
    {
        set { roomID = value; }
        get { return roomID; }
    }
    private int mapID;
    public int MapID
    {
        set { mapID = value; }
        get { return mapID; }
    }
    private List<BattleHeroData> heroLst;
    public List<BattleHeroData> HeroLst
    {
        set { heroLst = value; }
        get { return heroLst; }
    }
    private int selfIndex;
    public int SelfIndex
    {
        set { selfIndex = value; }
        get { return selfIndex; }
    }
    private int netDelay;
    public int NetDelay
    {
        set
        {
            netDelay = value;
        }
        get
        {
            return netDelay;
        }
    }
    #endregion
}
