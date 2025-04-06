using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.CAN.Extension
{
    public static class StringExtension
    {
        /// <summary>
        /// 转化为字节数组，按照16进制进行转化
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] ToHexByteArray(this string str)
        {
            return str.Split(new char[] { ' ' }).Select(s=> Convert.ToByte(s,16)).ToArray();
        }
     
    }
}
