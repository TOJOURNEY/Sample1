using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PEMath;
using System.Threading.Tasks;

/// <summary>
/// 单体移速buff
/// </summary>
public class MoveSpeedBuffCfg:BuffCfg
{
    /// <summary>
    /// 速度改变量,百分比
    /// </summary>
    public int amount;


}

public class MoveSpeedBuff_Single : Buff
{
    /// <summary>
    /// 偏移量
    /// </summary>
    private PEInt speedOffset;
    public MoveSpeedBuff_Single(MainLogicUnit source, MainLogicUnit owner, Skill skill, int buffID, object[] args = null) 
        : base(source, owner, skill, buffID, args){
    }
    public override void LogicInit()
    {
        base.LogicInit();

        MoveSpeedBuffCfg msbc = cfg as MoveSpeedBuffCfg;
        speedOffset = owner.moveSpeedBase * ((PEInt)msbc.amount / 100);

    }
    protected override void Start()
    {
        base.Start();
        owner.ModifyMoveSpeed(speedOffset, this, true);
    }

    protected override void End()
    {
        base.End();
        owner.ModifyMoveSpeed(-speedOffset, this, false);
    }

}
