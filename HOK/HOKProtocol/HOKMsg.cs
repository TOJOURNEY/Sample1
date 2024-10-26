using PENet;
using System;
using System.Collections.Generic;

namespace HOKProtocol
{
    /// <summary>
    /// 网络通讯数据协议
    /// </summary>
    [Serializable]
    public class HOKMsg :KCPMsg
    {
        public CMD cmd;
        public ErrorCode error;
        public bool isEmpty;
        public ReqLogin reqLogin;
        public RspLogin rspLogin;
        public ReqMatch reqMatch;
        public RspMatch rspMatch;
        public NtfConfirm ntfConfirm;
        public SndConfirm sndConfirm;
        public NtfSelect ntfSelect;
        public SndSelect sndSelect;
        public NtfLoadRes ntfLoadRes;
        public SndLoadPrg sndLoadPrg; 
        public NtfLoadPrg ntfLoadPrg;

        public ReqBattleStart reqBattleStart;
        public RspBattleStart rspBattleStart;

        public SndOpKey sndOpKey;
        public NtfOpKey ntfOpKey;

        public ReqBattleEnd reqBattleEnd;
        public RspBattleEnd rspBattleEnd;

        public SndChat sndChat;
        public NtfChat ntfChat;

        public ReqPing reqPing;
        public RspPing rspPing;

    }
    #region 登录相关协议
    /// <summary>
    /// 客户端请求登录
    /// </summary>
    [Serializable]
    public class ReqLogin
    {
        /// <summary>
        /// 账号
        /// </summary>
        public string acct;
        /// <summary>
        /// 密码
        /// </summary>
        public string pass;
    }
    /// <summary>
    /// 服务器回应登录
    /// </summary>
    [Serializable]
    public class RspLogin
    {
        /// <summary>
        /// 嵌套的玩家数据类
        /// </summary>
        public UserData userData;
    }
    /// <summary>
    /// 嵌套的玩家数据类
    /// </summary>
    [Serializable]
    public class UserData
    {
        /// <summary>
        /// ID
        /// </summary>
        public uint id;
        /// <summary>
        /// 名字
        /// </summary>
        public string  name;
        /// <summary>
        /// 等级
        /// </summary>
        public int lv;
        /// <summary>
        /// 经验值
        /// </summary>
        public int exp;
        /// <summary>
        /// 金币数量
        /// </summary>
        public int coin;
        /// <summary>
        /// 钻石
        /// </summary>
        public int diamond;
        /// <summary>
        /// 点卷
        /// </summary>
        public int ticket;
        /// <summary>
        /// 嵌套集合，可供选择的英雄数据
        /// </summary>
        public List<HeroSelectData> heroSelectData;
    }
    
    /// <summary>
    /// 可供选择的英雄数据
    /// </summary>
    [Serializable]
    public class HeroSelectData
    {
        public int heroID;
        //已拥有
        //本周限免
        //体验卡
    }
    #endregion

    #region 匹配确认相关协议
    /// <summary>
    /// 匹配类型
    /// </summary>
    [Serializable]
    public enum PvPEnum
    {
         None=0,
         _1v1=1,
         _3v3=2,
         _5v5=3,
    }

    [Serializable]
    public class ReqMatch
    {
        /// <summary>
        /// 匹配类型
        /// </summary>
        public PvPEnum pvpEnum;
    }
   
    [Serializable]
    public class RspMatch
    {
        public int predictTime;
    }
    /// <summary>
    /// 服务器推送匹配确认数据
    /// </summary>
    [Serializable] 
    public class NtfConfirm
    {
        /// <summary>
        /// 匹配房间ID号
        /// </summary>
        public uint roomID;
        /// <summary>
        /// 解散匹配
        /// </summary>
        public bool dissmiss;//解散
        /// <summary>
        /// 嵌套的匹配确认数据类
        /// </summary>
        public ConfirmData[] confirmArr;
    }
    /// <summary>
    /// 嵌套的匹配确认数据类
    /// </summary>
    [Serializable]
    public class ConfirmData
    {
        /// <summary>
        /// 头像数据
        /// </summary>
        public int iconIndex;
        /// <summary>
        /// 是否已经确认
        /// </summary>
        public bool confirmDone;
    }
    /// <summary>
    /// 客户端请求匹配确认数据
    /// </summary>
    [Serializable]
    public class SndConfirm
    {
        /// <summary>
        /// 匹配房间ID号
        /// </summary>
        public uint roomID;
    }
    #endregion

