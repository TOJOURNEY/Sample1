using HOKProtocol;
using PENet;
using System;
using System.Collections.Generic;
using System.Text;

namespace HOKServer
{
    /// <summary>
    /// 对战房间
    /// </summary>
    public class PVPRoom
    {
        public uint roomID;
        public PvPEnum pvpEnum = PvPEnum.None; 
        public ServerSession[] sessionArr;
        private SelectData[] selectArr = null;
        public SelectData[] SelectArr
        {
            set
            {
                selectArr = value;
            }
            get
            {
                return selectArr;
            }
        }

        private Dictionary<RoomStateEnum, RoomStateBase> fsm = new Dictionary<RoomStateEnum, RoomStateBase>();
        private RoomStateEnum currentRoomStateEnum = RoomStateEnum.None;

        public PVPRoom(uint roomID, PvPEnum pvpEnum, ServerSession[] sessionArr)
        {
            this.roomID = roomID;
            this.pvpEnum = pvpEnum;
            this.sessionArr = sessionArr;

            fsm.Add(RoomStateEnum.Confirm, new RoomStateConfirm(this));
            fsm.Add(RoomStateEnum.Select, new RoomStateSelect(this));
            fsm.Add(RoomStateEnum.Load, new RoomStateLoad(this));
            fsm.Add(RoomStateEnum.Fight, new RoomStateFight(this));
            fsm.Add(RoomStateEnum.End, new RoomStateEnd(this));

            ChangeRoomState(RoomStateEnum.Confirm);
        }
        public void ChangeRoomState(RoomStateEnum targetState)
        {
            if (currentRoomStateEnum == targetState)
            {
                return;
            }

            if (fsm.ContainsKey(targetState))
            {
                if (currentRoomStateEnum != RoomStateEnum.None)
                {
                    fsm[currentRoomStateEnum].Exit();
                }
                fsm[targetState].Enter();
                currentRoomStateEnum = targetState;
            }
        }
        public void BroadcastMsg(HOKMsg msg)
        {
            //优化，先进行序列化msg,不用每次都进行序列化发送，底层做了优化可以转化成二进制,可以节省cpu
            byte[] bytes = KCPTool.Serialize(msg);
            if (bytes != null)
            {
                for (int i = 0; i < sessionArr.Length; i++)
                {
                    sessionArr[i].SendMsg(bytes);
                    
                }
            }
        }
        int GetPosIndex(ServerSession session)
        {
            int posIndex = 0;
            for (int i = 0; i < sessionArr.Length; i++)
            {
                if(sessionArr[i].Equals(session))
                {
                    posIndex = i;
                }
            }
            return posIndex;
        }
        public void SndConfirm(ServerSession session)
        {
            if(currentRoomStateEnum==RoomStateEnum.Confirm)
            {
                if(fsm[currentRoomStateEnum] is RoomStateConfirm state)
                {
                    state.UpdateConfirmState(GetPosIndex(session));
                }
            }
        }
        public void SndSelect(ServerSession session,int heroID)
        {
            if (currentRoomStateEnum == RoomStateEnum.Select)
            {
                if (fsm[currentRoomStateEnum] is RoomStateSelect state)
                {
                    state.UpdateHeroSelect(GetPosIndex(session),heroID);
                }
            }
        }
        public void SndLoadPrg(ServerSession session, int percent)
        {
            if (currentRoomStateEnum == RoomStateEnum.Load)
            {
                if (fsm[currentRoomStateEnum] is RoomStateLoad state)
                {
                    state.UpdateLoadState(GetPosIndex(session), percent);
                }
            }
        }
        public void ReqBattleStart(ServerSession session)
        {
            if (currentRoomStateEnum == RoomStateEnum.Load)
            {
                if (fsm[currentRoomStateEnum] is RoomStateLoad state)
                {
                    state.UpdateLoadDone(GetPosIndex(session));
                }
            }
        }
        public void SndOpKey(OpKey opKey)
        {
            if (currentRoomStateEnum == RoomStateEnum.Fight)
            {
                if (fsm[currentRoomStateEnum] is RoomStateFight state)
                {
                    state.UpdateOpKey(opKey);
                }
            }
        }
        public void SndChat(string chatMsg)
        {
            HOKMsg msg = new HOKMsg
            {
                cmd = CMD.NtfChat,
                ntfChat = new NtfChat
                {
                    chatMsg = chatMsg
                }
            };
            BroadcastMsg(msg);
        }

        public void ReqBattleEnd(ServerSession session)
        {
            if (currentRoomStateEnum == RoomStateEnum.Fight)
            {
                if (fsm[currentRoomStateEnum] is RoomStateFight state)
                {
                    state.UpdateEndState(GetPosIndex(session));
                }
            }
        }
        public void Clear()
        {
            SelectArr = null;
            sessionArr = null;
            fsm = null;
        }
    }

}
