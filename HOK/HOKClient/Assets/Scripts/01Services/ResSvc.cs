using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using PEPhysx;
using PEMath;


/// <summary>
/// 资源管理服务
/// </summary>
public class ResSvc : MonoBehaviour
{
    public static ResSvc Instance;

    public void InitSvc()
    {
        Instance = this;
        this.Log("Init ResSvc Done");
    }
    private Action prgCB = null;
    /// <summary>
    /// 异步加载场景
    /// </summary>
    /// <param name="sceneName">场景名字</param>
    /// <param name="loadRate">加载进度</param>
    /// <param name="loaded">加载完需要做什么（告诉服务器战斗开始）</param>
    public void AsyncLoadScene(string sceneName,Action<float> loadRate,Action loaded)
    {
        AsyncOperation sceneAsync = SceneManager.LoadSceneAsync(sceneName);
        //使用prgCB包裹调用，每一帧进行调用
        prgCB = () =>
        {
            float progress = sceneAsync.progress;
            loadRate?.Invoke(progress);
            if (progress == 1)
            {
                loaded?.Invoke();
                prgCB = null;
                sceneAsync = null;

            }
        };
        
    }
    private void Update()
    {
        /// 在updata里面每一帧去判断是否为空，不为空一直调用prg
        prgCB?.Invoke();
    }
    private Dictionary<string, GameObject> goDic = new Dictionary<string, GameObject>();
    public GameObject LoadPrefab(string path, bool cache = false)
    {
        GameObject prefab = null;
        if (!goDic.TryGetValue(path, out prefab))
        {
            prefab = Resources.Load<GameObject>(path);
            if (cache)
            {
                goDic.Add(path, prefab);
            }
        }

        GameObject go = null; 
        if(prefab!=null)
        {
            go = Instantiate(prefab);
        }

        return go;
    }
    /// <summary>
    /// 音频集合
    /// </summary>
    private Dictionary<string, AudioClip> adDic = new Dictionary<string, AudioClip>();
    /// <summary>
    /// 加载音频
    /// </summary>
    /// <param name="path">路径</param>
    /// <param name="cache">用来判断集合里是否存在</param>
    /// <returns></returns>
    public AudioClip LoadAudio(string path, bool cache = false)
    {
        AudioClip au = null;
        if(!adDic.TryGetValue(path,out au))
        {
            au = Resources.Load<AudioClip>(path);
            if (cache)
            {
                adDic.Add(path, au);
            }
        }
        return au;
    }
    private Dictionary<string, Sprite> spDic = new Dictionary<string, Sprite>();
    /// <summary>
    /// 图片加载
    /// </summary>
    /// <param name="path">路径</param>
    /// <param name="cache">用来判断集合里是否存在</param>
    /// <returns></returns>
    public Sprite LoadSprite(string path,bool cache=false)
    {
        Sprite sp = null;
        if(!spDic.TryGetValue(path,out sp))
        {
            sp = Resources.Load<Sprite>(path);
            if(cache)
            {
                spDic.Add(path, sp);
            }
        }
        return sp;
    }

