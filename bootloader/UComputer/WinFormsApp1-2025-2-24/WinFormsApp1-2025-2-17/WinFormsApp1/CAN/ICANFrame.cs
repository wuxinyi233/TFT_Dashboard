using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.CAN
{
    public interface ICANFrame
    {
        /// <summary>
        /// 序列化为数组
        /// </summary>
        /// <returns></returns>
        byte[] ToBytes();
        /// <summary>
        /// 转化为16进制
        /// </summary>
        /// <returns></returns>
        string ToHexBytes(char splitStr = ' ');
    }
}
