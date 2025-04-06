using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.CAN.Extension
{
    public static class ObjectExtension
    {
        /// <summary>
        /// string转uint
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static uint ToUint(this object? value)
        {
            return Convert.ToUInt32(value);
        }
        /// <summary>
        /// string转int
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int ToInt(this object? value)
        {
            return Convert.ToInt32(value);
        }
    }
}
