using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.CAN.Extension
{
    public static class ByteArrayExtension
    {
        /// <summary>
        /// 获取字节数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static byte[] GetBytes(this byte[] bytes, int index, int length)
        {
            //var byteList = new List<byte>();
            //for (int i = index; i < index + length && i < bytes.Length; i++)
            //{
            //    byteList.Add(bytes[i]);
            //}
            return bytes.Skip(index).Take(length).ToArray();
        }
        /// <summary>
        /// 字节数组转化为uint
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static uint GetUInt(this byte[] bytes)
        {
            //byte[] byteTemp = new byte[]{ 0x76, 0x83, 0x33, 0x45 };
          
            if (BitConverter.IsLittleEndian) // 若为 小端模式
            {
                Array.Reverse(bytes); // 转换为 大端模式               
            }
            var value = BitConverter.ToUInt32(bytes, 0);
            return value;
        }
      
    }
}
