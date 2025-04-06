using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.CAN.Enums
{
    /// <summary>
    /// 帧类型
    /// </summary>
    public enum CANFrameTypeEnum:byte
    {
        /// <summary>
        /// 标准帧
        /// </summary>
        Standard=1,
        /// <summary>
        /// 扩展帧
        /// </summary>
        Extend=2
    }
}
