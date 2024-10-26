using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HOKProtocol;
using UnityEngine;

public class Buff : SubLogicUnit
{
    /// <summary>
    /// Buff附着单位
    /// </summary>
    public MainLogicUnit owner;
    protected int buffID;
    protected object[] args;

    protected int buffDuration;
    /// <summary>
    /// Dot计数
    /// </summary>
    int tickCount = 0;
    /// <summary>
    /// 时长计时
    /// </summary>
    int durationCount = 0;
    public BuffCfg cfg;
    /// <summary>
    /// 群体buff作用目标列表
    /// </summary>
    protected List<MainLogicUnit> targetLst;
    /// <summary>
    /// 表现数据
    /// </summary>
    BuffView buffView;
    public Buff(MainLogicUnit source,MainLogicUnit owner, Skill skill,int buffID,object[] args=null) 
        : base(source, skill){
        this.owner = owner;
        this.buffID = buffID;
        this.args = args;
    }
    public override void LogicInit()
    {
        cfg = ResSvc.Instance.GetBuffConfigByID(buffID);
        buffDuration = cfg.buffDuration;
        delayTime = cfg.buffDelay;

        base.LogicInit();
    }
    public override void LogicTick()
    {
        base.LogicTick();
        switch (unitState)
        {
            case SubUnitState.Start:
                Start();
                if (buffDuration > 0 || buffDuration == -1)
                {
                    unitState = SubUnitState.Tick;
                }
                else
                {
                    unitState = SubUnitState.End;
                }
                break;
            case SubUnitState.Tick:
                if (cfg.buffInterval > 0)
                {
                    tickCount += ServerConfig.ServerLogicFrameIntervelMs;
                    if (tickCount >= cfg.buffInterval)
                    {
                        tickCount -= cfg.buffInterval;
                        Tick();
                    }
                }
                durationCount += ServerConfig.ServerLogicFrameIntervelMs;
                if (durationCount >= buffDuration && buffDuration != -1)
                {
                    unitState = SubUnitState.End;
                }
                break;
        }
    }
    protected override void Start()
    {
        //根据staticPosType决定Buff初始位置
        if (cfg.staticPosType == StaticPosTypeEnum.None)
        {
            LogicPos = owner.LogicPos;
        }

        if (cfg.buffEffect != null)
        {
            GameObject go = ResSvc.Instance.LoadPrefab("ResEffects/" + cfg.buffEffect);
            go.name = source.unitName + "_" + cfg.buffName;
            buffView = go.GetComponent<BuffView>();
            if (buffView == null)
            {
                this.Error("Get BuffView Error:" + unitName);
            }

            if (cfg.staticPosType == StaticPosTypeEnum.None)
            {
                owner.mainViewUnit.SetBuffFollower(buffView);
            }
            buffView.Init(this);

            if (cfg.buffAudio != null)
            {
                buffView.PlayAudio(cfg.buffAudio);
            }
        }
        else
        {
            if (cfg.buffAudio != null)
            {
                owner.PlayAudio(cfg.buffAudio);
            }
        }
    }

    protected override void Tick()
    {
        if (cfg.hitTickAudio != null && targetLst.Count > 0)
        {
            owner.PlayAudio(cfg.hitTickAudio);
        }
    }
    protected override void End()
    {
        if (cfg.buffEffect != null)
        {
            buffView.DestroyBuff();
        }
    }
}
