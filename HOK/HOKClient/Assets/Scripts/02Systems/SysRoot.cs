using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 业务系统基类
/// </summary>
public class SysRoot: MonoBehaviour
{
    /// <summary>
    /// Root节点
    /// </summary>
    protected GameRoot root;
    /// <summary>
    /// 网络管理服务
    /// </summary>
    protected NetSvc netSvc;
    /// <summary>
    /// 资源管理服务
    /// </summary>
    protected ResSvc resSvc;
    /// <summary>
    /// 音频管理服务
    /// </summary>
    protected AudioSvc audioSvc;
    /// <summary>
    /// 初始化
    /// </summary>
    public virtual void InitSys()
    {
        root = GameRoot.Instance;
        netSvc = NetSvc.Instance;
        resSvc = ResSvc.Instance;
        audioSvc = AudioSvc.Instance;
    }
}

