using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsApp1.CAN.Extension;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WinFormsApp1
{
    /// <summary>
    /// 文件处理类
    /// </summary>
    public class FileBinModel
    {
        private byte[] _bytes = [];
        /// <summary>
        /// 包列表
        /// </summary>
        public List<DataPacket> DataPacketList { get; set; } = [];
        /// <summary>
        /// 总包数
        /// </summary>
        public ushort TotalDataPacketCount
        {
            get
            {
                ushort totalDataPacketCount = (ushort)(_bytes.Length / DataPacket.MaxDataLength);
                if (_bytes.Length % DataPacket.MaxDataLength > 0)
                {
                    totalDataPacketCount++;
                }
                return totalDataPacketCount;
            }
        }
        /// <summary>
        /// 总数据长度,单位字节
        /// </summary>
        public int TotalDataLength => _bytes.Length;
        /// <summary>
        /// 文件处理类
        /// </summary>
        /// <param name="bytes"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public FileBinModel(byte[]? bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException("文件内容不能为空");
            }
            _bytes = bytes;
        }
        /// <summary>
        /// 数据初始化
        /// </summary>
        public void DataPacketInit()
        {
            int index = 0;
            byte packetFrameNo = 0;
            DataPacketList.Clear();
            //总包数
            ushort totalDataPacketCount = TotalDataPacketCount;
            while (index < _bytes.Length)
            {
                var packModel = new DataPacket(packetFrameNo++);
                var itemBytes = _bytes.GetBytes(index, DataPacket.MaxDataLength);
                //获取一包的数据
                packModel.SetBytes(itemBytes);
                index += DataPacket.MaxDataLength;
                //packModel.TotalDataPacketCount = totalDataPacketCount;
                DataPacketList.Add(packModel);
            }
        }
        /// <summary>
        /// 序列化为数组
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            return [0xff, 0x01,  ..TotalDataLength.GetBytes(), 0xff, 0xff];        //..TotalDataPacketCount.GetBytes(),  
        }
        /// <summary>
        /// 转化为hex字符串
        /// </summary>
        /// <param name="splitStr"></param>
        /// <returns></returns>
        public string ToHexBytes(char splitStr = ' ')
        {
            return string.Join(splitStr, ToBytes().Select(s => s.ToString("X2")));
        }
    }
    /// <summary>
    /// 数据包
    /// </summary>
    public class DataPacket
    {
        /// <summary>
        /// 最大数据长度
        /// 7*254 = 1778个字节
        /// </summary>
        public const ushort MaxDataLength = 1778;
        /// <summary>
        /// 包编号，要求从0开始
        /// 最大长度65535 
        /// </summary>
        public ushort No { get; set; }
        /// <summary>
        /// 数据byte长度
        /// </summary>
        public ushort TotalDataLength { get; set; } = 0;
        /// <summary>
        /// 和校验
        /// </summary>
        public ushort DataCheckSum { get; set; }
        ///// <summary>
        ///// 总包数
        ///// </summary>
        //public ushort TotalDataPacketCount { get; set; }
        /// <summary>
        /// 数据帧
        /// </summary>
        public List<DataFrame> DataFrameList { get; set; } = [];
        /// <summary>
        /// 数据包
        /// </summary>
        /// <param name="no">编号</param>
        public DataPacket(ushort no)
        {
            No = no;
        }
        /// <summary>
        /// 序列化成字节
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            return [0x00,
                ..No.GetBytes(),
                ..TotalDataLength.GetBytes(),
                ..DataCheckSum.GetBytes(),
                0xff
            ];
        }
        public string ToHexBytes(char splitStr = ' ')
        {
            return string.Join(splitStr, ToBytes().Select(s => s.ToString("X2")));
        }
        /// <summary>
        /// 添加数据到byte
        /// </summary>
        /// <param name="bytes">数据</param>
        /// <returns></returns>
        public void SetBytes(byte[] bytes)
        {
            int index = 0;
            byte dataFrameNo = 1;
            DataFrameList.Clear();
            while (index < bytes.Length)
            {
                var dataFrame = new DataFrame(dataFrameNo++);
                dataFrame.SetBytes(bytes.GetBytes(index, 7));
                index += 7;
                DataFrameList.Add(dataFrame);
            }
            TotalDataLength = (ushort)bytes.Length;
            DataCheckSum = GetCheckSum(bytes);
        }
        /// <summary>
        /// 获取累加和
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private ushort GetCheckSum(IEnumerable<byte> bytes)
        {
            ushort checkSum = 0;
            foreach (var item in bytes)
            {
                checkSum += item;
            }
            return checkSum;
        }
    }
    /// <summary>
    /// 数据帧
    /// </summary>
    public class DataFrame
    {
        /// <summary>
        /// 数据帧序号
        /// </summary>
        public byte No { get; set; }
        /// <summary>
        /// 数据内容
        /// </summary>
        public byte[] Data { get; set; } = new byte[7];
        /// <summary>
        /// 序列号为字节数组
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            return [No, .. Data];
        }
        /// <summary>
        /// 数据帧
        /// </summary>
        /// <param name="no">编号</param>
        public DataFrame(byte no)
        {
            No = no;
        }
        /// <summary>
        /// 添加数据到byte
        /// </summary>
        /// <param name="bytes">数据</param>
        /// <returns></returns>
        public void SetBytes(byte[] bytes)
        {         
            if (bytes.Length < 7)
            {
                var list = new List<byte>();
                for (int i = 0; i < 7 - bytes.Length; i++)
                {
                    list.Add(0xff);
                }
                Data = [.. bytes , .. list];
            }
            else
            {
                Data = bytes;
            }
        }
      
        public string ToHexBytes(char splitStr = ' ')
        {
            return string.Join(splitStr, ToBytes().Select(s => s.ToString("X2")));
        }
    }
}
