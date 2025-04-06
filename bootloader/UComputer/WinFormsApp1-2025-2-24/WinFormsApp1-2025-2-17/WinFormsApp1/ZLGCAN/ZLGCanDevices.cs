using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.ZLGCAN
{
    public class ZLGCanDevices
    {
        ///// <summary>
        /////ZLG设备信息
        ///// </summary>
        ///// <param name="Name"></param>
        ///// <param name="Value"></param>
        //public record ZLGCanDevice(string Name, DeviceInfo Value);
        /// <summary>
        /// 返回周立功can设备信息
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, DeviceInfo> GetZLGCanDevices()
        {
            return new Dictionary<string, DeviceInfo>
            {
                //{ "ZCAN_USBCAN1", new DeviceInfo(Define.ZCAN_USBCAN1, 1) },
                { "ZCAN_USBCAN2", new DeviceInfo(Define.ZCAN_USBCAN2, 2) }
            };
        }
        /// <summary>
        /// 获取指定范围 int列表
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static string[] GetZLGCanIndexListString(int start, int end)
        {
            return Enumerable.Range(start, end).Select(s=>s.ToString()).ToArray();
        }

    }
}
