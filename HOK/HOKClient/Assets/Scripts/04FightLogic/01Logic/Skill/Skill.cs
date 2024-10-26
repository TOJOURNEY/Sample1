using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEMath;

/// <summary>
/// 技能状态
/// </summary>
public enum SkillState
{
    None,
    /// <summary>
    /// 施法阶段
    /// </summary>
    SpellStart,
    /// <summary>
    /// 施法成功
    /// </summary>
    SpellAter,
}
/// <summary>
/// 技能
/// </summary>
public class Skill
{
    public int skillID;
    public SkillCfg cfg;
    public PEVector3 skillArgs;
    public MainLogicUnit lockTarget;
    public SkillState skillState = SkillState.None;
    /// <summary>
    /// 施法时间
    /// </summary>
    public PEInt spellTime;
    /// <summary>
    /// 技能总时间
    /// </summary>
    public PEInt skillTime;

    public MainLogicUnit owner;

    public Action FreeAniCallback;
    public Action<Skill> SpellSuccCallback;
    public Skill(int skillID,MainLogicUnit owner)
    {
        this.skillID = skillID;
        cfg = ResSvc.Instance.GetSkillConfigByID(this.skillID);
        spellTime = cfg.spellTime;
        skillTime = cfg.skillTime;

        if(cfg.isNormalAttack)
        {
            owner.InitAttackSpeedRate(1000/skillTime);
        }

        this.owner = owner;
    }
    /// <summary>
    /// 命中目标
    /// </summary>
    /// <param name="target"></param>
    /// <param name="args"></param>
    void HitTarget(MainLogicUnit target,object[] args=null)
    {
        //音效表现
        if(cfg.audio_hit!=null)
        {
            target.PlayAudio(cfg.audio_hit);
        }
        //可能全为buff伤害，这里为0
        if (cfg.damage!=0)
        {
            PEInt damage = cfg.damage;
            target.GetDamageBySkill(damage, this);
        }
        //附加buff到目标
        if(cfg.buffIDArr==null)
        {
            return;
        }

        for (int i = 0; i < cfg.buffIDArr.Length; i++)
        {
            int buffID = cfg.buffIDArr[i];
            if(buffID==0)
            {
                this.Warn($"SkillID:{cfg.skillID} exist buffID==0,check your buffID Configs");
                continue;
            }
            BuffCfg buffCfg = ResSvc.Instance.GetBuffConfigByID(buffID);
            if(buffCfg.attacher==AttachTypeEnum.Target
                ||buffCfg.attacher==AttachTypeEnum.Bullet){
                target.CreateSkillBuff(owner, this, buffID, args);
            }
        }
    }
    /// <summary>
    /// 技能生效
    /// </summary>
    /// <param name="lockTarget"></param>
    void CalcSkillAttack(MainLogicUnit lockTarget)
    {
        if(cfg.bulletCfg!=null)
        {
            TargetBullet bullet = owner.CreateSkillBullet(owner, lockTarget, this) as TargetBullet;
            bullet.HitTargetCB = HitTarget;
        }
        else
        {
            HitTarget(lockTarget);
        }
    }
    /// <summary>
    /// 施法前摇开始，瞬时技能这个时间阶段为0
    /// </summary>
    void SkillSpellStart(PEVector3 spellDir)
    {
        skillState = SkillState.SpellStart;
        if (cfg.audio_start != null)
        {
            owner.PlayAudio(cfg.audio_start);
        }
        if (spellDir != PEVector3.zero)
        {
            owner.mainViewUnit.UpdateSkillRotation(spellDir);
        }
        if (cfg.aniName != null)
        {
            owner.InputFakeMoveKey(PEVector3.zero);
            owner.ClearFreeAniCallBack();
            owner.PlayAni(cfg.aniName);
            //技能被中断或后摇被移动取消需要调用动画重置
            FreeAniCallback = () => {
                owner.PlayAni("free");
            };
        }
    }

