using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.CAN.Enums
{
    /// <summary>
    /// 波特率
    /// </summary>
    public enum CANBaudRateEnum : byte
    {
        CAN_1Mbps = 1,
        CAN_800kps = 2,
        CAN_500kps = 3,
        CAN_400kps = 4,
        CAN_250kps = 5,
        CAN_200kps = 6,
        CAN_125kps = 7,
        CAN_100kps = 8,
        CAN_50kps = 9,
        CAN_20kps = 10,
        CAN_10kps = 11,
        CAN_5kps = 12,
    }
}
