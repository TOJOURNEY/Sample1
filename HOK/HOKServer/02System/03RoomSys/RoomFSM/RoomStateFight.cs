using HOKProtocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace HOKServer
{
    /// <summary>
    /// 对战进行
    /// </summary>
    public class RoomStateFight : RoomStateBase
    {
        uint frameID = 0;
        List<OpKey> opkeyLst = new List<OpKey>();
        int checkTaskID;

        private bool[] endArr;
        public RoomStateFight(PVPRoom room) : base(room)
        {
            int len = room.sessionArr.Length;
            endArr = new bool[len];
        }

        public override void Enter()
        {
            opkeyLst.Clear();
            checkTaskID = TimerSvc.Instance.AddTask(ServerConfig.ServerLogicFrameIntervelMs, SyncLogicFrame, null, 0);
        }

        void SyncLogicFrame(int tid)
        {
            ++frameID;
            HOKMsg msg = new HOKMsg
            {
                cmd = CMD.NtfOpkey,
                isEmpty = true,
                ntfOpKey = new NtfOpKey
                {
                    frameID = frameID,
                    keyList = new List<OpKey>()
                }
            };

            int count = opkeyLst.Count;
            if (count > 0)
            {
                msg.isEmpty = false;
                msg.ntfOpKey.keyList.AddRange(opkeyLst);
            }
            opkeyLst.Clear();
            room.BroadcastMsg(msg);
        }

        public override void Exit()
        {
            checkTaskID = 0;
            opkeyLst.Clear();
            endArr = null;
        }

        public override void Update() { }

        public void UpdateOpKey(OpKey key)
        {
            opkeyLst.Add(key);
        }

        public void UpdateEndState(int posIndex)
        {
            endArr[posIndex] = true;

            if (TimerSvc.Instance.DeleteTask(checkTaskID))
            {
                this.ColorLog(PEUtils.LogColorEnum.Green, "Delete Sync Task Success.");
            }
            else
            {
                this.Warn("Delete Sync Task Failed.");
            }
            room.ChangeRoomState(RoomStateEnum.End);
        }
    }
}
