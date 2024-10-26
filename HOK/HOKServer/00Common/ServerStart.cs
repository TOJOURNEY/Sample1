using System;
using System.Threading;



namespace HOKServer
{
    /// <summary>
    /// 服务器启动入口
    /// </summary>
    class ServerStart
    {
        static void Main(string[] args)
        {
            ServerRoot.Instance.Init();

            while (true)
            {
                ServerRoot.Instance.Update();
                Thread.Sleep(10);
            }
        }
    }
}
