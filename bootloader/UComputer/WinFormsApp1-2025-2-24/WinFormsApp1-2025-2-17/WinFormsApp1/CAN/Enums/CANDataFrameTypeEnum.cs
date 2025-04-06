using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.CAN.Enums
{
    /// <summary>
    /// CAN数据帧类型
    /// </summary>
    public enum CANDataFrameTypeEnum
    {
        /// <summary>
        /// 数据帧
        /// </summary>
        Data = 1,
        /// <summary>
        /// 远程帧
        /// </summary>
        Remote = 2,
    }
}
