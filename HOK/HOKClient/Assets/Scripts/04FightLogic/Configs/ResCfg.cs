using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEMath;
using PEPhysx;


public class SoldierData : LogicUnitData
{
    public int soldierID;
    public int waveIndex;
    public int orderIndex;
    public string soldierName;
}

public class TowerData : LogicUnitData
{
    public int towerID;
    public int towerIndex;
    public string towerName;
}
public class HeroData : LogicUnitData
{
    public int heroID;
    public int posIndex;
    public string userName;

}
public class LogicUnitData
{
    /// <summary>
    /// 队伍
    /// </summary>
    public TeamEnum teamEnum;
    /// <summary>
    /// 出生位置
    /// </summary>
    public PEVector3 bornPos;
    public UnitCfg unitCfg;
}
public class UnitCfg
{
    public int unitID;
    /// <summary>
    /// 单位角色名字
    /// </summary>
    public string unitName;
    /// <summary>
    /// 资源名字
    /// </summary>
    public string resName;
    /// <summary>
    /// 命中高度
    /// </summary>
    public PEInt hitHeight;
    //核心属性
    /// <summary>
    /// 血量
    /// </summary>
    public int hp;
    /// <summary>
    /// 防御
    /// </summary>
    public int def;
    /// <summary>
    /// 移速
    /// </summary>
    public int moveSpeed;
    /// <summary>
    /// 碰撞体
    /// </summary>
    public ColliderConfig colliCfg;
    /// <summary>
    /// 技能ID数组
    /// </summary>
    public int[] skillArr;
    /// <summary>
    /// 被动技能数组
    /// </summary>
    public int[] pasvBuff;

}

/// <summary>
/// 地图配置
/// </summary>
public class MapCfg
{
    /// <summary>
    /// 地图ID
    /// </summary>
    public int mapID;
    /// <summary>
    /// 蓝色方玩家出生位置
    /// </summary>
    public PEVector3 blueBorn;
    /// <summary>
    /// 红色方玩家出生位置
    /// </summary>
    public PEVector3 redBorn;

    public int[] towerIDArr;
    public PEVector3[] towerPosArr;
    /// <summary>
    /// 小兵出生延时
    /// </summary>
    public int bornDelay;
    /// <summary>
    /// 小兵间隔，每波次间隔时间
    /// </summary>
    public int bornInterval;
    /// <summary>
    /// 每一个小兵距离间隔
    /// </summary>
    public int waveInterval;

    public int[] blueSoldierIDArr;
    public PEVector3[] blueSoldierPosArr;
    public int[] redSoldierIDArr;
    public PEVector3[] redSoldierPosArr;
}


