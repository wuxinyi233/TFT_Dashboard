using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.CAN.Enums
{
    /// <summary>
    /// 接收状态
    /// </summary>
    public enum ReceivedStateEnum
    {
        /// <summary>
        /// 无
        /// </summary>
        None = 0,
        /// <summary>
        /// 准备
        /// </summary>
        Ready,
        /// <summary>
        /// 失败
        /// </summary>
        Fail,
        /// <summary>
        /// 成功
        /// </summary>
        Success,
        /// <summary>
        /// 握手准备
        /// </summary>
        HandshakeReady

    }
}
