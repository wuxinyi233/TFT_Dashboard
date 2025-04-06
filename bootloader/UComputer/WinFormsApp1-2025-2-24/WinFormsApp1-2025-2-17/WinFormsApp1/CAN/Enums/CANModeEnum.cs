using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.CAN.Enums
{
    public enum CANModeEnum:byte
    {
        /// <summary>
        /// 正常模式
        /// </summary>
        CAN_MODE_NORMAL = 0,
        /// <summary>
        /// 静默模式
        /// </summary>
        CAN_MODE_SILENT = 1,
        /// <summary>
        /// 环回模式
        /// </summary>
        CAN_MODE_LOOPBACK = 2,
        /// <summary>
        /// 环回静默模式
        /// </summary>
        CAN_MODE_SILENT_LOOPBACK =3
    }
}
