using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 血条显示Item
/// </summary>
public abstract class ItemHP:WindowRoot
{
    public RectTransform rect;
    public Image imgPrg;
    /// <summary>
    /// 是否是自己队伍
    /// </summary>
    protected bool isFriend;
    Transform rootTrans;
    int hpVal;
    public virtual void InitItem(MainLogicUnit unit,Transform root,int hp)
    {
        TeamEnum selfTeam;
        if(BattleSys.Instance.GetCurrentUserTeam()==TeamEnum.Blue)
        {
            selfTeam = TeamEnum.Blue;
        }
        else
        {
            selfTeam = TeamEnum.Red;
        }
        isFriend = unit.IsTeam(selfTeam);

        imgPrg.fillAmount = 1;
        rootTrans = root;
        hpVal = hp;
    }
    /// <summary>
    /// 更新血条进度显示
    /// </summary>
    /// <param name="newVal">新的值</param>
    public void UpdateHPPrg(int newVal)
    {
        if(newVal==0)
        {
            SetActive(gameObject, false);
        }
        else
        {
            SetActive(gameObject);
        }
        imgPrg.fillAmount = newVal * 1.0f / hpVal;
    }
    /// <summary>
    /// 负面状态显示
    /// </summary>
    /// <param name="state">状态类型</param>
    /// <param name="show">是否显示</param>
    public virtual void SetStateInfo(StateEnum state,bool show)
    {

    }
    class JumpPack
    {
        public JumpNum jn;
        public JumpUpdateInfo jui;
    }
    Queue<JumpPack> jpq = new Queue<JumpPack>();
    public void AddHPJumpNum(JumpNum jn, JumpUpdateInfo jui)
    {
        jpq.Enqueue(new JumpPack { jn = jn, jui = jui });
    }
    
    public float JumpNumInterval;
    bool canShow = true;
    float delayTimeCounter = 0;
    /// <summary>
    /// 血条跟随
    /// </summary>
    public void UpdateCheck()
    {
        if (jpq.Count > 0 && canShow)
        {
            JumpPack jp = jpq.Dequeue();
            jp.jn.Show(jp.jui);

            canShow = false;
        }

        if (canShow == false)
        {
            delayTimeCounter += Time.deltaTime;
            if (delayTimeCounter > JumpNumInterval / 1000)
            {
                delayTimeCounter = 0;
                canShow = true;
            }
        }

        if (rootTrans)
        {
            float scaleRate = 1.0F * ClientConfig.ScreenStandardHeight / Screen.height;
            Vector3 screenPos = Camera.main.WorldToScreenPoint(rootTrans.position);
            rect.anchoredPosition = screenPos * scaleRate;
        }
    }

}
/// <summary>
/// 负面状态
/// </summary>
public enum StateEnum
{
    None,
    /// <summary>
    /// 沉默
    /// </summary>
    Silenced,
    /// <summary>
    /// 击飞
    /// </summary>
    Knockup,
    /// <summary>
    /// 眩晕
    /// </summary>
    Stunned,

    //TODO
    
    /// <summary>
    /// 无敌
    /// </summary>
    Invincible,
    /// <summary>
    /// 禁锢
    /// </summary>
    Restricted,
}