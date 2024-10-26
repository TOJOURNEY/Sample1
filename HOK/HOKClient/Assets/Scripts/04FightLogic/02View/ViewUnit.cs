using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public abstract class ViewUnit:MonoBehaviour
{
    //Pos
    public bool IsSyncPos;
    /// <summary>
    /// 是否预测(预测位置)
    /// </summary>
    public bool PredictPos;
    /// <summary>
    /// 最大预测帧
    /// </summary>
    public int PredictMaxCount;
    /// <summary>
    /// 是否平滑
    /// </summary>
    public bool SmoothPos;
    /// <summary>
    /// 平滑速度
    /// </summary>
    public float viewPosAccer;
    //Dir
    public bool IsSyncDir;
    /// <summary>
    /// 是否转向平滑
    /// </summary>
    public bool SmoothDir;
    /// <summary>
    /// 角度倍增器(改变朝向的加速度)
    /// </summary>
    public float AngleMultiplier;
    /// <summary>
    /// 角度变化加速
    /// </summary>
    public float viewDirAccer;

    public Transform RoationRoot;
    /// <summary>
    /// 当前预测次数
    /// </summary>
    int predictCount;
    /// <summary>
    /// 视图的位置
    /// </summary>
    protected Vector3 viewTargetPos;
    /// <summary>
    /// 视图目标朝向
    /// </summary>
    protected Vector3 viewTargetDir;
    
    LogicUnit logicUnit = null;

    public virtual void Init(LogicUnit logicUnit)
    {
        this.logicUnit = logicUnit;
        gameObject.name = logicUnit.unitName + "_" + gameObject.name;

        transform.position = logicUnit.LogicPos.ConvertViewVector3();
        if(RoationRoot==null)
        {
            RoationRoot = transform;
        }
        //RoationRoot.rotation = CalcRotation(logicUnit.LogicDir.ConvertViewVector3());
        RoationRoot.forward = logicUnit.LogicDir.ConvertViewVector3();

    }
    protected virtual void Update()
    {
        if(IsSyncDir)
        {
            UpdateDirection();
        }
        if(IsSyncPos)
        {
            UpdatePosition();
        }
    }
    void UpdateDirection()
    {
        //运动预测转向平滑移动
        if(logicUnit.isDirChanged)
        {
            viewTargetDir = GetUnitViewDir();
            logicUnit.isDirChanged = false;
        }
        if(SmoothDir)
        {
            //计算角度插值
            float threshold = Time.deltaTime * viewDirAccer;
            //计算当前角度改变的值
            float angle = Vector3.Angle(RoationRoot.forward, viewTargetDir);
            //计算需要变化的值
            float angleMult = (angle / 180) * AngleMultiplier*Time.deltaTime;
            if(viewTargetDir!=Vector3.zero)
            {
                Vector3 interDir = Vector3.Lerp(RoationRoot.forward, viewTargetDir, threshold + angleMult);
                //RoationRoot.rotation = CalcRotation(interDir);
                RoationRoot.forward = interDir;
            }
        }
        else
        {
            RoationRoot.forward = viewTargetDir;
            //RoationRoot.rotation = CalcRotation(viewTargetDir);
        }
    }
    /// <summary>
    /// 运动预测与平滑移动
    /// </summary>
    void UpdatePosition()
    {
        ///运动平滑
        if(PredictPos)
        {
            ///运动预测
            if(logicUnit.isPosChanged)
            {
                ///逻辑有Tick，目标位置更新到最新
                viewTargetPos = logicUnit.LogicPos.ConvertViewVector3();
                logicUnit.isPosChanged = false;
                predictCount = 0;
            }
            else
            {
                if(predictCount>PredictMaxCount)
                {
                    return;
                }
                ///逻辑未Tick，使用预测计算
                float delta = Time.deltaTime;
                ///预测位置 = 逻辑速度*逻辑方向
                var predictPos = delta * logicUnit.LogicMoveSpeed.RawFloat * logicUnit.LogicDir.ConvertViewVector3();
                ///新的目标位置 = 表现目标位置 + 预测位置
                viewTargetPos += predictPos;
                ++predictCount;
            }
            ///平滑移动
            if(SmoothPos)
            {
                ///做一个线性插值(在当前的位置以及预测的位置)根据当前的time.deltaTime*一个加速度
                transform.position = Vector3.Lerp(transform.position, viewTargetPos, Time.deltaTime * viewPosAccer);
            } 
            else
            {
                transform.position = viewTargetPos;
            }
        }
        else
        {
            //无平滑无预测，强制每帧刷新逻辑层的位置
            ForcePosSync();
        }
        
    }
    public void ForcePosSync()
    {
        transform.position = logicUnit.LogicPos.ConvertViewVector3();
    }

    protected virtual Vector3 GetUnitViewDir()
    {
        return logicUnit.LogicDir.ConvertViewVector3();
    }
    protected Quaternion CalcRotation(Vector3 targetDir)
    {
        return Quaternion.FromToRotation(Vector3.forward, targetDir);
    }
    /// <summary>
    /// 动画播放
    /// </summary>
    /// <param name="aniName">动画名字</param>
    public virtual void PlayAni(string aniName) { }
    /// <summary>
    /// 音效播放
    /// </summary>
    /// <param name="audName">音效名字</param>
    /// <param name="loop">是否循环</param>
    /// <param name="delay">(默认为0 不延时)延时多久</param>
    public virtual void PlayAudio(string audName,bool loop=false,int delay=0) {
        AudioSvc.Instance.PlayEntityAudio(audName,GetComponent<AudioSource>(),loop,delay);
    }
}

