using HOKProtocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace HOKServer
{
    /// <summary>
    /// 英雄选择
    /// </summary>
    public class RoomStateSelect : RoomStateBase
    {
        private SelectData[] selectArr = null;
        private int checkTaskID = -1;
        private bool isAllSelected = false;
        public RoomStateSelect(PVPRoom room) : base(room)
        {
        }

        public override void Enter()
        {
            int len = room.sessionArr.Length;
            selectArr = new SelectData[len];
            for (int i = 0; i < len; i++)
            {
                selectArr[i] = new SelectData
                {
                    selectID=0,
                    selectDone=false
                };
            }

            HOKMsg msg = new HOKMsg
            {
                cmd = CMD.NtfSelect
            };

            room.BroadcastMsg(msg);

            checkTaskID = TimerSvc.Instance.AddTask(ServerConfig.SelectCountDown * 1000, ReachTimeLimit);
        }
        void ReachTimeLimit(int tid)
        {
            if (isAllSelected)
            {
                return;
            }
            else
            {
                this.Warn("RoomID:{0} 玩家超时未确认选择，指定默认英雄。", room.roomID);
                for (int i = 0; i < selectArr.Length; i++)
                {
                    if(selectArr[i].selectDone==false)
                    {
                        selectArr[i].selectID = GetDefaultHeroSelect(i);
                        selectArr[i].selectDone = true;
                    }
                }
                room.SelectArr = selectArr;
                room.ChangeRoomState(RoomStateEnum.Load);
            }
        }
        int GetDefaultHeroSelect(int posIndex)
        {
            UserData ud = CacheSvc.Instance.GetUserDataBySession(room.sessionArr[posIndex]);
            if(ud!=null)
            {
                return ud.heroSelectData[0].heroID;
            }
            return 0;
        }
        void CheckSelectState()
        {
            for (int i = 0; i < selectArr.Length; i++)
            {
                if(selectArr[i].selectDone==false)
                {
                    return;
                }
            }
            isAllSelected = true;
        }
        public void UpdateHeroSelect(int posIndex,int heroID)
        {
            selectArr[posIndex].selectID = heroID;
            selectArr[posIndex].selectDone = true;
            CheckSelectState();
            if(isAllSelected)
            {
                //进入Load状态
                if(TimerSvc.Instance.DeleteTask(checkTaskID))
                {
                    this.ColorLog(PEUtils.LogColorEnum.Green, "RoomID:{0}所有玩家选择英雄完成,进入游戏加载", room.roomID);
                }
                else
                {
                    this.Warn("Remove CheckTaskID Failed.");
                }

                room.SelectArr = selectArr;
                room.ChangeRoomState(RoomStateEnum.Load);
            }
        }
        public override void Exit()
        {
            selectArr = null;
            checkTaskID = 0;
            isAllSelected = false;
        }

        public override void Update()
        {
            
        }
    }
}
