using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.CAN.Extension
{
    public static class UShortExtension
    {
        public static byte[] GetBytes(this ushort value)
        {
            //byte[] byteTemp = new byte[]{ 0x76, 0x83, 0x33, 0x45 };
            var bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian) // 若为 小端模式
            {
                Array.Reverse(bytes); // 转换为 大端模式               
            }
            return bytes;
        }

    }
}
