using HOKProtocol;
using PENet;
using PEUtils;
using System;
using System.Collections.Generic;
using System.Text;



namespace HOKServer
{
    public class MsgPack
    {
        public ServerSession session;
        public HOKMsg msg;
        public MsgPack(ServerSession session, HOKMsg msg)
        {
            this.session = session;
            this.msg = msg;
        }
    }
    /// <summary>
    /// 网络服务
    /// </summary>
    public class NetSvc:Singleto<NetSvc>
    {
        public static readonly string pkgque_lock = "pkgque_lock"; 
        private KCPNet<ServerSession, HOKMsg> server = new KCPNet<ServerSession, HOKMsg>();
        private Queue<MsgPack> msgPackQue = new Queue<MsgPack>();

        public override void Init()
        {
            base.Init();

            msgPackQue.Clear();

            //初始化
            KCPTool.LogFunc = this.Log;
            KCPTool.WarnFunc = this.Warn;
            KCPTool.ErrorFunc = this.Error;
            KCPTool.ColorLogFunc = (color, msg) =>
            {
                this.ColorLog((LogColorEnum)color, msg);
            };

#if DEBUG
            //服务器启动
            server.StartAsServer(ServerConfig.LocalDevInnerIP, ServerConfig.UdpPort);
#else
            server.StartAsServer(ServerConfig.RemoteServerIP, ServerConfig.UdpPort);
#endif
            this.Log("NetSvc Init Done.");
        }

        public void AddMsgQue(ServerSession session, HOKMsg msg)
        {
            lock (pkgque_lock)
            {
                msgPackQue.Enqueue(new MsgPack(session, msg));
            }
        }
        public override void Update()
        {
            base.Update();

            while (msgPackQue.Count > 0)
            {
                lock (pkgque_lock)
                {
                    MsgPack msg = msgPackQue.Dequeue();
                    HandoutMsg(msg);
                }
            }

        }

        /// <summary>
        /// 分发消息
        /// </summary>
        /// <param name="msg"></param>
        private void HandoutMsg(MsgPack pack)
        {
            switch (pack.msg.cmd)
            {
                case CMD.ReqLogin:
                    LoginSys.Instance.ReqLogin(pack);
                    break;
                case CMD.ReqMatch:
                    MatchSys.Instance.ReqMatch(pack);
                    break;
                case CMD.SndConfirm:
                    RoomSys.Instance.SndConfirm(pack);
                    break;
                case CMD.SndSelect:
                    RoomSys.Instance.SndSelect(pack);
                    break;
                case CMD.SndLoadPrg:
                    RoomSys.Instance.SndLoadPrg(pack);
                    break;

                case CMD.ReqBattleStart:
                    RoomSys.Instance.ReqBattleStart(pack);
                    break;

                case CMD.SndOpkey:
                    RoomSys.Instance.SndOpKey(pack);
                    break;
                case CMD.SndChat:
                    RoomSys.Instance.SndChat(pack);
                    break;
                case CMD.ReqBattleEnd:
                    RoomSys.Instance.ReqBattleEnd(pack);
                    break;
                case CMD.ReqPing:
                    SyncPingCMD(pack);
                    break;
                case CMD.None:
                default:
                    break;
            }
        }
        private void SyncPingCMD(MsgPack pack)
        {
            ReqPing req = pack.msg.reqPing;
            HOKMsg msg = new HOKMsg
            {
                cmd = CMD.RspPing,
                rspPing = new RspPing
                {
                    pingID = req.pingID
                }
            };
            pack.session.SendMsg(msg);
        }
    }
}
