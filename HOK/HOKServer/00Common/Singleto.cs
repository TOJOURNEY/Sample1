using System;
using System.Collections.Generic;
using System.Text;



namespace HOKServer
{
    /// <summary>
    /// 服务器单例类
    /// </summary>
    public class Singleto<T> where T : new()
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if(instance==null)
                {
                    instance = new T();
                }
                return instance;
            }
        }
        public virtual void Init() { }
        public virtual void Update() { }
    }
}