    #region 选择与加载
    /// <summary>
    /// 服务器推送英雄选择
    /// </summary>
    [Serializable]
    public class NtfSelect
    {

    }
    /// <summary>
    /// 选择英雄的数据
    /// </summary>
    [Serializable]
    public class SelectData
    {
        /// <summary>
        /// 选择的英雄ID号
        /// </summary>
        public int selectID;
        /// <summary>
        /// 是否已经确认选择英雄
        /// </summary>
        public bool selectDone;
    }
    /// <summary>
    /// 客户端请求选择数据
    /// </summary>
    [Serializable]
    public class SndSelect
    {
        public uint roomID;
        /// <summary>
        /// 英雄ID
        /// </summary>
        public int heroID;
    }
    /// <summary>
    /// 服务器推送加载数据 
    /// </summary>
    [Serializable]
    public class NtfLoadRes
    {
        /// <summary>
        /// 地图ID
        /// </summary>
        public int mapID;
        /// <summary>
        /// 玩家数据类
        /// </summary>
        public List<BattleHeroData> heroList;
        /// <summary>
        /// 玩家数据位置
        /// </summary>
        public int posIndex;
    }
    /// <summary>
    /// 玩家拥有的数据
    /// </summary>
    [Serializable]
    public class BattleHeroData
    {
        /// <summary>
        /// 玩家名字
        /// </summary>
        public string userName;//玩家名字
        /// <summary>
        /// 英雄ID号
        /// </summary>
        public int heroID;
        //级别，皮肤ID，边框，称号TODO
    }
    /// <summary>
    /// 客户端请求加载数据
    /// </summary>
    [Serializable]
    public class SndLoadPrg
    {
        /// <summary>
        /// 房间ID
        /// </summary>
        public uint roomID;
        /// <summary>
        /// 进度
        /// </summary>
        public int percent;
    }
    /// <summary>
    /// 服务器推送加载进度
    /// </summary>
    [Serializable]
    public class NtfLoadPrg
    {
        /// <summary>
        /// 加载进度
        /// </summary>
        public List<int> percentLst;
    }
    #endregion

    #region 核心战斗
    /// <summary>
    /// 客户端请求战斗开始
    /// </summary>
    [Serializable]
    public class ReqBattleStart
    {
        /// <summary>
        /// 房间ID
        /// </summary>
        public uint roomID;
    }
    /// <summary>
    /// 服务器回应战斗是否开始
    /// </summary>
    [Serializable]
    public class RspBattleStart
    {
        
    }

    [Serializable]
    public class SndOpKey
    {
        public uint roomID;
        public OpKey opKey;
    }

    [Serializable]
    public class NtfOpKey
    {
        public uint frameID;
        public List<OpKey> keyList;
    }
    [Serializable]
    public class ReqBattleEnd
    {
        public uint roomID;
    }
    [Serializable]
    public class RspBattleEnd
    {
        //结算数据
    }

    [Serializable]
    public class SndChat
    {
        public uint roomID;
        public string chatMsg;
    }
    [Serializable]
    public class NtfChat
    {
        public string chatMsg;
    }
    #endregion

    [Serializable]
    public class ReqPing
    {
        public uint pingID;
        public ulong sendTime;
        public ulong backTime;
    }
    [Serializable]
    public class RspPing
    {
        public uint pingID;
    }
    /// <summary>
    /// 错误码
    /// </summary>
    public enum ErrorCode
    {
        None,
        AcctisOnline,
    }
    /// <summary>
    /// 通信协议命令号
    /// </summary>
    public enum CMD
    {
        None=0,
        ReqLogin=1,
        RspLogin=2,

        //匹配
        ReqMatch=3,
        RspMatch=4,

        //确认
        NtfConfirm=5,
        SndConfirm=6,

        //选择
        NtfSelect=7,
        SndSelect=8,

        //加载
        NtfLoadRes=9,
        SndLoadPrg=10,
        NtfLoadPrg=11,

        //战斗
        ReqBattleStart=12,
        RspBattleStart=13,

        //PING
        ReqPing = 14,
        RspPing = 15,

        //操作码
        SndOpkey =100,
        NtfOpkey=101,

        //结算
        ReqBattleEnd = 201,
        RspBattleEnd = 202,

        //聊天
        SndChat = 203,
        NtfChat = 204
    }
}
