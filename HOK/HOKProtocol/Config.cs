using System;
using System.Collections.Generic;
using System.Text;

namespace HOKProtocol
{
    /// <summary>
    /// 通用配置数据
    /// </summary>
    public class ServerConfig
    {
        //正常 局域网ip
        //public const string LocalDevInnerIP = "192.168.1.100";
        //公司局域网ip  10.95.214.75  192.168.255.10
        //public const string LocalDevInnerIP = "192.168.255.10";

        //家里的局域网
        //public const string LocalDevInnerIP = "192.168.1.101";
        public const string LocalDevInnerIP = "192.168.0.101";

        //服务器IP
        //公网
        public const string RemoteGateIP = "49.232.214.145";
        //内网
        public const string RemoteServerIP = "172.21.0.5";

        /// <summary>
        /// 网络端口号
        /// </summary>
        public const int UdpPort = 17666;

        //确认匹配倒计时:15秒
        public const int ConfirmCountDown = 15;

        //选择英雄倒计时
        public const int SelectCountDown = 15;

        /// <summary>
        /// 服务器逻辑帧时间：ms
        /// </summary>
        public const int ServerLogicFrameIntervelMs = 66;
    }

    public class Configs
    {
        /// <summary>
        /// 秒钟
        /// </summary>
        public const float ClientLogicFrameDeltaSec = 0.066f;
    }
}
