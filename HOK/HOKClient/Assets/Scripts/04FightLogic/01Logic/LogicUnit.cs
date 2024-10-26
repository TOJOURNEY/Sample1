using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEMath;
using System;

/// <summary>
/// 基础逻辑层
/// </summary>
public abstract class LogicUnit : ILogic
{
    /// <summary>
    /// 产生ui输入时
    /// </summary>
    public Action OnDirChange;
    /// <summary>
    /// 逻辑单位角色名称
    /// </summary>
    public string unitName;

    #region Key Properties
    /// <summary>
    /// 是否发生变更(逻辑位置)
    /// </summary>
    public bool isPosChanged = false;
    PEVector3 logicPos;
    /// <summary>
    /// 逻辑位置
    /// </summary>
    public PEVector3 LogicPos
    {
        set
        {
            logicPos = value;
            isPosChanged = true;
        }
        get
        {
            return logicPos;
        }
    }
    /// <summary>
    /// 是否发送改变(逻辑方向)
    /// </summary>
    public bool isDirChanged = false;
    PEVector3 logicDir;
    /// <summary>
    /// 逻辑方向
    /// </summary>
    public PEVector3 LogicDir
    {
        set
        {
            logicDir = value;
            isDirChanged = true;
            OnDirChange?.Invoke();
        }
        get
        {
            return logicDir;
        }
    }
    PEInt logicMoveSpeed;
    /// <summary>
    /// 逻辑速度
    /// </summary>
    public PEInt LogicMoveSpeed
    {
        set
        {
            logicMoveSpeed = value;
        }
        get
        {
            return logicMoveSpeed;
        }
    }
    /// <summary>
    /// 基础速度
    /// </summary>
    public PEInt moveSpeedBase; 
    #endregion
    public abstract void LogicInit();
    
    public abstract void LogicTick();

    public abstract void LogicUnInit();
}
interface ILogic
{
    void LogicInit();
    void LogicTick();
    void LogicUnInit();
}