    void SkillSpellAfter()
    {
        skillState = SkillState.SpellAter;
        if(cfg.audio_work!=null)
        {
            owner.PlayAudio(cfg.audio_work);
        }

        //施法成功消耗相应资源
        if(owner.IsPlayerSelf()&&!cfg.isNormalAttack)
        {
            //进入技能CD
            BattleSys.Instance.EnterCDState(skillID, cfg.cdTime);
        }
        //技能施放成功回调，以提供事件给buff使用
        SpellSuccCallback?.Invoke(this);

        if(cfg.aniName!=null)
        {
            owner.RecoverUIInput();
        }
        //启动定时器，在后摇完成后技能状态重置为None
        //配置的技能时间必须大于施法时间，否则就无意义
        if(skillTime>spellTime)
        {
            owner.CreateLogicTimer(SkillEnd, skillTime - spellTime);
        }
        else
        {
            SkillEnd();
        }

    }
    /// <summary>
    /// 施法后摇动作完成，角色切换到Idle状态
    /// </summary>
    void SkillEnd()
    {
        if(skillState==SkillState.None||skillState==SkillState.SpellStart)
        {
            if(owner.IsPlayerSelf())
            {
                if(cfg.targetCfg!=null
                    &&cfg.targetCfg.targetTeam==TargetTeamEnum.Enemy
                    &&cfg.targetCfg.searchDis>0){
                    Buff mf = owner.GetBuffByID(ClientConfig.CommonMoveAttackBuffID);
                    if(mf!=null)
                    {
                        mf.unitState = SubUnitState.End;
                    }

                    this.Log("技能未施放成功，添加通用移动攻击buff");
                    owner.CreateSkillBuff(owner, this, ClientConfig.CommonMoveAttackBuffID) ;
                }
            }
        }
        if(FreeAniCallback!=null)
        {
            FreeAniCallback();
            FreeAniCallback = null;
        }
        skillState = SkillState.None;
        lockTarget = null;
    }
    /// <summary>
    /// 施放技能
    /// </summary>
    /// <param name="skillArgs"></param>
    public void ReleaseSkill(PEVector3 skillArgs)
    {
        this.skillArgs = skillArgs; 
        //目标技能，必须存在施法目标，且目标队伍类型不能为动态类型
        if(cfg.targetCfg!=null&&cfg.targetCfg.targetTeam!=TargetTeamEnum.Dynamic)
        {
            lockTarget = CalcRule.FindSingleTargetByRule(owner, cfg.targetCfg, skillArgs);
            if(lockTarget!=null)
            {
                PEVector3 spellDir = lockTarget.LogicPos - owner.LogicPos;
                SkillSpellStart(spellDir);

                void SkillWork()
                {
                    CalcSkillAttack(lockTarget);
                    AttachSkillBuffToCaster();
                    SkillSpellAfter();
                }
                if(spellTime==0)
                {
                    this.Log("瞬发技能，立即生效");
                    //附着buff
                    SkillWork();
                }
                else
                {
                    void DelaySkillWork()
                    {
                        lockTarget = CalcRule.FindSingleTargetByRule(owner, cfg.targetCfg, skillArgs);
                        if(lockTarget!=null)
                        {
                            SkillWork();
                        }
                        else
                        {
                            SkillEnd();
                        }
                    }
                   
                    //定时处理
                    owner.CreateLogicTimer(DelaySkillWork, spellTime);
                }
            }
            else
            {
                this.Warn("没有符合条件的技能目标");
                SkillEnd();
            }
        }
        //非目标技能
        else
        {
            SkillSpellStart(skillArgs);

            void DirectionBullet()
            {
                //非目标弹道技能
                DirectionBullet bullet = owner.CreateSkillBullet(owner, null, this) as DirectionBullet;
                bullet.hitTargetCB = (MainLogicUnit target, object[] args) => {
                    this.Log("路径上击中目标：" + target.unitName);
                    HitTarget(target, args);
                };
                bullet.ReachPosCB = () => {
                    this.Log("子弹达到最终位置");
                };
            }
            if(spellTime==0)
            {
                if(cfg.bulletCfg!=null)
                {
                    DirectionBullet();
                }
                AttachSkillBuffToCaster();
                SkillSpellAfter();
            }
            else
            {
                owner.CreateLogicTimer(() =>
                {
                    if (cfg.bulletCfg != null)
                    {
                        DirectionBullet();
                    }
                    AttachSkillBuffToCaster();
                    SkillSpellAfter();
                },spellTime);
            }
        }
    }
    void AttachSkillBuffToCaster()
    {
        if(cfg.buffIDArr==null)
        {
            return;
        }
        for (int i = 0; i < cfg.buffIDArr.Length; i++)
        {
            int buffID = cfg.buffIDArr[i];
            if (buffID == 0)
            {
                this.Warn(string.Format("SkillID:{0} exist: buffID ==0,Check your buffID Configs.", cfg.skillID));
                continue;
            }

            BuffCfg buffCfg = ResSvc.Instance.GetBuffConfigByID(buffID);
            if(buffCfg.attacher==AttachTypeEnum.Caster||buffCfg.attacher==AttachTypeEnum.Indie)
            {
                owner.CreateSkillBuff(owner, this, buffID);
            }
        }
    }
    int tempSkillID;
    public int TempSkillID
    {
        set
        {
            tempSkillID = value;
            this.Log("Set TempSkillID:" + value);
        }
        get
        {
            return tempSkillID;
        }
    }
    /// <summary>
    /// 技能替换
    /// </summary>
    /// <param name="replaceID"></param>
    public void ReplaceSkillCfg(int replaceID)
    {
        if (skillID == replaceID)
        {
            TempSkillID = 0;
        }
        else
        {
            TempSkillID = replaceID;
        }

        cfg = ResSvc.Instance.GetSkillConfigByID(replaceID);

        //spellTime = cfg.spellTime;
        //skillTime = cfg.skillTime;
        //if (cfg.isNormalAttack)
        //{
        //    owner.InitAttackSpeedRate(1000 / skillTime);
        //}

    }
}

