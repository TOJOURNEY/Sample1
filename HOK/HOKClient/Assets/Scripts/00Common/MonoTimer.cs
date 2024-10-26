using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/// <summary>
/// CD计时器
/// </summary>
public class MonoTimer
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
    /// 计时器回调函数
    /// </summary>
    Action<int> cbAction;
    /// <summary>
    /// 计时时间间隔
    /// </summary>
    float intervelTime;
    /// <summary>
    /// 计时循环次数
    /// </summary>
    int loopCount;
    /// <summary>
    /// 进度显示
    /// </summary>
    Action<bool, float, float> prgAction;
    /// <summary>
    /// 计时器完成
    /// </summary>
    Action endAction;
    /// <summary>
    /// 延时
    /// </summary>
    float delayTime;
    /// <summary>
    /// 整个时间
    /// </summary>
    float prgAllTime;

    /// <summary>
    /// 延迟计数器
    /// </summary>
    float delayCounter;
    /// <summary>
    /// 回调计数器
    /// </summary>
    float cbCounter;
    /// <summary>
    /// 循环次数计数器
    /// </summary>
    int loopCounter;
    /// <summary>
    /// 总体进度计数器
    /// </summary>
    float prgCounter;
    /// <summary>
    /// 延时进度
    /// </summary>
    float prgLoopRate = 0;
    /// <summary>
    /// 总进度
    /// </summary>
    float prgAllRate = 0;


    public MonoTimer(
        Action<int> cbAction,
        float intervelTime,
        int loopCount=1,
        Action<bool,float,float> prgAction=null,
        Action endAction=null,
        float delayTime=0
        ){
        this.cbAction = cbAction;
        this.intervelTime = intervelTime;
        this.loopCount = loopCount;
        this.prgAction = prgAction;
        this.endAction = endAction;
        this.delayTime = delayTime;

        isActive = true;
        prgAllTime = delayTime + intervelTime * loopCount;


    }

    /// <summary>
    /// 驱动计时器运行
    /// </summary>
    /// <param name="delta">间隔的时间单位：ms</param>
    public void TickTimer(float delta)
    {
        if(IsActive)
        {
            if(delayTime>0&& delayCounter<delayTime)
            {
                delayCounter += delta;
                if(delayCounter>=delayTime)
                {
                    Tick(delayCounter - delayTime);
                }
                else
                {
                    //delay循环进度
                    prgLoopRate = delayCounter / delayTime;
                    if(prgAllTime>0)
                    {
                        prgCounter += delta;
                        prgAllRate = prgCounter / prgAllTime; 
                    }
                    prgAction?.Invoke(true,prgLoopRate,prgAllRate);
                }
            }
            else
            {
                Tick(delta);
            }
        }
        
    }
    void Tick(float delta)
    {
        cbCounter += delta;
        //当前这次循环进度
        prgLoopRate = cbCounter / intervelTime;
        //所有计时进度
        if (prgAllTime > 0)
        {
            prgCounter += delta;
            prgAllRate = prgCounter / prgAllTime;
        }

        prgAction?.Invoke(false, prgLoopRate, prgAllRate);

        if(cbCounter>=intervelTime)
        {
            ++loopCounter;
            cbAction(loopCounter);
            if(loopCount!=0&&loopCounter>=loopCount)
            {
                //达到最大循环次数
                IsActive = false;
                endAction?.Invoke();

                cbAction = null;
                prgAction = null;
                endAction = null;
            }
            else
            {
                //未达到最大循环次数
                cbCounter -= intervelTime;
            }
        }
    }
    public void DisableTimer()
    {
        IsActive = false;
        endAction?.Invoke();

        cbAction = null;
        prgAction = null;
        endAction = null;
    }
}
