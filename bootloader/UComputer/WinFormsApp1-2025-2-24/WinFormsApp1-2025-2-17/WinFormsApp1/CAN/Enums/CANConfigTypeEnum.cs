using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.CAN.Enums
{
    /// <summary>
    /// can配置类型
    /// </summary>
    public enum CANConfigTypeEnum:byte
    {
        /// <summary>
        /// 可变
        /// </summary>
        Auto = 0x12,
        /// <summary>
        /// 固定
        /// </summary>
        Fixed =0x02
    }
}
