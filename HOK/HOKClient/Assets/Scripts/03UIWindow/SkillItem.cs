using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 技能Button
/// </summary>
public class SkillItem:WindowRoot
{
    public Image imgCycle;
    public Image skillIcon;
    public Image imgCD;
    public Text txtCD;
    public Image imgPoint;
    public Image imgForbid;
    public Transform EffectRoot;

    HeroView viewHero;

    int skillIndex;
    SkillCfg skillCfg;
    float pointDis;
    Vector2 startPos = Vector2.zero;

    public void InitSkillItem(SkillCfg skillCfg, int skillIndex)
    {
        SetActive(EffectRoot, false);
        InitWnd();

        viewHero = BattleSys.Instance.GetSelfHero().mainViewUnit as HeroView;

        this.skillIndex = skillIndex;
        this.skillCfg = skillCfg;

        pointDis = Screen.height * 1.0f / ClientConfig.ScreenStandardHeight * ClientConfig.SkillOPDis;
        if (skillCfg.isNormalAttack == false)
        {
            SetSprite(skillIcon, "ResImages/PlayWnd/" + skillCfg.iconName);
            SetActive(imgCD, false);
            SetActive(txtCD, false);

            OnClickDown(skillIcon.gameObject, (evt, args) => {
                startPos = evt.position;
                SetActive(imgCycle);
                SetActive(imgPoint);
                ShowSkillAtkRange(true);


                if (skillCfg.releaseMode==ReleaseModeEnum.Postion)
                {
                    //通知场景中显示skill guide
                    viewHero.SetSkillGuide(skillIndex, true, ReleaseModeEnum.Postion, Vector3.zero);
                }
                else if(skillCfg.releaseMode==ReleaseModeEnum.Direction)
                {
                    viewHero.SetSkillGuide(skillIndex, true, ReleaseModeEnum.Direction, Vector3.zero);
                }

            });
            OnDrag(skillIcon.gameObject, (evt, args) =>
            {
                Vector2 dir = evt.position - startPos;
                float len = dir.magnitude;
                if(len>pointDis)
                {
                    Vector2 clampDir = Vector2.ClampMagnitude(dir, pointDis);
                    imgPoint.transform.position = startPos + clampDir;
                }
                else
                {
                    imgPoint.transform.position = evt.position;
                }
                if(skillCfg.releaseMode==ReleaseModeEnum.Postion)
                {
                    if(dir==Vector2.zero)
                    {
                        return;
                    }
                    dir = BattleSys.Instance.SkillDisMultipler * dir;
                    Vector2 clampDIr = Vector2.ClampMagnitude(dir, skillCfg.targetCfg.selectRange);
                    Vector3 clampDirVector3 = new Vector3(clampDIr.x, 0, clampDIr.y);
                    clampDirVector3 = Quaternion.Euler(0, 45, 0) * clampDirVector3;
                    viewHero.SetSkillGuide(skillIndex, true,ReleaseModeEnum.Postion,clampDirVector3);
                }
                else if(skillCfg.releaseMode==ReleaseModeEnum.Direction)
                {
                    Vector3 dirVecrot3 = new Vector3(dir.x, 0, dir.y);
                    dirVecrot3 = Quaternion.Euler(0, 45, 0) * dirVecrot3;
                    viewHero.SetSkillGuide(skillIndex, true, ReleaseModeEnum.Direction, dirVecrot3.normalized);
                }
                else
                {
                    this.Warn(skillCfg.releaseMode.ToString());
                }
                if(len>=ClientConfig.SkillCancelDis)
                {
                    SetActive(BattleSys.Instance.playWnd.imgCancelSkill);
                }
                else
                {
                    SetActive(BattleSys.Instance.playWnd.imgCancelSkill,false);

                }
            });
            OnClickUp(skillIcon.gameObject, (evt, args) =>
            {
                Vector2 dir = evt.position - startPos;
                imgPoint.transform.position = transform.position;
                SetActive(imgCycle, false);
                SetActive(imgPoint, false);

                SetActive(BattleSys.Instance.playWnd.imgCancelSkill, false);
                ShowSkillAtkRange(false);

                if (dir.magnitude >= ClientConfig.SkillCancelDis)
                {
                    this.Log("取消技能施放");
                    viewHero.DisableSkillGuide(skillIndex);
                    return;
                }
                if(skillCfg.releaseMode==ReleaseModeEnum.Click)
                {
                    this.Log("直接施放技能");
                    ClickSkillItem();
                }
                else if(skillCfg.releaseMode==ReleaseModeEnum.Postion)
                {
                    dir = BattleSys.Instance.SkillDisMultipler * dir;
                    Vector2 clampDIr = Vector2.ClampMagnitude(dir, skillCfg.targetCfg.selectRange);
                    this.Log("Pos Info:" + clampDIr.ToString());
                    viewHero.DisableSkillGuide(skillIndex);
                    Vector3 clampDirVector3 = new Vector3(clampDIr.x, 0, clampDIr.y);
                    clampDirVector3 = Quaternion.Euler(0, 45, 0) * clampDirVector3;
                    ClickSkillItem(clampDirVector3);
                }
                else if (skillCfg.releaseMode == ReleaseModeEnum.Direction)
                {
                    viewHero.DisableSkillGuide(skillIndex);

                    if (dir==Vector2.zero)
                    {
                        return;
                    }
                    Vector3 dirVecrot3 = new Vector3(dir.x, 0, dir.y);
                    dirVecrot3 = Quaternion.Euler(0, 45, 0) * dirVecrot3;
                    ClickSkillItem(dirVecrot3.normalized);

                }
                else
                {
                    this.Warn("skill release mode not exist");
                }
                ShowEffect();
            });
        }
        else
        {
            //普通攻击
            OnClickDown(skillIcon.gameObject, (evt, args) =>
            {
                ShowSkillAtkRange(true);
                ClickSkillItem();
            });

            OnClickUp(skillIcon.gameObject, (evt, args) =>
            {
                ShowEffect();
                ShowSkillAtkRange(false);
            });
        }
    }
    Coroutine ct = null;
    void ShowEffect()
    {
        if (ct != null)
        {
            StopCoroutine(ct);
            SetActive(EffectRoot, false);
        }
        SetActive(EffectRoot);
        if (gameObject.activeInHierarchy)
        {
            ct = StartCoroutine(DisableEffect());
        }
    }
    IEnumerator DisableEffect()
    {
        yield return new WaitForSeconds(0.5f);
        SetActive(EffectRoot, false);
    }
    private void ShowSkillAtkRange(bool state)
    {
        if(skillCfg.targetCfg!=null)
        {
            viewHero.SetAtkSkillRange(state, skillCfg.targetCfg.selectRange);
        }
    }
    public bool CheckSkillID(int skillID)
    {
        return skillCfg.skillID == skillID;
    }
    /// <summary>
    /// 计时器冷缩cd
    /// </summary>
    public void EnterCDState(int cdTime)
    {
        int sec = cdTime / 1000;
        int ms = cdTime % 1000;
        CreateMonoTimer(
            (loopCount) =>
            {
                SetText(txtCD, sec - loopCount);
            },
            1000,
            sec,
            (isDelay, looPrg, allPrg) =>
            {
                imgCD.fillAmount = 1 - allPrg;
            },
            () =>
            {
                SetActive(imgCD, false);
                SetActive(txtCD, false);
                AudioSvc.Instance.PlayUIAudio(NameDefine.BattleCDAudio);
                ShowEffect();
                skillIcon.raycastTarget = true;
            },
            ms);

        SetActive(imgCD);
        SetActive(txtCD);
        SetText(txtCD, sec);
        skillIcon.raycastTarget = false;

    }
    /// <summary>
    /// 静止状态显示
    /// </summary>
    public void ClickSkillItem(Vector3 vec)
    {
        BattleSys.Instance.SendSkillKey(skillCfg.skillID, vec);
    }
    public void ClickSkillItem()
    {
        BattleSys.Instance.SendSkillKey(skillCfg.skillID, Vector2.zero);
    }
     
    public void SetForbidState(bool state)
    {
        SetActive(imgForbid, state);
    }

}

