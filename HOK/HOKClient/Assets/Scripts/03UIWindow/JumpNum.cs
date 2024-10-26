using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 飘字类型
/// </summary>
public enum JumpTypeEnum
{
    None,
    /// <summary>
    /// 技能伤害
    /// </summary>
    SkillDamage,
    /// <summary>
    /// Buff伤害
    /// </summary>
    BuffDamage,
    /// <summary>
    /// 治疗
    /// </summary>
    Cure,
    /// <summary>
    /// 减速
    /// </summary> 
    SlowSpeed,
}
/// <summary>
/// 飘字动画类型
/// </summary>
public enum JumpAniEnum
{
    None,
    /// <summary>
    /// 左面飘字动画
    /// </summary>
    LeftCurve,
    /// <summary>
    /// 右面飘字动画
    /// </summary>
    RightCurve,
    /// <summary>
    /// 中间飘字动画
    /// </summary>
    CenterUp
}
/// <summary>
/// 更新飘字信息
/// </summary>
public class JumpUpdateInfo 
{
    public int jumpVal;
    public Vector2 pos;
    public JumpTypeEnum jumpType;
    public JumpAniEnum jumpAni;
}
/// <summary>
/// 伤害飘字
/// </summary>
public class JumpNum:MonoBehaviour
{
    public RectTransform rect;
    public Animator ani;
    public Text txt;

    public int MaxFont;
    public int MinFont;
    /// <summary>
    /// 计算阈值
    /// </summary>
    public int MaxFontValue;
    public Color skillDamageColor;
    public Color buffDamageColor;
    public Color cureDamageColor;
    public Color slowSpeedDamageColor;

    JumpNumPool ownerPool;
    public void Init(JumpNumPool ownerPool)
    {
        this.ownerPool = ownerPool;
    }

    public void Show(JumpUpdateInfo ji)
    {
        int fontSize = (int)Mathf.Clamp(ji.jumpVal * 1.0f / MaxFontValue, MinFont, MaxFont);
        txt.fontSize = fontSize;
        rect.anchoredPosition = ji.pos;

        switch (ji.jumpType)
        {
            case JumpTypeEnum.SkillDamage:
                txt.text = ji.jumpVal.ToString();
                txt.color = skillDamageColor;
                break;
            case JumpTypeEnum.BuffDamage:
                txt.text = ji.jumpVal.ToString();
                txt.color = buffDamageColor;
                break;
            case JumpTypeEnum.Cure:
                txt.text = "+" + ji.jumpVal;
                txt.color = cureDamageColor;
                break;
            case JumpTypeEnum.SlowSpeed:
                txt.text = "减速";
                txt.color = slowSpeedDamageColor;
                break;
            case JumpTypeEnum.None:
            default:
                break;
        }

        switch (ji.jumpAni)
        {
            case JumpAniEnum.LeftCurve:
                ani.Play("JumpLeft", 0);
                break;
            case JumpAniEnum.RightCurve:
                ani.Play("JumpRight", 0);
                break;
            case JumpAniEnum.CenterUp:
                ani.Play("JumpCenter", 0);
                break;
            case JumpAniEnum.None:
            default:
                break;
        }

        StartCoroutine(Recycle());
    }
    /// <summary>
    /// 回收
    /// </summary>
    /// <returns></returns>
    IEnumerator Recycle()
    {
        yield return new WaitForSeconds(0.75f);
        ani.Play("Empty");
        ownerPool.PushOne(this);
    }


}
/// <summary>
/// 对象池
/// </summary>
public class JumpNumPool
{
    Transform poolRoot;
    private Queue<JumpNum> jumpNumQue;
    public JumpNumPool(int count,Transform poolRoot)
    {
        this.poolRoot = poolRoot;
        jumpNumQue = new Queue<JumpNum>();

        for (int i = 0; i < count; i++)
        {
            PushOne( CreateOne());
        }
    }
    int index = 0;
    int Index
    {
        get {
            return ++index;
        }
    }
    /// <summary>
    /// 初始化对象池
    /// </summary>
    JumpNum CreateOne()
    {
        GameObject go = ResSvc.Instance.LoadPrefab("UIPrefab/DynamicItem/JumpNum");
        go.name = "JumpNum_" + Index;
        go.transform.SetParent(poolRoot);
        go.transform.localPosition = Vector3.zero;
        go.transform.localScale = Vector3.one;
        JumpNum jn = go.GetComponent<JumpNum>();
        jn.Init(this);
        return jn;
    }
    /// <summary>
    /// 取出，超过数量则重置
    /// </summary>
    /// <returns></returns>
    public JumpNum PopOne()
    {
        if (jumpNumQue.Count > 0)
        {
            return jumpNumQue.Dequeue();
        }
        else
        {
            this.Warn("飘字超额，动态调整上限");
            PushOne(CreateOne());
            return PopOne();
        }
    }
    /// <summary>
    /// 添加进对象池
    /// </summary>
    /// <param name="jn"></param>
    public void PushOne(JumpNum jn)
    {
        jumpNumQue.Enqueue(jn);
    }
}
