using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEPhysx;
using PEMath;
using HOKProtocol;


/// <summary>
/// 主要逻辑单位，移动碰撞处理
/// </summary>
public partial class MainLogicUnit
{
    private PEVector3 inputDir;
    /// <summary>
    /// 输入方向
    /// </summary>
    public PEVector3 InputDir
    {
        private set
        {
            inputDir = value;
        } 
        get
        {
            return inputDir;
        }
    }
    /// <summary>
    /// 战斗单位物理碰撞器
    /// </summary>
    public PECylinderCollider collider;

    List<PEColliderBase> envColliLst;
    void InitMove()
    {
        LogicPos = ud.bornPos;
        moveSpeedBase = ud.unitCfg.moveSpeed;
        LogicMoveSpeed = ud.unitCfg.moveSpeed;
        envColliLst = BattleSys.Instance.GetEnvColliders();

        collider = new PECylinderCollider(ud.unitCfg.colliCfg)
        {
            mPos = LogicPos
        };
    }
    void TickMove()
    {
        PEVector3 moveDir = InputDir;
        collider.mPos += moveDir * LogicMoveSpeed * (PEInt)Configs.ClientLogicFrameDeltaSec;
        PEVector3 adj = PEVector3.zero;
        collider.CalcCollidersInteraction(envColliLst,ref moveDir,ref adj);
        if(LogicDir!=moveDir)
        {
            LogicDir = moveDir;
        }
        if(LogicDir!=PEVector3.zero)
        {
            LogicPos = collider.mPos + adj;
        }
        collider.mPos = LogicPos;
        //this.Log($"{unitName} pos:" + collider.mPos.ConvertViewVector3());
    }
    void UnInitMove()
    {
         
    } 
    /// <summary>
    /// 模拟输入方向
    /// </summary>
    /// <param name="dir"></param>
    public void InputFakeMoveKey(PEVector3 dir)
    {
        inputDir = dir;
    }
    /// <summary>
    /// 临时存储ui输入方向，以便于施放完技能恢复方向输入
    /// </summary>
    private PEVector3 UIInputDir;
    public void InputMoveKey(PEVector3 dir)
    {
        UIInputDir = dir;
        InputDir = dir;
        if(IsSkillSpelling()==false
            &&IsStunned()==false
            &&IsKnockup()==false){

            InputDir = dir;
        }
        //this.Log("InputDir:" + dir.ConvertViewVector3());
    }

    /// <summary>
    /// 恢复ui输入
    /// </summary>
    public void RecoverUIInput()
    {
        if(InputDir!=UIInputDir)
        {
            InputDir = UIInputDir;
        }
    }
    /// <summary>
    /// 是否能移动
    /// 未被眩晕/击飞
    /// 未在其他技能施法前摇阶段
    /// </summary>
    /// <returns></returns>
    public bool CanMove()
    {
        return IsStunned() == false 
            && IsKnockup() == false 
            && IsSkillSpelling() == false;
    }
}

