using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEMath;
using HOKProtocol;

/// <summary>
/// 逻辑定时器
/// </summary>
public class LogicTimer
{
    /// <summary>
    /// 是否激活计时器
    /// </summary>
    bool isActive;
    /// <summary>
    /// 是否激活计时器
    /// </summary>
    public bool IsActive
    {
        private set
        {
            isActive = value;
        }
        get
        {
            return isActive;
        }
    }
    /// <summary>
    /// 延迟多久开始计时
    /// </summary>
    PEInt delayTime;
    PEInt loopTime;
    /// <summary>
    /// 每一帧的data时间，需要把66ms转化成定点数
    /// </summary>
    PEInt delta;
    /// <summary>
    /// 循环使用定时计数
    /// </summary>
    PEInt callbackCount;
    /// <summary>
    /// 回调函数
    /// </summary>
    Action cb;

    public LogicTimer(Action cb,PEInt delayTime,int loopTime=0)
    {
        this.cb = cb;
        this.delayTime = delayTime;
        this.loopTime = loopTime;

        delta = ServerConfig.ServerLogicFrameIntervelMs;
        IsActive = true;

    }

    public void TickTimer()
    {
        callbackCount += delta;
        if(callbackCount>=delayTime&&cb!=null)
        {
            cb();
            if(loopTime==0)
            {
                IsActive = false;
                cb = null;
            }
            else
            {
                callbackCount -= delayTime;
                delayTime = loopTime;
            }
        }

    }
}
