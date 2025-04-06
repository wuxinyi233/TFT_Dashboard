using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using WinFormsApp1.CAN.Enums;
using WinFormsApp1.CAN.Extension;

namespace WinFormsApp1.CAN
{
    /// <summary>
    /// 微雪Can数据帧
    /// </summary>
    public class CANDataFrame : ICANFrame
    {
        /// <summary>
        /// 前导符0
        /// </summary>
        public const byte Preamble0 = 0xAA;
        /// <summary>
        /// 前导符1
        /// </summary>
        public const byte Preamble1 = 0x55;
        ///// <summary>
        ///// 类型
        ///// </summary>
        //public const byte DataType = 0x01;
        /// <summary>
        /// 帧类型
        /// </summary>
        public CANFrameTypeEnum CANFrameType { get; set; } = CANFrameTypeEnum.Extend;
        /// <summary>
        /// 数据帧
        /// </summary>
        public CANDataFrameTypeEnum CANDataFrameType { get; set; } = CANDataFrameTypeEnum.Data;
        /// <summary>
        /// 帧ID
        /// </summary>
        public uint ID { get; set; } = 0;
        /// <summary>
        /// 帧数据长度
        /// </summary>
        public byte DataLength { get; set; } = 8;
        /// <summary>
        /// 帧数据
        /// </summary>
        public byte[] Data { get; set; } = new byte[8];
        /// <summary>
        /// 保留位
        /// </summary>
        public byte Reserve0 { get; set; } = 0;

        /// <summary>
        /// 序列化为数组
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            var IDBytes = ID.GetBytes();//默认为大端模式

            //if (BitConverter.IsLittleEndian) // 若为小端模式
            {
                Array.Reverse(IDBytes); // 转换为 小端模式               
            }
            List<byte> bytes =
            [
                //bytes.Add(Preamble0);
                //bytes.Add(Preamble1);
                0x01,
                (byte)CANFrameType,
                (byte)CANDataFrameType,
                .. IDBytes,
                DataLength,
                .. Data,
                Reserve0,
            ];
            bytes.Add(GetCheckSum(bytes));
            bytes.Insert(0, Preamble1);
            bytes.Insert(0, Preamble0);
            return bytes.ToArray();
        }

        public bool FrameCheck(byte[] bytes)
        {
            //寻找AA和55
            var indexAA = 0;
            for (int i = 0; i < bytes.Length; i++)
            {
                if (Preamble0 == bytes[i])
                {
                    indexAA = i;
                    if (bytes[i + 1] == Preamble1)
                    {
                        break;
                    }
                }
            }
            var byteList = new List<byte>();
            for (int i = indexAA+2; i < bytes.Length; i++)
            {
                byteList.Add(bytes[i]);
            }
            //如果长度不等于18,则直接退出
            if (byteList.Count < 18)
            {
                return false;
            }
            int index = 1;
            CANFrameType = (CANFrameTypeEnum)byteList[index++];
            CANDataFrameType = (CANDataFrameTypeEnum)byteList[index++];
            var idList = byteList.GetRange(index, 4).ToArray();
            index += 4;
            Array.Reverse(idList);
            ID = idList.GetUInt();
            DataLength = byteList[index++];
            Data = byteList.GetRange(index, DataLength).ToArray();
            index += DataLength;
            Reserve0 = byteList[index++];
            //校验位检查
            if (byteList[index] == GetCheckSum(byteList.GetRange(0, index)))
            {
                return true;
            }
            return false;
        }

        public string ToHexBytes(char splitStr = ' ')
        {
            return string.Join(splitStr, ToBytes().Select(s => s.ToString("X2")));
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
