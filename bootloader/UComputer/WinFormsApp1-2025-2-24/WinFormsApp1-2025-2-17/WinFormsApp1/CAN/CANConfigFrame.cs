using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsApp1.CAN.Enums;
using WinFormsApp1.CAN.Extension;

namespace WinFormsApp1.CAN
{
    /// <summary>
    /// 微雪Can配置帧
    /// </summary>
    public class CANConfigFrame : ICANFrame
    {
        /// <summary>
        /// 前导符0
        /// </summary>
        public const byte Preamble0 = 0xAA;
        /// <summary>
        /// 前导符1
        /// </summary>
        public const byte Preamble1 = 0x55;
        /// <summary>
        /// 配置类型
        /// </summary>
        public CANConfigTypeEnum CANConfigType { get; set; } = CANConfigTypeEnum.Fixed;
        /// <summary>
        /// 波特率
        /// </summary>
        public CANBaudRateEnum CANBaudRate { get; set; } = CANBaudRateEnum.CAN_1Mbps;
        /// <summary>
        /// 帧类型
        /// </summary>
        public CANFrameTypeEnum CANFrameType { get; set; } = CANFrameTypeEnum.Extend;
        /// <summary>
        /// 滤波ID
        /// </summary>
        public uint FilterID { get; set; } = 0;
        /// <summary>
        /// 屏蔽ID
        /// </summary>
        public uint MaskID { get; set; } = 0;
        /// <summary>
        /// 工作模式
        /// </summary>
        public CANModeEnum CANMode { get; set; } = CANModeEnum.CAN_MODE_NORMAL;
        /// <summary>
        /// 发送模式
        /// </summary>
        public CANSendTypeEnum SendType { get; set; } = CANSendTypeEnum.AutoResend;

        #region 备用
        public byte Reserve0 { get; set; } = 0;
        public byte Reserve1 { get; set; } = 0;
        public byte Reserve2 { get; set; } = 0;
        public byte Reserve3 { get; set; } = 0;
        #endregion 备用
        /// <summary>
        /// 和校验
        /// </summary>
        public byte CheckSum { get; set; } = 0;
        /// <summary>
        /// 序列化为数组
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            List<byte> bytes =
            [
                //bytes.Add(Preamble0);
                //bytes.Add(Preamble1);
                (byte)CANConfigType,
                (byte)CANBaudRate,
                (byte)CANFrameType,
                .. FilterID.GetBytes(),
                .. MaskID.GetBytes(),
                (byte)CANMode,
                (byte)SendType,
                Reserve0,
                Reserve1,
                Reserve2,
                Reserve3
            ];
            bytes.Add(GetCheckSum(bytes));
            bytes.Insert(0, Preamble1);
            bytes.Insert(0, Preamble0);
            return bytes.ToArray();
        }

        public string ToHexBytes(char splitStr = ' ')
        {
            return string.Join(splitStr, ToBytes().Select(s=>s.ToString("X2")));
        }

        /// <summary>
        /// 获取累加和
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private byte GetCheckSum(IEnumerable<byte> bytes)
        {
            byte checkSum = 0;
            foreach (var item in bytes)
            {
                checkSum += item;
            }
            return checkSum;
        }
    }
}
