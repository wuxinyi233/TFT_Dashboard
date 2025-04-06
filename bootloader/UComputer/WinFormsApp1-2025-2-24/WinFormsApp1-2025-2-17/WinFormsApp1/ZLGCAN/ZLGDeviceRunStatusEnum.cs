using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.ZLGCAN
{
    /// <summary>
    /// 周立功设备运行状态
    /// </summary>
    public enum ZLGDeviceRunStatusEnum
    {
        None = 0,
        /// <summary>
        /// 已打开设备
        /// </summary>
        OpenedDevice,
        /// <summary>
        /// 已连接设备
        /// </summary>
        ConnectedDevice,
    }
}