    public Buff CreateBuff(MainLogicUnit source,MainLogicUnit owner,Skill skill,int buffID,object[] args)
    {
        BuffCfg cfg = GetBuffConfigByID(buffID);
        switch (cfg.buffType)
        {
            case BuffTypeEnum.MoveAttack:
                return new MoveAttackBuff(source, owner, skill, buffID, args);
            case BuffTypeEnum.MoveSpeed_Single:
                return new MoveSpeedBuff_Single(source, owner, skill, buffID, args);
            case BuffTypeEnum.ModifySkill:
                return new CommonModifySkillBuff(source, owner, skill, buffID, args);
            case BuffTypeEnum.Silense:
                return new SilenseBuff_Single(source, owner, skill, buffID, args);
            case BuffTypeEnum.ArthurMark:
                return new ArthurMarkBuff(source, owner, skill, buffID, args);
            case BuffTypeEnum.MoveSpeed_DynamicGroup:
                return new MoveSpeedBuff_DynamicGroup(source, owner, skill, buffID, args);
            case BuffTypeEnum.HPCure:
                return new HPCureBuff_Single(source, owner, skill, buffID, args);
            case BuffTypeEnum.Knockup_Group:
                return new KnockUpBuff_Group(source, owner, skill, buffID, args);

            case BuffTypeEnum.Damage_DynamicGroup:
                return new DamageBuff_DynamicGroup(source, owner, skill, buffID, args);
            case BuffTypeEnum.TargetFlashMove:
                return new TargetFlashMoveBuff(source, owner, skill, buffID, args);
            case BuffTypeEnum.ExecuteDamage:
                return  new ExecuteDamageBuff(source, owner, skill, buffID, args);
            case BuffTypeEnum.Damage_StaticGroup:
                return new DamageBuff_StaticGroup(source, owner, skill, buffID, args);

            //后羿技能
            case BuffTypeEnum.HouyiPasvAttackSpeed:
                return new HouyiPasvAttackSpeedBuff(source, owner, skill, buffID, args);
            case BuffTypeEnum.HouyiPasvSkillModify:
                return new HouyiMultipleSkillModifyBuff(source, owner, skill, buffID, args);
            case BuffTypeEnum.HouyiPasvMultiArrow:
                return new HouyiMultipleArrowBuff(source, owner, skill, buffID, args);
            case BuffTypeEnum.HouyiActiveSkillModify:
                return new HouyiScatterSkillModifyBuff(source, owner, skill, buffID, args);
            case BuffTypeEnum.Scatter:
                return new HouyiScatterArrowBuff(source, owner, skill, buffID, args);
            case BuffTypeEnum.HouyiMixedMultiScatter:
                return new HouyiMixedMultiScatterBuff(source, owner, skill, buffID, args);
            case BuffTypeEnum.MoveSpeed_StaticGroup:
                return new MoveSpeedBuff_StaticGroup(source, owner, skill, buffID, args);

            case BuffTypeEnum.Stun_Single_DynamicTime:
                return new StunBuff_DynamicTime(source, owner, skill, buffID, args);


            case BuffTypeEnum.None:
            default:
                this.Error("Create Buff Failed,BuffID:" + buffID);
                return null;
        }
    }
    /// <summary>
    /// 创建子弹
    /// </summary>
    /// <param name="source"></param>
    /// <param name="target"></param>
    /// <param name="skill"></param>
    /// <returns></returns>
    public Bullet CreateBullet(MainLogicUnit source, MainLogicUnit target, Skill skill)
    {
        switch (skill.cfg.bulletCfg.bulletType)
        {
            case BulletTypeEnum.SkillTarget:
                return new TargetBullet(source, target, skill);
            case BulletTypeEnum.UIDirection:
                return new DirectionBullet(source, skill);
            case BulletTypeEnum.UIPosition:
            case BulletTypeEnum.BuffSearch:
            default:
                this.Error("Create Bullet Error.");
                return null;
        }
    }
    public UnitCfg GetUnitConfigByID(int unitID)
    {
        switch (unitID)
        {
            case 101:
                return new UnitCfg
                {
                    unitID = 101,
                    unitName = "亚瑟",
                    resName = "arthur",
                    hitHeight = (PEInt)1.5F,
                    hp = 6500,
                    def = 0,
                    moveSpeed = 5,
                    colliCfg = new ColliderConfig
                    {
                        mType = ColliderType.Cylinder,
                        mRadius = (PEInt)0.5f
                    },
                    pasvBuff = new int[] { 10100 },
                    skillArr = new int[] { 1010, 1011, 1012, 1013 }
                };
            case 102:
                return new UnitCfg
                {
                    unitID = 102,
                    unitName = "后羿",
                    resName = "houyi",
                    hitHeight = (PEInt)1.5F,
                    hp = 3500,
                    def = 10,
                    moveSpeed = 5,
                    colliCfg = new ColliderConfig
                    {
                        mType = ColliderType.Cylinder,
                        mRadius = (PEInt)0.5f
                    },
                    pasvBuff = new int[] { 10200, 10201 },
                    skillArr = new int[] { 1020, 1021, 1022, 1023 }
                };

            case 1001:
                return new UnitCfg
                {
                    unitID = 1001,
                    unitName = "蓝方一塔",
                    resName = "blueTower",
                    hitHeight = (PEInt)1.5f,
                    hp = 400,
                    def = 0,
                    colliCfg = new ColliderConfig
                    {
                        mType = ColliderType.Cylinder,
                        mRadius = (PEInt)0.25f,
                    },
                    pasvBuff = null,
                    skillArr = new int[] { 10010 }
                };
            case 1002:
                return new UnitCfg
                {
                    unitID = 1002,
                    unitName = "蓝方水晶",
                    resName = "blueCrystal",
                    hitHeight = (PEInt)1f,
                    hp = 800,
                    def = 0,
                    colliCfg = new ColliderConfig
                    {
                        mType = ColliderType.Cylinder,
                        mRadius = (PEInt)1f,
                    },
                    pasvBuff = null,
                    skillArr = new int[] { 10020 }
                };
            case 1003:
                return new UnitCfg
                {
                    unitID = 1003,
                    unitName = "蓝方近战小兵",
                    resName = "xb_blue_jz",
                    hitHeight = (PEInt)0.6f,
                    hp = 500,
                    def = 0,
                    moveSpeed = 2,
                    colliCfg = new ColliderConfig
                    {
                        mType = ColliderType.Cylinder,
                        mRadius = (PEInt)0.25f,
                    },
                    pasvBuff = null,
                    skillArr = new int[] { 10030 }
                };
            case 1004:
                return new UnitCfg
                {
                    unitID = 1004,
                    unitName = "蓝方远程小兵",
                    resName = "xb_blue_yc",
                    hitHeight = (PEInt)0.6f,
                    hp = 300,
                    def = 0,
                    moveSpeed = 2,
                    colliCfg = new ColliderConfig
                    {
                        mType = ColliderType.Cylinder,
                        mRadius = (PEInt)0.25f,
                    },
                    pasvBuff = null,
                    skillArr = new int[] { 10040 }
                };

            case 2001:
                return new UnitCfg
                {
                    unitID = 2001,
                    unitName = "红方一塔",
                    resName = "redTower",
                    hitHeight = (PEInt)1.5f,
                    hp = 400,
                    def = 0,
                    colliCfg = new ColliderConfig
                    {
                        mType = ColliderType.Cylinder,
                        mRadius = (PEInt)0.25f,
                    },
                    pasvBuff = null,
                    skillArr = new int[] { 20010 }
                };
            case 2002:
                return new UnitCfg
                {
                    unitID = 2002,
                    unitName = "红方水晶",
                    resName = "redCrystal",
                    hitHeight = (PEInt)1f,
                    hp = 800,
                    def = 0,
                    colliCfg = new ColliderConfig
                    {
                        mType = ColliderType.Cylinder,
                        mRadius = (PEInt)1f,
                    },
                    pasvBuff = null,
                    skillArr = new int[] { 20020 }
                };
            case 2003:
                return new UnitCfg
                {
                    unitID = 2003,
                    unitName = "红方近战小兵",
                    resName = "xb_red_jz",
                    hitHeight = (PEInt)0.6f,
                    hp = 500,
                    def = 0,
                    moveSpeed = 2,
                    colliCfg = new ColliderConfig
                    {
                        mType = ColliderType.Cylinder,
                        mRadius = (PEInt)0.25f,
                    },
                    pasvBuff = null,
                    skillArr = new int[] { 20030 }
                };
            case 2004:
                return new UnitCfg
                {
                    unitID = 2004,
                    unitName = "红方远程小兵",
                    resName = "xb_red_yc",
                    hitHeight = (PEInt)0.6f,
                    hp = 300,
                    def = 0,
                    moveSpeed = 2,
                    colliCfg = new ColliderConfig
                    {
                        mType = ColliderType.Cylinder,
                        mRadius = (PEInt)0.25f,
                    },
                    pasvBuff = null,
                    skillArr = new int[] { 20040 }
                };
        }
        return null;
    }
    /// <summary>
    /// 获取buff配置文件
    /// </summary>
    /// <param name="buffID"></param>
    /// <returns></returns>
    public BuffCfg GetBuffConfigByID(int buffID)
    {
        switch (buffID)
        {

            case 10100://亚瑟被动回血
                return ResBuffConfigs.buff_10100;

            //亚瑟一技能
            case 10110://移速加速
                return ResBuffConfigs.buff_10110;
            case 10111://强化普攻
                return ResBuffConfigs.buff_10111;
            case 10140:
                return ResBuffConfigs.buff_10140; 
            case 10141:
                return ResBuffConfigs.buff_10141;
            case 10142:
                return ResBuffConfigs.buff_10142;

            //亚瑟二技能
            case 10120:
                return ResBuffConfigs.buff_10120;

            //亚瑟三技能
            case 10130:
                return ResBuffConfigs.buff_10130;
            case 10131:
                return ResBuffConfigs.buff_10131;
            case 10132:
                return ResBuffConfigs.buff_10132;
            case 10133:
                return ResBuffConfigs.buff_10133;

            //后羿被动技能buff
            case 10200:
                return ResBuffConfigs.buff_10200;
            case 10201:
                return ResBuffConfigs.buff_10201;
            case 10250:
                return ResBuffConfigs.buff_10250;
            //后羿一技能
            case 10210://技能替换
                return ResBuffConfigs.buff_10210;
            case 10240://散射     scatter
                return ResBuffConfigs.buff_10240;
            case 10260://混合强化     
                return ResBuffConfigs.buff_10260;

            //后羿二技能
            case 10220: 
                return ResBuffConfigs.buff_10220;
            case 10221:   
                return ResBuffConfigs.buff_10221;
            case 10222:   
                return ResBuffConfigs.buff_10222;
            case 10223:   
                return ResBuffConfigs.buff_10223;

            //后羿三技能
            case 10230:     
                return ResBuffConfigs.buff_10230;
            case 10231:
                return ResBuffConfigs.buff_10231;

            //通用
            case 90000:
                return ResBuffConfigs.buff_90000;
            default:
                break;
        }
        this.Error("Get Buff Config Failed,buffID:" + buffID);
        return null;
    }
    public SkillCfg GetSkillConfigByID(int skillID)
    {
        switch (skillID)
        {
            case 1010:
                return ResSkillConfigs.sk_1010;
            case 1011:
                return ResSkillConfigs.sk_1011;
            case 1012:
                return ResSkillConfigs.sk_1012;
            case 1013:
                return ResSkillConfigs.sk_1013;
            case 1014:
                return ResSkillConfigs.sk_1014;
            case 1020:
                return ResSkillConfigs.sk_1020;
            case 1021:
                return ResSkillConfigs.sk_1021;
            case 1022:
                return ResSkillConfigs.sk_1022;
            case 1023:
                return ResSkillConfigs.sk_1023;
            case 1024:
                return ResSkillConfigs.sk_1024;
            case 1025:
                return ResSkillConfigs.sk_1025;
            case 1026:
                return ResSkillConfigs.sk_1026;


            //防御塔与水晶的技能
            case 10010:
                return ResSkillConfigs.sk_10010;
            case 10020:
                return ResSkillConfigs.sk_10020;
            case 10030:
                return ResSkillConfigs.sk_10030;
            case 10040:
                return ResSkillConfigs.sk_10040;
            case 20010:
                return ResSkillConfigs.sk_20010;
            case 20020:
                return ResSkillConfigs.sk_20020;
            case 20030:
                return ResSkillConfigs.sk_20030;
            case 20040:
                return ResSkillConfigs.sk_20040;


            default:
                this.Error("Get Skill Config Failed,skillID:" + skillID);
                return null;
        }
    }
    public MapCfg GetMapConfigByID(int mapID)
    {
        switch (mapID)
        {
            case 101:
                return new MapCfg
                {
                    mapID = 101,
                    blueBorn = new PEVector3(-27, 0, 0),
                    redBorn = new PEVector3(27, 0, 0),
                    //blueBorn = new PEVector3(-5, 0, -3),
                    //redBorn = new PEVector3(5, 0, -3),
                    towerIDArr = new int[] { 1001, 1002, 2001, 2002 },
                    towerPosArr = new PEVector3[] {
                        new PEVector3(-(PEInt)12.6f, 0, -(PEInt)0.2f),
                        new PEVector3(-(PEInt)24.1f, 0, -(PEInt)0.2f),
                        new PEVector3((PEInt)12.6f, 0, -(PEInt)0.2f),
                        new PEVector3((PEInt)24.1f, 0, -(PEInt)0.2f)
                    },
                    bornDelay = 15000,
                    bornInterval = 2000,
                    waveInterval = 50000,
                    blueSoldierIDArr = new int[] { 1003, 1003, 1004, 1004 },
                    blueSoldierPosArr = new PEVector3[] {
                        new PEVector3(-22,0,-(PEInt)1.7f),
                        new PEVector3(-22,0,(PEInt)1.7f),
                        new PEVector3(-22,0,-(PEInt)1.7f),
                        new PEVector3(-22,0,(PEInt)1.7f),
                    },
                    redSoldierIDArr = new int[] { 2003, 2003, 2004, 2004 },
                    redSoldierPosArr = new PEVector3[] {
                        new PEVector3(22,0,-(PEInt)1.7f),
                        new PEVector3(22,0,(PEInt)1.7f),
                        new PEVector3(22,0,-(PEInt)1.7f),
                        new PEVector3(22,0,(PEInt)1.7f),
                    },

                };
            case 102:
                return new MapCfg
                {
                    mapID = 102,
                    blueBorn = new PEVector3(-5, 0, -3),
                    redBorn = new PEVector3(5, 0, -3),
                    towerIDArr = new int[] { 1001, 1002, 2001, 2002 },
                    towerPosArr = new PEVector3[] {
                        new PEVector3(-(PEInt)12.6f, 0, -(PEInt)0.2f),
                        new PEVector3(-(PEInt)24.1f, 0, -(PEInt)0.2f),
                        new PEVector3((PEInt)12.6f, 0, -(PEInt)0.2f),
                        new PEVector3((PEInt)24.1f, 0, -(PEInt)0.2f)
                    },
                    bornDelay = 15000,
                    bornInterval = 2000,
                    waveInterval = 50000,
                    blueSoldierIDArr = new int[] { 1003, 1003, 1004, 1004 },
                    blueSoldierPosArr = new PEVector3[] {
                        new PEVector3(-22,0,-(PEInt)1.7f),
                        new PEVector3(-22,0,(PEInt)1.7f),
                        new PEVector3(-22,0,-(PEInt)1.7f),
                        new PEVector3(-22,0,(PEInt)1.7f),
                    },
                    redSoldierIDArr = new int[] { 2003, 2003, 2004, 2004 },
                    redSoldierPosArr = new PEVector3[] {
                        new PEVector3(22,0,-(PEInt)1.7f),
                        new PEVector3(22,0,(PEInt)1.7f),
                        new PEVector3(22,0,-(PEInt)1.7f),
                        new PEVector3(22,0,(PEInt)1.7f),
                    },
                };
            default:
                return null;
        }
    }
}