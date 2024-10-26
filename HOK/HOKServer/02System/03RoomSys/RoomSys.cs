using HOKProtocol;
using PEUtils;
using System;
using System.Collections.Generic;                                                                                                                                                                                                                                                                                                                   
using System.Text;


namespace HOKServer
{
    /// <summary>
    /// 房间系统
    /// </summary>
    public class RoomSys : SystemRoot<RoomSys>
    {
        List<PVPRoom> pvpRoomlst = null;
        Dictionary<uint, PVPRoom> pvpRoomDic = null;
        public override void Init()
        {
            base.Init();

            pvpRoomlst = new List<PVPRoom>();
            pvpRoomDic = new Dictionary<uint, PVPRoom>();
            this.Log("RoomSys Init Done");                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  

        }
        public void AddPVPRoom(ServerSession[] sessionArr,PvPEnum pvp)
        {
            uint roomID = GetUniqueRoomID();
            PVPRoom room = new PVPRoom(roomID,pvp, sessionArr);
            pvpRoomlst.Add(room);
            pvpRoomDic.Add(roomID, room);
        }
        public override void Update()
        {
            base.Update();
        }

        public void SndConfirm(MsgPack pack)
        {
            SndConfirm req = pack.msg.sndConfirm;
            if(pvpRoomDic.TryGetValue(req.roomID,out PVPRoom room))
            {
                room.SndConfirm(pack.session);
            }
            else
            {
                this.Warn("PVPRoom ID:" + req.roomID+"is destroyed.");
            }
        }
        public void SndSelect(MsgPack pack)
        {
            SndSelect req = pack.msg.sndSelect;
            if (pvpRoomDic.TryGetValue(req.roomID, out PVPRoom room))
            {
                room.SndSelect(pack.session,req.heroID);
            }
            else
            {
                this.Warn("PVPRoom ID:" + req.roomID + "is destroyed.");
            }
        }

        public void SndLoadPrg(MsgPack pack)
        {
            SndLoadPrg req = pack.msg.sndLoadPrg;
            if (pvpRoomDic.TryGetValue(req.roomID, out PVPRoom room))
            {
                room.SndLoadPrg(pack.session, req.percent);
            }
            else
            {
                this.Warn("PVPRoom ID:" + req.roomID + " is destroyed.");
            }
        }
        public void ReqBattleStart(MsgPack pack)
        {
            ReqBattleStart req = pack.msg.reqBattleStart;
            if (pvpRoomDic.TryGetValue(req.roomID, out PVPRoom room))
            {
                room.ReqBattleStart(pack.session);
            }
            else
            {
                this.Warn("PVPRoom ID:" + req.roomID + " is destroyed.");
            }
        }
        public void SndOpKey(MsgPack pack)
        {
            SndOpKey snd = pack.msg.sndOpKey;
            if (pvpRoomDic.TryGetValue(snd.roomID, out PVPRoom room))
            {
                room.SndOpKey(snd.opKey);
            }
            else
            {
                this.Warn("PVPRoom ID:" + snd.roomID + " is not exist.");
            }
        }
        public void SndChat(MsgPack pack)
        {
            SndChat snd = pack.msg.sndChat;
            if (pvpRoomDic.TryGetValue(snd.roomID, out PVPRoom room))
            {
                room.SndChat(snd.chatMsg);
            }
            else
            {
                this.Warn("PVPRoom ID:" + snd.roomID + " is not exist.");
            }
        }

        public void ReqBattleEnd(MsgPack pack)
        {
            ReqBattleEnd snd = pack.msg.reqBattleEnd;
            if (pvpRoomDic.TryGetValue(snd.roomID, out PVPRoom room))
            {
                room.ReqBattleEnd(pack.session);
            }
            else
            {
                this.Warn("PVPRoom ID:" + snd.roomID + " is not exist.");
            }
        }
        uint roomID = 0;
        public uint GetUniqueRoomID()
        {
            roomID += 1;
            return roomID;
        }
        //clear room
        public void DestroyRoom(uint roomID)
        {
            if (pvpRoomDic.TryGetValue(roomID, out PVPRoom room))
            {
                room.Clear();

                int index = -1;
                for (int i = 0; i < pvpRoomlst.Count; i++)
                {
                    if (pvpRoomlst[i].roomID == roomID)
                    {
                        index = i;
                        break;
                    }
                }
                if (index >= 0)
                {
                    pvpRoomlst.RemoveAt(index);
                }
                pvpRoomDic.Remove(roomID);
            }
            else
            {
                this.Error("PVPRoom is not exist ID:" + roomID);
            }
        }
    }
}
