using HOKProtocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace HOKServer
{
    /// <summary>
    /// 战斗结束
    /// </summary>
    public class RoomStateEnd : RoomStateBase
    {
        public RoomStateEnd(PVPRoom room) : base(room)
        {
        }

        public override void Enter()
        {
            HOKMsg msg = new HOKMsg
            {
                cmd = CMD.RspBattleEnd,
                rspBattleEnd = new RspBattleEnd
                {
                    //TOADD
                }
            };

            room.BroadcastMsg(msg);
            Exit();
        }

        public override void Exit()
        {
            RoomSys.Instance.DestroyRoom(room.roomID);
        }

        public override void Update()
        {
        }
    }
}
