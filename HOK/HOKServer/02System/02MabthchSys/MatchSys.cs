using HOKProtocol;
using System.Collections.Generic;

namespace HOKServer
{
    /// <summary>
    /// 匹配系统 
    /// </summary>
    class MatchSys : SystemRoot<MatchSys>
    {
        private Queue<ServerSession> que1v1 = null;
        private Queue<ServerSession> que3v3 = null;
        private Queue<ServerSession> que5v5 = null;
        public override void Init()
        {
            base.Init();

            que1v1 = new Queue<ServerSession>(); 
            que3v3 = new Queue<ServerSession>(); 
            que5v5 = new Queue<ServerSession>(); 

            this.Log("MabthchSys Init Done");

        }
        public override void Update()
        {
            base.Update();

            while (que1v1.Count >= 2)
            {
                ServerSession[] sessionArr = new ServerSession[2];
                for (int i = 0; i < 2; i++)
                {
                    sessionArr[i] = que1v1.Dequeue();
                }
                RoomSys.Instance.AddPVPRoom(sessionArr, PvPEnum._1v1);
            }

            while (que3v3.Count >= 6)
            {
                ServerSession[] sessionArr = new ServerSession[6];
                for (int i = 0; i < 6; i++)
                {
                    sessionArr[i] = que3v3.Dequeue();
                }
                RoomSys.Instance.AddPVPRoom(sessionArr, PvPEnum._3v3);
            }

            while (que5v5.Count >= 10)
            {
                ServerSession[] sessionArr = new ServerSession[10];
                for (int i = 0; i < 10; i++)
                {
                    sessionArr[i] = que5v5.Dequeue();
                }
                RoomSys.Instance.AddPVPRoom(sessionArr, PvPEnum._5v5);
            }
        }
        public void ReqMatch(MsgPack pack)
        {
            ReqMatch data = pack.msg.reqMatch;
            PvPEnum pvpEnum = data.pvpEnum;
            switch (pvpEnum)
            {
                case PvPEnum._1v1:
                    que1v1.Enqueue(pack.session);
                    break;
                case PvPEnum._3v3:
                    que3v3.Enqueue(pack.session);
                    break;
                case PvPEnum._5v5:
                    que5v5.Enqueue(pack.session);
                    break;
                case PvPEnum.None:
                default:
                    this.Error("PVPType Error:" + pvpEnum.ToString());
                    break;
            }
            HOKMsg msg = new HOKMsg
            {
                cmd = CMD.RspMatch,
                rspMatch = new RspMatch
                {
                    predictTime = GetPredictTime(pvpEnum),

                }
            };
            pack.session.SendMsg(msg);
        }
        private int GetPredictTime(PvPEnum pvpEnum)
        {
            int waitCount;
            switch (pvpEnum)
            {
                case PvPEnum._1v1:
                    waitCount = 2 - que1v1.Count;
                    if(waitCount<0)
                    {
                        waitCount = 0;
                    }
                    return waitCount * 10 + 5;
                case PvPEnum._3v3:
                    waitCount = 6 - que3v3.Count;
                    if (waitCount < 0)
                    {
                        waitCount = 0;
                    }
                    return waitCount * 10 + 5;
                case PvPEnum._5v5:
                    waitCount = 10 - que5v5.Count;
                    if (waitCount < 0)
                    {
                        waitCount = 0;
                    }
                    return waitCount * 10 + 5;
                default:
                    return 0;
            }
        }
    }
}
