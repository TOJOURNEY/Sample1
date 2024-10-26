using System;
using System.Collections.Generic;
using System.Text;

namespace HOKProtocol
{
    [Serializable]
    public enum KeyType
    {
        None,
        Move,
        Skill,

    }
    [Serializable]
    public class OpKey
    {
        public int opIndex;
        public KeyType keyType;

        /// <summary>
        /// 释放技能
        /// </summary>
        public SkillKey skillKey;
        /// <summary>
        /// 移动
        /// </summary>
        public MoveKey moveKey;
    }

    /// <summary>
    /// 释放技能
    /// </summary>
    [Serializable]
    public class SkillKey
    {
        //debug
        public uint skillID;

        public long x_value;
        public long z_value;

    }
    /// <summary>
    /// 移动
    /// </summary>
    [Serializable]
    public class MoveKey
    {
        //debug
        public uint keyID;

        public long x;
        public long z;

    }
    
}
