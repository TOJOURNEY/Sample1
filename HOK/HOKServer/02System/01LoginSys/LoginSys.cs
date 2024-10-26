using HOKProtocol;
using PEUtils;
using System;
using System.Collections.Generic;
using System.Text;

namespace HOKServer
{
    /// <summary>
    /// 登陆系统
    /// </summary>
    class LoginSys:SystemRoot<LoginSys>
    {
        public override void Init()
        {
            base.Init();

            this.Log("LoginSys Init Done");

        }
        public override void Update()
        {
            base.Update();
        }
        public void ReqLogin(MsgPack pack)
        {
            ReqLogin data = pack.msg.reqLogin;

            HOKMsg msg = new HOKMsg
            {
                cmd = CMD.RspLogin
            };

            if(cacheSvc.IsAcctOnLine(data.acct))
            {
                //已上线返回错误信息，错误码
                msg.error = ErrorCode.AcctisOnline;
            }
            else
            {
                //未上线，并且无缓存，创建默认账号数据，并缓存
                uint sid = pack.session.GetSessionID();
                UserData ud = new UserData
                {
                    id = sid,
                    name = "龙组_" + sid,
                    lv = 18,
                    exp = 10086,
                    coin = 999,
                    diamond = 666,
                    ticket = 0,
                    heroSelectData = new List<HeroSelectData>
                    {
                        new HeroSelectData
                        {
                            heroID=101,
                        },
                        new HeroSelectData
                        {
                            heroID=102,
                        }
                    }
                };
                msg.rspLogin = new RspLogin
                {
                    userData = ud
                };
                cacheSvc.AcctOnline(data.acct, pack.session, ud);
            }
            pack.session.SendMsg(msg);
        }
        
    }
}
