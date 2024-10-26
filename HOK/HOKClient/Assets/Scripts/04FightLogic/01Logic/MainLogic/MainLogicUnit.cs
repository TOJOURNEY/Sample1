using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HOKProtocol;
using PEMath;

/// <summary>
/// 单位状态
/// </summary>
public enum UnitStateEnum
{
    /// <summary>
    /// 存活
    /// </summary>
    Alive,
    /// <summary>
    /// 死亡
    /// </summary>
    Dead
}
/// <summary>
/// 角色，小兵
/// </summary>
public enum UnitTypeEnum
{
    Hero,
    Soldier,
    Tower,
}
/// <summary>
/// 队伍
/// </summary>
public enum TeamEnum
{
    None,
    Blue,
    Red,
    /// <summary>
    /// 野怪
    /// </summary>
    Neutal,

}
/// <summary>
/// 主要逻辑层(英雄，小兵，防御塔，水晶)
/// </summary>
public abstract partial class MainLogicUnit : LogicUnit
{
    public LogicUnitData ud;
    public UnitStateEnum unitState;
    public UnitTypeEnum unitType;

    protected string pathPrefix = "";
    public MainViewUnit mainViewUnit;
    public MainLogicUnit(LogicUnitData ud)
    {
        this.ud = ud;
        unitName = ud.unitCfg.unitName;

    }
    public override void LogicInit()
    {
        //初始化属性
        InitPropertier();
        //初始化技能
        InitSkill();
        //初始化移动
        InitMove();

        GameObject go = ResSvc.Instance.LoadPrefab(pathPrefix + "/" + ud.unitCfg.resName);
        mainViewUnit = go.GetComponent<MainViewUnit>(); 
        if(mainViewUnit==null)
        {
            this.Error("Get MainViewUnit null Error"+unitName);
        }
        mainViewUnit.Init(this);

        unitState = UnitStateEnum.Alive;

    }

    public override void LogicTick()
    {
        TickSkill();
        TickMove();
    }

    public override void LogicUnInit() 
    {
        //UnInitSkill();
        UnInitMove();
    }
    public void InputKey(OpKey key)
    {
        switch (key.keyType)
        {
            case KeyType.Skill:
                InputSkillKey(key.skillKey);
                break;
            case KeyType.Move:
                PEInt x = PEInt.zero;
                x.ScaledValue = key.moveKey.x;
                PEInt z = PEInt.zero;
                z.ScaledValue = key.moveKey.z;
                InputMoveKey(new PEVector3(x, 0, z));
                break;
            case KeyType.None:
            default:
                this.Error("KEY is not exist");
                break;
        }
    }
    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="audioName">音效名字</param>
    /// <param name="loop">是否循环</param>
    /// <param name="delay">延时多久</param>
    public void PlayAudio(string audioName,bool loop=false,int delay=0)
    {
        mainViewUnit.PlayAudio(audioName, loop, delay);
    }
    public void PlayAni(string aniName)
    {
        mainViewUnit.PlayAni(aniName);
    }

    /// <summary>
    /// 判断是否是自己受伤或对其造成伤害
    /// </summary>
    /// <returns></returns>
    public virtual bool IsPlayerSelf()
    {
        return false;
    }
    /// <summary>
    /// 用于子类重写，判定传入逻辑单位是否相等。
    /// </summary>
    /// <param name="logicAttack"></param>
    /// <returns></returns>
    public virtual bool Equals(MainLogicUnit logicAttack)
    {
        return false;
    }
}
