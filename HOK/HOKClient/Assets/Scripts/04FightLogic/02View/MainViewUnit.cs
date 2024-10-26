using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using PEMath;

/// <summary>
/// 主要表现控制,攻速/移速动画变化
/// 技能动画播放
/// 血条信息显示
/// 小地图显示
/// </summary>
public abstract class MainViewUnit:ViewUnit
{
    public Transform skillRange;
    /// <summary>
    /// 血条定位点
    /// </summary>
    public Transform hpRoot;
    /// <summary>
    /// 动画渐变
    /// </summary>
    public float fade;
    public Animation ani;

    float aniMoveSpeedBase;
    float aniAttackSpeedBase;

    HPWnd hpWnd;
    PlayWnd playWnd;

    MainLogicUnit mainLoogicUnit = null;

    public override void Init(LogicUnit logicUnit)
    {
        base.Init(logicUnit);

        mainLoogicUnit = logicUnit as MainLogicUnit;

        //移速
        aniMoveSpeedBase = mainLoogicUnit.LogicMoveSpeed.RawFloat;
        aniAttackSpeedBase = mainLoogicUnit.AttackSpeedRate.RawFloat;


        //血条显示
        hpWnd = BattleSys.Instance.hpWnd;
        hpWnd.AddHPItemInfo(mainLoogicUnit, hpRoot,mainLoogicUnit.Hp.RawInt);

        playWnd = BattleSys.Instance.playWnd;
        playWnd.AddMiniIconItemInfo(mainLoogicUnit);

        mainLoogicUnit.OnHPChange += UpdateHp;
        mainLoogicUnit.OnStateChange += UpdateState;
        mainLoogicUnit.OnSlowDown += UpdateJui;
    }

    protected override void Update()
    {
        if (mainLoogicUnit.isDirChanged)
        {
            if(mainLoogicUnit.LogicDir.ConvertViewVector3().Equals(Vector3.zero))
            {
                PlayAni("free");
            }
            else
            {
                PlayAni("walk");
            }
        }


        base.Update();
    }
    private void OnDestroy()
    {
        mainLoogicUnit.OnHPChange -= UpdateHp;
    }
    public virtual void OnDeath(MainLogicUnit unit)
    {

    }
    public override void PlayAni(string aniName)
    {
        //this.Log("Play ani:" + aniName);
        if (aniName == "atk")
        { 
            aniName = "atk" + UnityEngine.Random.Range(1, 3);
        }

        if (aniName.Contains("walk"))
        {
            float moveRate = mainLoogicUnit.LogicMoveSpeed.RawFloat / aniMoveSpeedBase;
            ani[aniName].speed = moveRate;
            ani.CrossFade(aniName, fade / moveRate);
            //this.Log($"fade:{fade} moveRate{moveRate}");
        }
        else if (aniName.Contains("atk"))
        {
            if (ani.IsPlaying(aniName))
            {
                ani.Stop();
            }
            float attackRate = mainLoogicUnit.AttackSpeedRate.RawFloat / aniAttackSpeedBase;
            ani[aniName].speed = attackRate;
            ani.CrossFade(aniName, fade / attackRate);
            //this.Log($"fade:{fade} attackRate{attackRate}");
        }
        else
        {
            if (ani == null)
            {
                this.Log("ani is null");
            }
            ani.CrossFade(aniName, fade);
            //this.Log($"fade:{fade}");
        }
    }

    public void UpdateHp(int hp,JumpUpdateInfo jui)
    {
        if (jui != null)
        {
            float scaleRate = 1.0f * ClientConfig.ScreenStandardHeight / Screen.height;
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 1, 0));
            jui.pos = screenPos * scaleRate;
        }
        hpWnd.SetHPVal(mainLoogicUnit, hp, jui);

    }
    public void UpdateJui(JumpUpdateInfo jui)
    {
        if(jui!=null)
        {
            float scaleRate = 1.0f * ClientConfig.ScreenStandardHeight / Screen.height;
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 1, 0));
            jui.pos = screenPos * scaleRate;
        }
        hpWnd.SetJumpUpdateInfo(jui);
    }
    public void UpdateState(StateEnum state,bool show)
    {
        if(state==StateEnum.Knockup
            ||state==StateEnum.Stunned
            ||state==StateEnum.Silenced){
            if(mainLoogicUnit.IsPlayerSelf()&&show)
            {
                playWnd.SetForbidState();
            }
        }
        hpWnd.SetStateInfo(mainLoogicUnit, state, show);
    }
    public void UpdateSkillRotation(PEVector3 skillRotation)
    {
        viewTargetDir = skillRotation.ConvertViewVector3();
    }
    public void SetAtkSkillRange(bool state, float range = 2.5f)
    {
        if (skillRange != null)
        {
            range += mainLoogicUnit.ud.unitCfg.colliCfg.mRadius.RawFloat;
            skillRange.localScale = new Vector3(range / 2.5f, range / 2.5f, 1);
            skillRange.gameObject.SetActive(state); 
        }
    }
    public void SetBuffFollower(BuffView buffView)
    {
        buffView.transform.SetParent(transform);
        buffView.transform.localPosition = Vector3.zero;
        buffView.transform.localScale = Vector3.one;
    }
    public void RmvUIItemInfo()
    {
        hpWnd.RmvHPItemInfo(mainLoogicUnit);
        playWnd.RmvMapIconItemInfo(mainLoogicUnit);
    }
    public void SetImgInfo(int cdTime)
    {
        if (mainLoogicUnit.IsPlayerSelf())
        {
            playWnd.SetImgInfo(cdTime);
        }
    }
}

