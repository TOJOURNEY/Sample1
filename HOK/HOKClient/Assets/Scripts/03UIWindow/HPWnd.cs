using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 血条及伤害数值显示
/// </summary>
public class HPWnd : WindowRoot
{
    /// <summary>
    /// 血条
    /// </summary>
    public Transform hpItemRoot;
    /// <summary>
    /// 伤害数值
    /// </summary>
    public Transform JumpNumRoot;
    /// <summary>
    /// 缓存数量
    /// </summary>
    public int jumpNumCount;
    JumpNumPool pool;
    private Dictionary<MainLogicUnit, ItemHP> itemDic;
    protected override void InitWnd()
    {
        base.InitWnd();

        itemDic = new Dictionary<MainLogicUnit, ItemHP>();
        pool = new JumpNumPool(jumpNumCount,JumpNumRoot);
    }
    /// <summary>
    /// 添加血条预制体
    /// </summary>
    /// <param name="unit">单位</param>
    /// <param name="trans">位置</param>
    /// <param name="hp">血条</param>
    public void AddHPItemInfo(MainLogicUnit unit,Transform trans,int hp)
    {
        if(itemDic.ContainsKey(unit))
        {
            this.Error(unit.unitName + "hp item is already exist.");
        }
        else
        {
            //判断单位类型
            string path = GetItemPath(unit.unitType);
            GameObject go = resSvc.LoadPrefab(path,true);
            go.transform.SetParent(hpItemRoot);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;

            ItemHP ih = go.GetComponent<ItemHP>();
            ih.InitItem(unit, trans, hp);
            itemDic.Add(unit,ih);
        }
    }

    private void Update()
    {
        foreach (var item in itemDic)
        {
            item.Value.UpdateCheck();
        }
    }
    /// <summary>
    /// 预制体路径
    /// </summary>
    /// <param name="unitType">路径名字</param>
    /// <returns></returns>
    string GetItemPath(UnitTypeEnum unitType)
    {
        string path = "";
        switch (unitType)
        {
            case UnitTypeEnum.Hero:
                path = "UIPrefab/DynamicItem/ItemHPHero";
                break;
            case UnitTypeEnum.Soldier:
                path = "UIPrefab/DynamicItem/ItemHPSoldier";
                break;
            case UnitTypeEnum.Tower:
                path = "UIPrefab/DynamicItem/ItemHPTower";
                break;
            default:
                break;
        }
        return path;
    }
    /// <summary>
    /// 设置血条值(刷新)
    /// </summary>
    /// <param name="key">单位</param>
    /// <param name="hp"></param>
    public void SetHPVal(MainLogicUnit key, int hp, JumpUpdateInfo jui)
    {
        if (itemDic.TryGetValue(key, out ItemHP item))
        {
            item.UpdateHPPrg(hp);
        }

        if (jui != null)
        {
            JumpNum jn = pool.PopOne();
            item.AddHPJumpNum(jn, jui);
            //if(jn != null) {
            //    jn.Show(jui);
            //}
        }
    }
    public void SetJumpUpdateInfo(JumpUpdateInfo jui)
    {
        if (jui != null)
        {
            JumpNum jn = pool.PopOne();
            if (jn != null)
            {
                jn.Show(jui);
            }
        }
    }
    public void SetStateInfo(MainLogicUnit key,StateEnum state,bool show =true)
    {
        if(itemDic.TryGetValue(key,out ItemHP item))
        {
            item.SetStateInfo(state, show);
        }
    }
    /// <summary>
    /// 删除血条
    /// </summary>
    /// <param name="key"></param>
    public void RmvHPItemInfo(MainLogicUnit key)
    {
        if(itemDic.TryGetValue(key,out ItemHP item))
        {
            Destroy(item.gameObject);
            itemDic.Remove(key);
        }
    }
    /// <summary>
    /// 窗口关闭时清理血条
    /// </summary>
    protected override void UninitWnd()
    {
        base.UninitWnd();

        for (int i = hpItemRoot.childCount - 1; i >= 0; --i)
        {
            Destroy(hpItemRoot.GetChild(i).gameObject);
        }
        for (int i = JumpNumRoot.childCount - 1; i >= 0; --i)
        {
            Destroy(JumpNumRoot.GetChild(i).gameObject);
        }

        if (itemDic != null)
        {
            itemDic.Clear();
        }
    }
}
