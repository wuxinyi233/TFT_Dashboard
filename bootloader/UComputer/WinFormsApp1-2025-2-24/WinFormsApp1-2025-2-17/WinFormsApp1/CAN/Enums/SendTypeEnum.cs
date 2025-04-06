using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.CAN.Enums
{
    /// <summary>
    /// 发送类型
    /// </summary>
    public enum CANSendTypeEnum:byte
    {
        /// <summary>
        /// 自动重发
        /// </summary>
        AutoResend = 0x00,
        /// <summary>
        /// 发送一次
        /// </summary>
        OnceSend=0x01,
    }
}
