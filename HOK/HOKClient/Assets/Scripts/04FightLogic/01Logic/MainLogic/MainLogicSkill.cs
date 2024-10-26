using HOKProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEMath;

/// <summary>
/// 主要逻辑单位(技能处理)
/// </summary>
public partial class MainLogicUnit
{
    protected Skill[] skillArr;

    List<LogicTimer> timerLst;

    List<Buff> buffLst;
    void InitSkill()
    {
        int len = ud.unitCfg.skillArr.Length;
        skillArr = new Skill[len];
        for (int i = 0; i < len; i++)
        {
            skillArr[i] = new Skill(ud.unitCfg.skillArr[i], this);
        }

        timerLst = new List<LogicTimer>();
        buffLst = new List<Buff>();

        //被动buff
        int[] pasvBuffArr = ud.unitCfg.pasvBuff;
        if (pasvBuffArr != null)
        {
            for (int i = 0; i < pasvBuffArr.Length; i++)
            {
                CreateSkillBuff(this, null, pasvBuffArr[i], null);
            }
        }

        OnDirChange += ClearFreeAniCallBack;

    }
    void TickSkill()
    {
        //Buff Tick
        for (int i = buffLst.Count - 1; i >= 0; --i)
        {
            if (buffLst[i].unitState == SubUnitState.None)
            {
                buffLst[i].LogicUnInit();
                buffLst.RemoveAt(i);
            }
            else
            {
                buffLst[i].LogicTick();
            }
        }

        //timer tick
        for (int i = timerLst.Count - 1; i >= 0; --i)
        {
            LogicTimer timer = timerLst[i];
            if (timer.IsActive)
            {
                timer.TickTimer();
            }
            else
            {
                timerLst.RemoveAt(i);
            }
        }
    }
    void InputSkillKey(SkillKey key)
    {
        for (int i = 0; i < skillArr.Length; i++)
        {
            if(skillArr[i].skillID==key.skillID)
            { 
                PEInt x = PEInt.zero;
                PEInt z = PEInt.zero;
                x.ScaledValue = key.x_value;
                z.ScaledValue = key.z_value;
                PEVector3 skillArgs = new PEVector3(x, 0, z);
                skillArr[i].ReleaseSkill(skillArgs);
                return;
            }
        }
        this.Error($"skillID:{key.skillID} is not exist.");
    }
    public void CreateLogicTimer(Action cb, PEInt waitTime)
    {
        LogicTimer timer = new LogicTimer(cb, waitTime);
        timerLst.Add(timer);
    }

    public Buff CreateSkillBuff(MainLogicUnit source, Skill skill, int buffID, object[] args = null)
    {
        Buff buff = ResSvc.Instance.CreateBuff(source, this, skill, buffID, args);
        buff.LogicInit();
        buffLst.Add(buff);
        return null;
    }
    public Bullet CreateSkillBullet(MainLogicUnit source, MainLogicUnit target, Skill skill)
    {
        Bullet bullet = ResSvc.Instance.CreateBullet(source, target, skill);
        bullet.LogicInit();
        BattleSys.Instance.AddBullet(bullet);
        return bullet;
    }

    #region  API Functios

    public Skill GetSkillByID(int skillID)
    {
        for (int i = 0; i < skillArr.Length; i++)
        {
            if (skillArr[i].skillID == skillID)
            {
                return skillArr[i];
            }
        }
        this.Error($"SkillID:{skillID} is not exist");
        return null;
    }
    /// <summary>
    /// 获取普攻
    /// </summary>
    /// <returns></returns>
    public Skill GetNormalSkill()
    {
        if(skillArr!=null&&skillArr[0]!=null)
        {
            return skillArr[0];
        }
        return null;
    }
    public Skill[] GetAllSkill()
    {
        return skillArr;
    }
    public Buff GetBuffByID(int buffID)
    {
        for (int i = 0; i < buffLst.Count; i++)
        {
            if(buffLst[i].cfg.buffID==buffID)
            {
                return buffLst[i];
            }
        }
        return null;
    }
    public void ClearFreeAniCallBack()
    { 
        for (int i = 0; i < skillArr.Length; i++)
        {
            if(skillArr[i].skillState!=SkillState.SpellStart)
            {
                skillArr[i].FreeAniCallback = null;
            }
        }
    }
    /// <summary>
    /// 是否处于施法阶段
    /// </summary>
    /// <returns></returns>
    public bool IsSkillSpelling()
    {
        for (int i = 0; i < skillArr.Length; i++)
        {
            if(skillArr[i].skillState==SkillState.SpellStart)
            {
                return true;
            }
        }
        return false;
    }
    bool IsSkillReady(int skillID)
    {
        for (int i = 0; i < skillArr.Length; i++)
        {
            if(skillArr[i].skillID==skillID)
            {
                return skillArr[i].skillState == SkillState.None;
            }
        }
        this.Warn("skill id config error");
        return false;
    }
    /// <summary>
    /// 是否可以施放技能
    /// </summary>
    /// <param name="skillID"></param>
    /// <returns></returns>
    public bool CanReleaseSkill(int skillID)
    {
        return IsSilenced() == false
            && IsStunned() == false
            && IsKnockup() == false
            && IsSkillSpelling() == false
            &&IsSkillReady(skillID);
    }
    /// <summary>
    /// 是否禁止施放技能
    /// </summary>
    /// <returns></returns>
    public bool IsForbidReleaseSkill()
    {
        return IsSilenced()
            ||IsStunned()
            ||IsKnockup();
    }
    #endregion

}

