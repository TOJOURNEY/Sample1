using HOKProtocol;
using PEUtils;
using System;
using System.Collections.Generic;
using System.Text;



namespace HOKServer
{
    /// <summary>
    /// 资源服务
    /// </summary>
    public class CacheSvc : Singleto<CacheSvc>
    {
        /// <summary>
        /// 存储所有已上线的账号
        /// </summary>
        private Dictionary<string, ServerSession> onLineAcctDic;
        /// <summary>
        /// 玩家的游戏数据
        /// </summary>
        private Dictionary<ServerSession, UserData> onLineSessionDic;
        public override void Init()
        {
            base.Init();

            onLineAcctDic = new Dictionary<string, ServerSession>();
            onLineSessionDic = new Dictionary<ServerSession, UserData>();

            this.Log("CacheSvc Init Done");

        }
        public override void Update()
        {
            base.Update();
        }

        public bool IsAcctOnLine (string acct)
        {
            return onLineAcctDic.ContainsKey(acct);
        }
        public void AcctOnline(string acct, ServerSession session, UserData playerData)
        {
            onLineAcctDic.Add(acct, session);
            onLineSessionDic.Add( session, playerData);
        }
        public UserData GetUserDataBySession(ServerSession session)
        {
            if(onLineSessionDic.TryGetValue(session,out UserData playerData))
            {
                return playerData;
            }
            else
            {
                return null;
            }
        }
    }
}