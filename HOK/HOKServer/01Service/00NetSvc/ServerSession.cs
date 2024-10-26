using HOKProtocol;
using PENet;
using System;
using System.Collections.Generic;
using System.Text;

namespace HOKServer
{
    /// <summary>
    /// .NET Core 服务端Session连接
    /// </summary>
    public class ServerSession : KCPSession<HOKMsg>
    {
        protected override void OnConnected()
        {
            this.ColorLog(PEUtils.LogColorEnum.Green, "Client Online, Sid:{0} ", m_sid);
        }

        protected override void OnDisConnected()
        {
            this.Warn("Client Offlien, Sid:{0} ", m_sid);

        }

        protected override void OnReciveMsg(HOKMsg msg)
        {
            this.Log("RcvPack CMD:{0} ", msg.cmd.ToString());

            NetSvc.Instance.AddMsgQue(this,msg);
        }

        protected override void OnUpdate(DateTime now)
        {
            
        }
    }
}
