using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HOKProtocol;

public class GMSystem : SysRoot
{
    public static GMSystem Instance;
    public bool isActive = false;
    private uint frameID = 0;
    private List<OpKey> opkeyLst = new List<OpKey>();
    public override void InitSys()
    {
        base.InitSys();

        Instance = this;
        this.Log("Init GMSystem Done.");
    }
    public void StartSimulate()
    {
        isActive = true;
        StartCoroutine(BattleSimulate());
    }
    public IEnumerator BattleSimulate()
    {
        SimulateLoadRes();
        yield return new WaitForSeconds(0.5f);
        SimulateBattleStart();
    }
    void SimulateLoadRes()
    {
        HOKMsg msg = new HOKMsg
        {
            cmd = CMD.NtfLoadRes,
            ntfLoadRes = new NtfLoadRes
            {
                mapID = 101,
                heroList = new List<BattleHeroData> 
                {
                    new BattleHeroData { heroID = 102, userName = "龙族1" },
                    new BattleHeroData { heroID = 101 , userName = "凤族1" },

                    new BattleHeroData { heroID = 101, userName = "龙族2" },
                    new BattleHeroData { heroID = 102 , userName = "凤族2" },
                    new BattleHeroData { heroID = 101, userName = "龙族3" },
                    new BattleHeroData { heroID = 102 , userName = "凤族3" },
                },
                posIndex = 0
            }
        };
        LobbySys.Instance.NtfLoadRes(msg);
    }
    void SimulateBattleStart()
    {
        HOKMsg msg = new HOKMsg
        {
            cmd = CMD.RspBattleStart
        };
        BattleSys.Instance.RspBattleStart(msg);
    }
    public void SimulateServerRcvMsg(HOKMsg msg)
    {
        switch (msg.cmd)
        {
            case CMD.SndOpkey:
                UpdateOpeKey(msg.sndOpKey.opKey);
                break;
            default:
                break;
        }
    }
    void FixedUpdate()
    {
        if(isActive==false)
        {
            return;
        }
        ++frameID;
        HOKMsg msg = new HOKMsg
        {
            cmd = CMD.NtfOpkey,
            ntfOpKey = new NtfOpKey
            {
                frameID = frameID,
                keyList = new List<OpKey>()
            }
        };

        int count = opkeyLst.Count;
        if(count>0)
        {
            for (int i = 0; i < opkeyLst.Count; i++)
            {
                OpKey key = opkeyLst[i];
                msg.ntfOpKey.keyList.Add(key);
            }
        }
        opkeyLst.Clear();
        netSvc.AddMsgQue(msg);
    }
    void UpdateOpeKey(OpKey key)
    {
        opkeyLst.Add(key); 
    }
}
