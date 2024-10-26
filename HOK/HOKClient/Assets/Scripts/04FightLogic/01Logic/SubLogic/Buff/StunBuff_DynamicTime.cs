using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class StunBuffCfg_DynamicTime : BuffCfg
{
    public int minStunTime;
    public int maxStunTime;
}
/// <summary>
/// 子弹命中动态晕眩时间Buff
/// </summary>
public class StunBuff_DynamicTime : Buff
{
    public StunBuff_DynamicTime(MainLogicUnit source, MainLogicUnit owner, Skill skill, int buffID, object[] args = null)
        : base(source, owner, skill, buffID, args)
    {
    }

    public override void LogicInit()
    {
        base.LogicInit();

        StunBuffCfg_DynamicTime dtsbc = cfg as StunBuffCfg_DynamicTime;
        int argsTime = (int)args[0];
        if (argsTime < dtsbc.minStunTime)
        {
            argsTime = dtsbc.minStunTime;
        }
        if (argsTime > dtsbc.maxStunTime)
        {
            argsTime = dtsbc.maxStunTime;
        }
        buffDuration = argsTime;
    }

    protected override void Start()
    {
        base.Start();

        owner.StunnedCount += 1;
    }

    protected override void End()
    {
        base.End();

        owner.StunnedCount -= 1;
    }
}