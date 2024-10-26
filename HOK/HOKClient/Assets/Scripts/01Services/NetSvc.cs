using HOKProtocol;
using PENet;
using PEUtils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 网络管理服务
/// </summary>
public class NetSvc : MonoBehaviour
{
    public static NetSvc Instance;

    public static readonly string pkgque_lock = "pkgque_lock";
    private KCPNet<ClientSession, HOKMsg> client = null;
    private Queue<HOKMsg> msgPackQue = null;
    private Task<bool> checkTask = null;

    public void InitSvc()
    {
        Instance = this;

        client = new KCPNet<ClientSession, HOKMsg>(); 
        msgPackQue= new Queue<HOKMsg>(); 


        //初始化
        KCPTool.LogFunc = this.Log;
        KCPTool.WarnFunc = this.Warn;
        KCPTool.ErrorFunc = this.Error;
        KCPTool.ColorLogFunc = (color, msg) =>
        {
            this.ColorLog((LogColorEnum)color, msg);
        };

        string srvIP = ServerConfig.RemoteGateIP;
        LoginSys login = LoginSys.Instance;
        if(login!=null)
        {
            if(login.loginWnd.togSrv.isOn)
            {
                //TODO
                srvIP = ServerConfig.LocalDevInnerIP;
            }
        }

        //服务器启动
        client.StartAsClient(srvIP, ServerConfig.UdpPort);
        checkTask= client.ConnectServer(100); 

        this.Log("Init NetSvc Done"); 
    }

    public void AddMsgQue(HOKMsg msg)
    {
        lock (pkgque_lock)
        {
            msgPackQue.Enqueue(msg);
        }
    }

    uint sendPingID = 0;
    int pingCounter = 0;
    Dictionary<uint, DateTime> pingDic = new Dictionary<uint, DateTime>();
    public void NetPing()
    {
        ++sendPingID;
        SendMsg(new HOKMsg
        {
            cmd = CMD.ReqPing,
            reqPing = new ReqPing
            {
                pingID = sendPingID,
                sendTime = KCPTool.GetUTCStartMilliseconds()
            }
        });

        //检测Ping有没有回应，累计三次没有回应，弹出提示
        if (pingDic.Count > 0)
        {
            ++pingCounter;
            if (pingCounter >= 3)
            {
                GameRoot.Instance.ShowTips("网络异常，检测手机网络环境");
                pingCounter = 0;
            }
        }
        pingDic.Add(sendPingID, DateTime.Now);
    }

    void RspPing(HOKMsg msg)
    {
        RspPing rsp = msg.rspPing;

        uint recivePingID = rsp.pingID;
        if (pingDic.ContainsKey(recivePingID))
        {
            TimeSpan ts = DateTime.Now - pingDic[recivePingID];
            GameRoot.Instance.NetDelay = (int)ts.TotalMilliseconds;
            pingDic.Clear();
            pingCounter = 0;
        }
        else
        {
            this.Warn("Net Ping ID Error:" + recivePingID);
        }
    }
    private int counter = 0;
    public void Update()
    {
        if(checkTask!=null&&checkTask.IsCompleted)
        {
            if(checkTask.Result)
            {
                //GameRoot.Instance.ShowTips("连接服务器成功");
                this.ColorLog( LogColorEnum.Green,"ConeectSrever Success");
                checkTask = null;

            }
            else
            {
                ++counter;
                if(counter>4)
                {
                    this.Error(string.Format("Coneect Failed {0} Times,Check Your Network Connection.", counter));
                    GameRoot.Instance.ShowTips("无法连接服务器，请检查网络状况");
                    checkTask = null;
                }
                else
                {
                    this.Warn(string.Format("Coneect Failed {0} Times ,Retry", counter));
                    checkTask = client.ConnectServer(100);
                }
                
            }
        }

        if (client != null&&client.clientSession!=null)
        {
            if (msgPackQue.Count > 0)
            {
                lock (pkgque_lock)
                {
                    HOKMsg msg = msgPackQue.Dequeue();
                    HandoutMsg(msg);
                }
            }
            return;
        }
        if(GMSystem.Instance.isActive)
        {
            if(msgPackQue.Count>0)
            {
                lock (pkgque_lock)
                {
                    HOKMsg msg = msgPackQue.Dequeue();
                    HandoutMsg(msg);
                }
            }
        }

    }
    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="cb"></param>
    public void SendMsg(HOKMsg msg, Action<bool> cb = null)
    {
        if (GMSystem.Instance.isActive)
        {
            GMSystem.Instance.SimulateServerRcvMsg(msg);
            cb?.Invoke(true);
            return;
        }

        if (client.clientSession != null && client.clientSession.IsConnected())
        {
            client.clientSession.SendMsg(msg);
            cb?.Invoke(true);
        }
        else
        {
            GameRoot.Instance.ShowTips("服务器未连接");
            this.Error("服务器未连接");
            cb?.Invoke(false);
        }
    }
    /// <summary>
    /// 分发消息
    /// </summary>
    /// <param name="msg"></param>
    private void HandoutMsg(HOKMsg msg)
    {
        if(msg.error!=ErrorCode.None)
        {
            switch(msg.error)
            {
                case ErrorCode.AcctisOnline:
                    GameRoot.Instance.ShowTips("当前账号已经上线");
                    break;
                default:
                    break;
            }
            return;
        }

        switch (msg.cmd)
        {
            case CMD.RspLogin:
                LoginSys.Instance.RspLogin(msg);
                break;
            case CMD.RspMatch:
                LobbySys.Instance.RspMatch(msg);
                break;
            case CMD.NtfConfirm:
                LobbySys.Instance.NtfConfirm(msg);
                break;
            case CMD.NtfSelect:
                LobbySys.Instance.NtfSelect(msg);
                break;
            case CMD.NtfLoadRes:
                LobbySys.Instance.NtfLoadRes(msg);
                break;
            case CMD.NtfLoadPrg:
                BattleSys.Instance.NtfLoadPrg(msg);
                break;
            case CMD.RspBattleStart:
                BattleSys.Instance.RspBattleStart(msg);
                break;
            case CMD.NtfOpkey:
                BattleSys.Instance.NtfOpKey(msg);
                break;
            case CMD.NtfChat:
                BattleSys.Instance.NtfChat(msg);
                break;
            case CMD.RspBattleEnd:
                BattleSys.Instance.RspBattleEnd(msg);
                break;
            case CMD.RspPing:
                RspPing(msg);
                break;
            default:
                break;
        }
    }
}
