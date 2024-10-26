using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HOKProtocol;

public abstract class SubLogicUnit:LogicUnit
{
    /// <summary>
    /// 辅助单元来源角色
    /// </summary>
    public MainLogicUnit source;
    /// <summary>
    /// 辅助单位所属技能
    /// </summary>
    protected Skill skill;
    /// <summary>
    /// 延迟生效时间
    /// </summary>
    protected int delayTime;
    /// <summary>
    /// 延迟时间计数
    /// </summary>
    protected int delayCounter;
    /// <summary>
    /// 辅助单元状态
    /// </summary>
    public SubUnitState unitState;
    public SubLogicUnit (MainLogicUnit source,Skill skill)
    {
        this.source = source;
        this.skill = skill;
    }
    public override void LogicInit()
    {
        if(delayTime==0)
        {
            unitState = SubUnitState.Start;
        }
        else
        {
            delayCounter = delayTime;
            unitState = SubUnitState.Delay;
        }
    }
    public override void LogicTick()
    {
        switch (unitState)
        {
            case SubUnitState.Delay:
                delayCounter -= ServerConfig.ServerLogicFrameIntervelMs;
                if(delayCounter<=0)
                {
                    unitState = SubUnitState.Start;
                }
                break;
            case SubUnitState.End:
                End();
                unitState = SubUnitState.None;
                break;
            case SubUnitState.None:
            default:
                break;
        }
    }

    public override void LogicUnInit()
    {

    }

    protected abstract void Start();
    protected abstract void Tick();
    protected abstract void End();
}
/// <summary>
/// 辅助逻辑状态
/// </summary>
public enum SubUnitState
{
    None,
    Delay,
    Start,
    Tick,
    End
}
