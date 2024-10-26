using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 沉默buff
/// </summary>
public class SilenseBuff_Single : Buff
{
    public SilenseBuff_Single(MainLogicUnit source, MainLogicUnit owner, Skill skill, int buffID, object[] args = null) : base(source, owner, skill, buffID, args)
    {
    }
    protected override void Start()
    {
        base.Start();
        owner.SilenceCount += 1;
    }
    protected override void End()
    {
        base.End();
        owner.SilenceCount -= 1;
    }
}
