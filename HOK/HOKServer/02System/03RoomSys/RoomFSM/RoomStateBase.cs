using System;
using System.Collections.Generic;
using System.Text;

namespace HOKServer
{
    public interface IRoomState
    {
        void Enter();
        void Update();
        void Exit();
    }
    /// <summary>
    /// 房间的状态基类
    /// </summary>
    public abstract class RoomStateBase : IRoomState
    {
        public PVPRoom room;
        public RoomStateBase(PVPRoom room)
        {
            this.room = room;
        }

        public abstract void Enter();

        public abstract void Exit();

        public abstract void Update();
    }

    public enum RoomStateEnum
    {
        None=0,
        Confirm,  //确认
        Select,   //选择
        Load,     //加载
        Fight,    //战斗
        End,      //完成
    }
}
